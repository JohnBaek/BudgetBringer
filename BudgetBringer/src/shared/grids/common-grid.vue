<script setup lang="ts" generic="T">
import {AgGridVue} from "ag-grid-vue3";
import {ref} from "vue";
import {communicationService, CommunicationService} from "../../services/communication-service";

/**
 * Prop 정의
 */
const props = defineProps({
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  inputRowData : {
    Type: Array<T> ,
    required: true ,
    default: []
  },
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
  }
});

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


/**
 * 그리드의 rowData
 */
const items = ref(null);

/**
 *  스켈레톤 로더
 */
// TODO 테스트
setTimeout(() =>{items.value = props.inputRowData; },1000);

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
 * gridReady 이벤트 핸들러
 * @param params 파라미터
 */
const onGridReady = (params) => {
  gridApi.value = params.api;
  gridColumnApi.value = params.columnApi;
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

/**
 * Cell 이 열렸다 닫혔을때
 * @param params Cell 파라미터
 */
const onCellEditingStopped = (params) => {
  // 인서트 그리드를 사용하지 않는경우 제외한다.
  if(!isUseInsert)
    return;

  // 데이터 입력이 완료된경우
  if (isUseInsert && isPinnedRowDataCompleted(params)) {

    // 최상단에 추가된 데이터를 추가한다.
    items.value = [inputRow,...items.value];

    // 부모 컴포넌트에게 전달
    emits('onNewRowAdded' , inputRow);
    inputRow = {};
    // 최상단 Row 를 초기화한다.
    params.api.setPinnedTopRowData([inputRow]);
  }
}

/**
 * 그리드의 셀렉트가 변경되었을때
 */
const onSelectionChanged = () => {
  // 선택된 Row 를 업데이트한다.
  selectedRows.value = gridApi.value.getSelectedRows();
  console.log('onSelectionChanged' , selectedRows);
};

/**
 * 데이터 삭제
 */
const removeItems = () => {
  const selectedRows = gridApi.value.getSelectedRows();
  console.log('selectedRows',selectedRows);
}

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
}


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

  <!--공통 그리드-->
  <ag-grid-vue
    :rowData="items"
    :columnDefs="columDefined"
    :pinnedTopRowData="pinnedTopRowData"
    :defaultColDef="defaultColDefined"
    :getRowStyle="getRowStyle"
    :style="{ width, height }"
    :localeText="{ noRowsToShow: '데이터가 없습니다.' }"
    @selection-changed="onSelectionChanged"
    @grid-ready="onGridReady"
    @cell-editing-stopped="onCellEditingStopped"
    @keyup.esc="removeItems"
    rowSelection='multiple'
    class="ag-theme-alpine"
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
