<script setup lang="ts" generic="T">
import {AgGridVue} from "ag-grid-vue3";
import {onMounted, ref} from "vue";
import {RequestQuery} from "../../models/requests/query/request-query";
import {HttpService} from "../../services/api-services/http-service";
import {EnumResponseResult} from "../../models/enums/enum-response-result";
import {ResponseList} from "../../models/responses/response-list";


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
    required: true ,
    default: false
  },
  /**
   * Delete 그리드 사용여부
   */
  isUseButtons: {
    Type: Boolean ,
    required: true ,
    default: false
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
  queryRequest: {
    type: RequestQuery ,
    required: true ,
  }
});

let items = [];

/**
 * 쿼리 요청을 복사한다.
 */
let queryRequest: RequestQuery = Object.assign({},props.queryRequest) ;
/**
 * 선택된 row
 */
const selectedRows = ref([]);
/**
 * emit 정의
 */
const emits = defineEmits<{
  // 신규 데이터가 추가되었을때
  (e: 'onNewRowAdded', params): any,

  // 추가 버튼 클릭
  (e: 'onAdd'): any,

  // 삭제 버튼 클릭
  (e: 'onRemove', params): any,

  // 업데이트 버튼 클릭
  (e: 'onUpdate'): any,

  // 리프레쉬 버튼 클릭
  (e: 'onRefresh'): any,
}>();
defineExpose({
  doRefresh() {
    console.log('defineExpose','doRefresh')
    refresh();
  } ,
});

/**
 * 통신중 여부
 */
const inCommunication  = ref(false);
/**
 * 그리드의 column 데이터
 */
const columDefined = ref([...props.inputColumDefined]);
/**
 * 인서트 Grid 사용여부
 */
const isUseInsert = props.isUseInsert;
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

  // 데이터 소스를 바인딩한다.
  gridApi.value.setDatasource(dataSource);
};


// 그리드 인피니티 스크롤 관련 기본 설정값
const rowModelType = 'infinite';
const rowBuffer = 0;
const cacheBlockSize = queryRequest.pageCount;
const cacheOverflowSize = 2;
const maxConcurrentDatasourceRequests = 1;
const infiniteInitialRowCount = 100;
const maxBlocksInCache = 10;

/**
 * 데이터 소스 정의
 */
