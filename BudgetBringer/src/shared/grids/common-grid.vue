<script setup lang="ts" generic="T">
import {AgGridVue} from "ag-grid-vue3";
import {ref} from "vue";
import {RequestQuery} from "../../models/requests/query/request-query";
import {HttpService} from "../../services/api-services/http-service";
import {EnumResponseResult} from "../../models/enums/enum-response-result";
import {ResponseList} from "../../models/responses/response-list";
import {communicationService} from "../../services/communication-service";
import CommonGridButtonGroup from "./common-grid-button-group.vue";
import {CommonGridButtonGroupDefinesButtonEmits} from "./common-grid-button-group-defines";
import {getDateFormatForFile} from "../../services/utils/date-util";
import {toClone} from "../../services/utils/object-util";
import {CommonButtonDefinitions, CommonGridButton} from "./common-grid-button";

// Props
const props = defineProps({
  inputColumDefined : {Type: Array<T> , required: true , default: [] , width : 50 ,},
  title : {},
  width : { Type: String ,  required: false ,  default: '100%' },
  height : { Type: String ,  required: false ,  default: '600px' },
  queryRequest: {type: Object as () => RequestQuery , required: true ,},
  showButtons: {
    Type: Array<CommonGridButton>,
    default: [
      CommonButtonDefinitions.add,
      CommonButtonDefinitions.remove,
      CommonButtonDefinitions.update,
      CommonButtonDefinitions.refresh,
    ],
  }
});
/**
 * incoming calls from parent
 */
defineExpose({
  // When got refresh command from parent
  doRefresh() { gridRefresh(); },
  showAddDialog() { addDialog.value = true }
});
const addDialog = ref(false);

let items = [];
/**
 * 쿼리 요청을 복사한다.
 */
let queryRequest: RequestQuery = Object.assign({},props.queryRequest) ;
/**
 * Current selected rows list.
 */
const selectedRows = ref([]);
/**
 * Defines dispatches.
 */
const emits = defineEmits<CommonGridButtonGroupDefinesButtonEmits>();
/**
 * 그리드의 column 데이터
 */
const columDefined = ref([...props.inputColumDefined]);
/**
 * Grid API
 */
const gridApi = ref();
/**
 * Grid Column API
 */
const gridColumnApi = ref(null);
/**
 * 그리드 파라미터
 */
const gridParams = ref(null);

/**
 * gridReady Event Handler
 * @param params Parameters
 */
const onGridReady = (params) => {
  gridApi.value = params.api;
  gridColumnApi.value = params.columnApi;
  gridParams.value = params;
  gridApi.value.setGridOption('datasource',dataSource);

  // 필터 이벤트를 핸들링한다.
  gridApi.value.addEventListener('filterChanged', () => {
    changedFilter();
  });

  // Sorting 이벤트를 핸들링한다.
  gridApi.value.addEventListener('sortChanged', () => {
    changeSort();
  });
};
/**
 * When change the sorts
 */
const changeSort = () => {
  // Sort 모델을 가져온다.
  const models = gridApi.value.getColumnState().filter(s => s.sort !== null);

  // 쿼리 정보 복원을 위한
  const queryRequestCloned: RequestQuery = Object.assign({},props.queryRequest) ;

  // 검색정보를 초기화한다.
  queryRequest.skip = queryRequestCloned.skip;
  queryRequest.sortOrders = [];
  queryRequest.sortFields = [];

  // 모든 sort 에 대해처리
  for (let i=0; i<models.length; i++) {
    const model = models[i];
    queryRequest.sortFields.push(model.colId);
    queryRequest.sortOrders.push(model.sort);
  }

  // 스크롤 위치를 맨 위로 초기화
  gridApi.value.ensureIndexVisible(0);

  // 데이터를 초기화한다.
  items = [];
}
/**
 * 필터링 변경시
 */
const changedFilter = () => {
  // 필터링 모델을 가져온다.
  const filterModel = gridApi.value.getFilterModel();

  // 쿼리 정보 복원을 위한
  const queryRequestCloned: RequestQuery = Object.assign({},props.queryRequest) ;

  // 검색정보를 초기화한다.
  queryRequest.searchFields = queryRequestCloned.searchFields.slice();
  queryRequest.searchKeywords = queryRequestCloned.searchKeywords.slice();
  queryRequest.skip = queryRequestCloned.skip;

  // 데이터를 초기화한다.
  items = [];

  // 스크롤 위치를 맨 위로 초기화
  gridApi.value.ensureIndexVisible(0);

  // 모든 필터에 대해 처리한다.
  for (const key in filterModel) {
    if (Object.prototype.hasOwnProperty.call(filterModel, key)) {
      // 현재 키(필드 이름)에 대한 필터 객체를 얻습니다.
      const filterObject = filterModel[key];

      queryRequest.searchFields.push(key);
      queryRequest.searchKeywords.push(filterObject.filter);
    }
  }
}
/**
 * Settings for Infinite Grid
 */
const gridSettings = ref({
  rowModelType : 'infinite',
  rowBuffer : 0,
  cacheBlockSize : queryRequest.pageCount,
  cacheOverflowSize : 2,
  maxConcurrentDatasourceRequests : 1,
  infiniteInitialRowCount : 100,
  maxBlocksInCache : 10,
});
/**
 * 데이터 소스 정의
 */
