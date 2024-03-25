<script setup lang="ts" generic="T">
import {AgGridVue} from "ag-grid-vue3";
import {ref} from "vue";


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
    default: []
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
    default: '900px'
  }
});


/**
 * 그리드의 rowData
 */
const items = ref(props.inputRowData);

/**
 * 입력 데이터
 */
let inputRow = {};

/**
 * Top 인서트 Pine
 */
const pinnedTopRowData = [inputRow];

/**
 * 그리드의 column 데이터
 */
const columDefined = ref([...props.inputColumDefined]);

/**
 * 인서트 Grid 사용여부
 */
const isUseInsert = props.isUseInsert;

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
  // console.log( 'createPinnedCellPlaceholder',colDefined.colDef.headerName,colDefined.colDef)

  console.log(colDefined.colDef);

  return colDefined.colDef.headerName + ' 입력..';
}

/**
 * 기본 그리드 컬럼 설정
 * * Insert 그리드 사용시만 초기화 *
 */
const defaultColDefined = isUseInsert ? {
  flex: 1,
  editable: true,
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

  if (isUseInsert && isPinnedRowDataCompleted(params)) {
    console.log(isPinnedRowDataCompleted);

    // 최상단에 추가된 데이터를 추가한다.
    items.value = [inputRow,...items.value];


    console.log(inputRow);

    // 최상단 Row 를 초기화한다.
    params.api.setPinnedTopRowData([{}]);
  }
}

</script>

<template>
  <!--공통 그리드-->
  <ag-grid-vue
    :rowData="items"
    :columnDefs="columDefined"
    :pinnedTopRowData="pinnedTopRowData"
    :defaultColDef="defaultColDefined"
    :getRowStyle="getRowStyle"
    :style="{ width, height }"
    @cell-editing-stopped="onCellEditingStopped"
    class="ag-theme-quartz"
  >
  </ag-grid-vue>
</template>

<style scoped lang="css">
</style>