const dataSource = {
  getRows: (params) => {
    // 서버로부터 데이터를 요청하는 URL 구성
    HttpService.requestGet<ResponseList<any>>(queryRequest.apiUri , queryRequest).subscribe({
      next(response) {

        console.log('common grid response',response);


        if(response.result === EnumResponseResult.success) {
          // 마지막 행
          let lastRow = -1;

          // 행이 남아있지 않은경우
          if (response.totalCount <= params.endRow) {
            lastRow = response.totalCount;
          }

          response.items.forEach(i => items.push(i));
          const rows = items.slice(params.startRow, params.endRow);
          console.log('items',items);
          console.log('rows',rows);


          queryRequest.skip += queryRequest.pageCount;

          console.log('queryRequest.skip', queryRequest.skip);
          console.log('queryRequest.pageCount', queryRequest.pageCount);

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
      }
    });
  }
};

/**
 * 데이터 입력 완료 판별
 * @param params 파라미터
 */
const isPinnedRowDataCompleted = (params) => {
  // 최상위 로우가 아닌경우
  if (params.rowPinned !== 'top'){
    return;
  }
  // 모든 Row 가 입력되었다면 true
  return columDefined.value.filter(i => i.field != '')
              .every((def) => inputRow[def.field]);
}

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
//
// /**
//  * Cell 이 열렸다 닫혔을때
//  * @param params Cell 파라미터
//  */
// const onCellEditingStopped = (params) => {
//   // 인서트 그리드를 사용하지 않는경우 제외한다.
//   if(!isUseInsert)
//     return;
//
//   // 데이터 입력이 완료된경우
//   if (isUseInsert && isPinnedRowDataCompleted(params)) {
//
//     // 최상단에 추가된 데이터를 추가한다.
//     items.value = [inputRow,...items.value];
//
//     // 부모 컴포넌트에게 전달
//     emits('onNewRowAdded' , inputRow);
//     inputRow = {};
//     // 최상단 Row 를 초기화한다.
//     params.api.setPinnedTopRowData([inputRow]);
//   }
// }

/**
 * 그리드의 셀렉트가 변경되었을때
 */
const onSelectionChanged = () => {
  // 선택된 Row 를 업데이트한다.
  selectedRows.value = gridApi.value.getSelectedRows();
  console.log('onSelectionChanged' , selectedRows);
};

/**
 * 추가 팝업 명령
 */
const add = () => {
    emits('onAdd');
}

/**
 * 삭제 명령
 */
const remove = () => {
  const selectedRows = gridApi.value.getSelectedRows();
  emits('onRemove' , selectedRows);
}

/**
 * 수정 팝업 명령
 */
const update = () => {
    emits('onUpdate');
}

/**
 * 새로고침 명령
 */
const refresh = () => {
  emits('onRefresh');

  // 최초 상태의 조회 조건을 복원한다.
  queryRequest = Object.assign({},props.queryRequest);
  items = [];
  gridApi.value.refreshInfiniteCache();

  // gridApi.value.purgeInfiniteCache();
  console.log('Refresh Grid');
}

/**
 * 온마운트 핸들링
 */
onMounted(() => {
});
</script>

<template>
  <v-row class="mt-1 mb-1" v-if="isUseButtons">
    <v-col>
      <div class="mt-2">
        <v-btn variant="outlined" @click="add()" class="mr-2" color="info">추가</v-btn>
        <v-btn variant="outlined" @click="remove()" class="mr-2" color="error" :disabled="selectedRows.length == 0">삭제</v-btn>
        <v-btn variant="outlined" @click="update()" :disabled="selectedRows.length == 0" color="warning">수정</v-btn>
        <v-icon @click="refresh()" class="ml-3" size="x-large" color="green" style="cursor: pointer;">mdi-refresh-circle</v-icon>
        <v-spacer class="mt-1"></v-spacer>
        <span class="text-grey">그리드를 shift 버튼을 누른채로 클릭하면 여러 행을 선택할수 있습니다.</span>
      </div>
    </v-col>
  </v-row>

  <ag-grid-vue
    style="width: 100%; height: 600px;"
    :columnDefs="columDefined"
    @grid-ready="onGridReady"
    :defaultColDef="defaultColDefined"
    :rowBuffer="rowBuffer"
    :rowSelection="'multiple'"
    :rowModelType="rowModelType"
    :cacheBlockSize="cacheBlockSize"
    :cacheOverflowSize="cacheOverflowSize"
    :maxConcurrentDatasourceRequests="maxConcurrentDatasourceRequests"
    :infiniteInitialRowCount="infiniteInitialRowCount"
    :maxBlocksInCache="maxBlocksInCache"
    class="ag-theme-alpine"
    :pinnedTopRowData="pinnedTopRowData"
    :getRowStyle="getRowStyle"
    :style="{ width, height }"
    @selection-changed="onSelectionChanged"
  >
  </ag-grid-vue>
</template>

<style lang="css" >
.ag-grid-custom-header {
  white-space: normal;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: "Roboto", sans-serif;
  font-weight: bold;
  font-size: 13px;
}

@keyframes skeleton-loading {
  0% {
    background-color: rgba(165, 165, 165, 0.1);
  }
  50% {
    background-color: rgba(165, 165, 165, 0.3);
  }
  100% {
    background-color: rgba(165, 165, 165, 0.1);
  }
}

.skeleton-cell {
  animation: skeleton-loading 1.5s infinite ease-in-out;
}
</style>
