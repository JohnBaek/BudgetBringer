<script setup lang="ts">
import {ref, onMounted} from "vue";
import {BudgetPlanGridData} from "./budget-plan-grid-data";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {RequestQuery} from "../../../models/requests/query/request-query";
/**
 * 그리드 모델
 */
const gridModel = new BudgetPlanGridData();

/**
 * 그리드 데이터
 */
const items = ref(gridModel.items);


/**
 * 마운트 핸들링
 */
onMounted(() => {

});

/**
 * 신규 행이 추가되었을때
 * @param params 파라미터
 */
const onNewRowAdded = (params) => {
  console.log('onNewRowAdded',params);
}

/**
 * 쿼리 정보
 */
const requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetPlan' ,
  pageCount: 40 ,
  skip: 0 ,
  searchFields: ['isAbove500K'] ,
  searchKeywords: [ 'true' ],
  sortFields: [ 'approvalDate' ],
  sortOrders: [ 'desc' ],
}
</script>

<template>
  <common-grid :is-use-insert="gridModel.isUseInsert"
               :input-colum-defined="gridModel.columDefined"
               @onNewRowAdded="onNewRowAdded"
               :is-use-buttons="true"
               :query-request="requestQuery"
  />
</template>

<style scoped lang="css">
</style>