const dataSource = {
  getRows: (params) => {
    communicationService.notifyInCommunication();

    // 서버로부터 데이터를 요청하는 URL 구성
    HttpService.requestGet<ResponseList<any>>(queryRequest.apiUri , queryRequest).subscribe({
      next(response) {
        if(response == null)
          return;

        if(response.result === EnumResponseResult.success) {
          // 마지막 행
          let lastRow = -1;

          // 행이 남아있지 않은경우
          if (response.totalCount <= params.endRow) {
            lastRow = response.totalCount;
          }

          response.items.forEach(i => items.push(i));
          const rows = items.slice(params.startRow, params.endRow);
          queryRequest.skip += queryRequest.pageCount;

          // 성공 콜백 호출, 데이터와 마지막 로우 인덱스 전달
          params.successCallback(rows, lastRow);
        } else {
          // 실패 처리
          params.successCallback([], 0);
        }

      },
      error(err) {
        console.error('Error loading data', err);
        params.successCallback([], 0);
      },
      complete() {
        // 커뮤니케이션 시작
        communicationService.notifyOffCommunication();
      },
    });
  }
};
/**
 * pined 된 행의 스타일 정보
 * @param node
 */
const getRowStyle = ({ node }) =>
  node.rowPinned ? {  fontStyle: 'italic' , fontWeight: 'bold', color:'gray' } : {};
/**
 * 그리드의 셀렉트가 변경되었을때
 */
const gridSelectionChanged = () => {
  // 선택된 Row 를 업데이트한다.
  selectedRows.value = gridApi.value.getSelectedRows();
};
/**
 * dispatch add
 */
const add = () => {
  emits('onAdd');
}
/**
 * dispatch remove
 */
const remove = () => {
  const selectedRows = gridApi.value.getSelectedRows();
  emits('onRemove' , selectedRows);
}
/**
 * dispatch update
 */
const update = () => {
  // Get Selected rows
  const selectedRows = gridApi.value.getSelectedRows();

  // Has not selected rows
  if(selectedRows.length == 0)
    return;

  emits('onUpdate' , selectedRows[0]);
}
/**
 * Request to server for excel
 */
const exportExcel = () => {
  // Copy Reference
  const _queryRequest = Object.assign({}, queryRequest);

  // Manipulate Range conditions
  _queryRequest.skip = 0;
  _queryRequest.pageCount = 1000000;

  // Request to Server
  HttpService.requestGetFileAutoNotify(`${queryRequest.apiUri}/export/excel` , _queryRequest).subscribe({
    next(response) {
      if(response == null)
        return;

      // Create URL dummy link
      const url = window.URL.createObjectURL(response);

      // Create Anchor dummy
      const link = document.createElement('a');

      // Simulate Click
      link.href = url;
      link.setAttribute('download', `${getDateFormatForFile()}_.xlsx`);
      document.body.appendChild(link);
      link.click();

      // Remove Dummy
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    },
  });
}
/**
 * 새로고침 명령
 */
const gridRefresh = () => {
  emits('onRefresh');

  // 최초 상태의 조회 조건을 복원한다.
  queryRequest = toClone(props.queryRequest);
  items = [];

  // 스크롤 위치를 맨 위로 초기화
  gridApi.value.ensureIndexVisible(0);

  // 캐싱 날리기
  gridApi.value.purgeInfiniteCache();
}
/**
 * 셀 더블 클릭시
 */
const gridCellDoubleClicked = (event) => {
  emits('onDoubleClicked', event.data);
}
/**
 * 셀 클릭시
 * @param event
 */
const gridCellClicked = (event) => {
  emits('onCellClicked', event.data);
}
/**
 * imported file
 * @param event
 */
const importFile = (event) => {
  emits('importFile', event);
}
const importFileDownload = () =>{
  emits('importExcelDownload');
}



</script>

<template>
  <!-- Action Buttons -->
  <v-row class="mt-1 mb-1">
    <v-col>
      <div class="mt-2">
        <common-grid-button-group
          :selected-rows="selectedRows"
          :show-buttons="showButtons"
          @on-add="add()"
          @on-remove="remove()"
          @on-update="update()"
          @on-refresh="gridRefresh()"
          @on-export-excel="exportExcel()"
          @import-file="importFile($event)"
          @import-excel-download="importFileDownload()"
        />
      </div>
    </v-col>
  </v-row>

  <!-- Grid -->
  <ag-grid-vue
    @grid-ready="onGridReady"
    @selection-changed="gridSelectionChanged"
    @cell-double-clicked="gridCellDoubleClicked"
    @cell-clicked="gridCellClicked"
    class="ag-theme-alpine"
    :columnDefs="columDefined"
    :rowSelection="'multiple'"
    :rowBuffer="gridSettings.rowBuffer"
    :rowModelType="gridSettings.rowModelType"
    :cacheBlockSize="gridSettings.cacheBlockSize"
    :cacheOverflowSize="gridSettings.cacheOverflowSize"
    :maxConcurrentDatasourceRequests="gridSettings.maxConcurrentDatasourceRequests"
    :infiniteInitialRowCount="gridSettings.infiniteInitialRowCount"
    :maxBlocksInCache="gridSettings.maxBlocksInCache"
    :getRowStyle="getRowStyle"
    :style="{ width, height }"
  >
  </ag-grid-vue>
</template>

<style lang="css">
.disabled-row {
  background-color: lightgray; /* 비활성화된 행의 배경색을 변경합니다. */
}
</style>
