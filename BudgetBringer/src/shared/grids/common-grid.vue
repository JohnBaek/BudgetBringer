<script setup lang="ts" generic="T">
import {AgGridVue} from "ag-grid-vue3";
import {onMounted, ref} from "vue";
import {RequestQuery} from "../../models/requests/query/request-query";
import {HttpService} from "../../services/api-services/http-service";
import {EnumResponseResult} from "../../models/enums/enum-response-result";
import {ResponseList} from "../../models/responses/response-list";
import {communicationService} from "../../services/communication-service";
import CommonGridButtonGroup from "./common-grid-button-group.vue";
import {CommonGridButtonGroupDefinesButtonEmits} from "./common-grid-button-group-defines";
import {getDateFormatForFile} from "../../services/utils/date-util";
/**
 * Prop 정의
 */
const props = defineProps({
  /**
   * 컬럼정보
   */
  inputColumDefined : {
    Type: Array<T> ,
    required: true ,
    default: [] ,
    width : 50 ,
  },

  /**
   * Insert 그리드 사용여부
   */
  isUseInsert : {
    Type: Boolean ,
    required: false ,
    default: false
  },

  gridTitle: {
    Type: String,
    required: false,
    default: ''
  },

  /**
   * 사용할 버튼
   */
  showButtons: {
    Type: Array<string> ,
    default: ['add','update','delete','refresh', 'excel']
  },
  /**
   * 너비
   */
  width : {
    Type: String ,
    required: false ,
    default: '100%'
  },
  /**
   * 높이
   */
  height : {
    Type: String ,
    required: false ,
    default: '600px'
  },
  /**
   * 쿼리 리퀘스트
   */
  queryRequest: {
    type: Object as () => RequestQuery ,
    required: true ,
  }
});
/**
 * incoming calls from parent
 */
defineExpose({
  doRefresh() {
    refresh();
  } ,
});
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
 * 인서트 Grid 사용여부
 */
const isUseInsert = props.isUseInsert;
/**
 * 디테일 다이얼로그
 */
const detailDialogReference = ref(false);
/**
 * 입력 데이터
 */
let inputRow = {};
/**
 * Top 인서트 Pine
 */
const pinnedTopRowData = isUseInsert ? [inputRow] : [];
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
 * gridReady 이벤트 핸들러
 * @param params 파라미터
 */
const onGridReady = (params) => {
  gridApi.value = params.api;
  gridColumnApi.value = params.columnApi;
  gridParams.value = params;
  gridApi.value.setGridOption('datasource',dataSource);

  // 필터 이벤트를 핸들링한다.
  gridApi.value.addEventListener('filterChanged', () => {
    console.log("[filterChanged]");
    changedFilter();
  });

  // Sorting 이벤트를 핸들링한다.
  gridApi.value.addEventListener('sortChanged', () => {
    changeSort();
  });
};

/**
 * 소팅 변경시
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


// 그리드 인피니티 스크롤 관련 기본 설정값
const rowModelType = 'infinite';
const rowBuffer = 0;
const cacheBlockSize = queryRequest.pageCount;
const cacheOverflowSize = 2;
const maxConcurrentDatasourceRequests = 1;
const infiniteInitialRowCount = 100;
const maxBlocksInCache = 10;

const createSkeletonData = (columnDefinitions, numberOfRows) => {
  console.log('columnDefinitions',columnDefinitions);

  return Array.from({length: numberOfRows}, () => {
    const row = {};
    columnDefinitions.forEach(col => {
      row[col.field] = 'loading...'; // 각 필드에 'loading...' 값을 할당
    });
    return row;
  });
};

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
 * 빈 탑 핀 컬럼 셀
 * @param params
 */
const isEmptyPinnedCell = (params) => {
  if(params.node == null)
    return false;

  return (
    (params.node.isRowPinned() && params.value == null) ||
    (params.node.isRowPinned() && params.value === '')
  );
}
/**
 * 빈 핀 컬럼의 컬럼명을 조정한다.
 * @param colDefined
 */
const createPinnedCellPlaceholder = (colDefined : any) => {
  return colDefined.colDef.headerName + ' 입력..';
}
/**
 * 기본 그리드 컬럼 설정
 * * Insert 그리드 사용시만 초기화 *
 */
const defaultColDefined = isUseInsert ? {
  flex: 1,
  valueFormatter: (params) =>
    isEmptyPinnedCell(params) ?
      createPinnedCellPlaceholder(params) : undefined,
} : {};
/**
 * 그리드의 셀렉트가 변경되었을때
 */
const onSelectionChanged = () => {
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
  communicationService.notifyInCommunication();

  // Copy Reference
  const _queryRequest = Object.assign({}, queryRequest);

  // Manipulate Range conditions
  _queryRequest.skip = 0;
  _queryRequest.pageCount = 1000000;

  // Request to Server
  HttpService.requestGetFile(`${queryRequest.apiUri}/export/excel` , _queryRequest).subscribe({
    next(response) {
      if(response == null)
        return;

      // Create URL dummy link
      const url = window.URL.createObjectURL(response);

      // Create Anchor dummy
      const link = document.createElement('a');

      // Simulate Click
      link.href = url;
      link.setAttribute('download', `${getDateFormatForFile()}_${props.gridTitle}.xlsx`);
      document.body.appendChild(link);
      link.click();

      // Remove Dummy
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    },
    error(err) {
      console.error('Error loading data', err);
    },
    complete() {
      // 커뮤니케이션 시작
      communicationService.notifyOffCommunication();
    },
  });
}
/**
 * 새로고침 명령
 */
const refresh = () => {
  emits('onRefresh');

  // 최초 상태의 조회 조건을 복원한다.
  queryRequest = Object.assign({},props.queryRequest);
  items = [];

  // 스크롤 위치를 맨 위로 초기화
  gridApi.value.ensureIndexVisible(0);

  // 캐싱 날리기
  gridApi.value.purgeInfiniteCache();
}

let detailData = ref();

const toPascalCase = (str) => {
  return str
  .replace(new RegExp(/[-_]+/, 'g'), ' ') // 대시와 언더스코어를 공백으로 대체
  .replace(new RegExp(/[^\w\s]/, 'g'), '') // 문자와 공백이 아닌 모든 것 제거
  .replace(
    /\s+(.)(\w*)/g, // 각 단어를 대문자로 시작하도록 변경
    (_, firstChar, rest) => firstChar.toUpperCase() + rest.toLowerCase()
  )
  .replace(/\s/g, '') // 공백 제거
  .replace(
    /^./, // 첫 글자도 대문자로
    str => str.toUpperCase()
  );
}

/**
 * 셀 더블 클릭시
 */
const onCellDoubleClicked = (event) => {
  emits('onDoubleClicked', event.data);
}

/**
 * 셀 클릭시
 * @param event
 */
const onCellClicked = (event) => {
  emits('onCellClicked', event.data);
}

/**
 * 온마운트 핸들링
 */
onMounted(() => {
});
</script>

<template>
  <v-row class="mt-1 mb-1">
    <v-col>
      <div class="mt-2">
        <!-- Action Buttons -->
        <common-grid-button-group
          :selected-rows="selectedRows"
          :show-buttons="showButtons"
          @on-add="add()"
          @on-remove="remove()"
          @on-update="update()"
          @on-refresh="refresh()"
          @on-export-excel="exportExcel()"
        />
      </div>
    </v-col>
  </v-row>

  <!-- Grid -->
  <ag-grid-vue
    @grid-ready="onGridReady"
    @selection-changed="onSelectionChanged"
    @cell-double-clicked="onCellDoubleClicked"
    @cell-clicked="onCellClicked"
    class="ag-theme-alpine"
    :columnDefs="columDefined"
    :defaultColDef="defaultColDefined"
    :rowBuffer="rowBuffer"
    :rowSelection="'multiple'"
    :rowModelType="rowModelType"
    :cacheBlockSize="cacheBlockSize"
    :cacheOverflowSize="cacheOverflowSize"
    :maxConcurrentDatasourceRequests="maxConcurrentDatasourceRequests"
    :infiniteInitialRowCount="infiniteInitialRowCount"
    :maxBlocksInCache="maxBlocksInCache"
    :pinnedTopRowData="pinnedTopRowData"
    :getRowStyle="getRowStyle"
    :style="{ width, height }"
  >
  </ag-grid-vue>

  <!--데이터 수정 다이얼로그-->
  <v-dialog v-model="detailDialogReference" width="80%">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>상세정보</h4>
      </v-card-title>
      <v-row dense>
        <v-col cols="12" md="12" lg="12">
          <div v-for="(value, key) in detailData" :key="key">
            <div v-if="key.toString().toLowerCase().indexOf('id') == -1">
              <!-- "content"가 포함된 키에 대해 v-textarea 사용 -->
              <v-textarea
                v-if="key.toString().toLowerCase().indexOf('content') > -1"
                :label="toPascalCase(key)"
                v-model="detailData[key]"
                variant="outlined"
                auto-grow
                readonly>
              </v-textarea>

              <!-- "content"가 포함되지 않은 나머지 키에 대해 v-text-field 사용 -->
              <v-text-field
                v-else
                :label="toPascalCase(key)"
                v-model="detailData[key]"
                variant="outlined"
                auto-grow
                readonly>
              </v-text-field>
            </div>
          </div>
        </v-col>
        <v-col>
          <v-btn variant="outlined" @click="detailDialogReference = false" class="mr-2" color="error">닫기</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style lang="css" scoped>
</style>
