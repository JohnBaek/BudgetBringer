<script setup lang="ts">
import {ref} from "vue";
import { BudgetPlanGridData} from "./budget-plan-grid-data";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {RequestQuery} from "../../../models/requests/query/request-query";


/**
 * 그리드 모델
 */
const gridModel = new BudgetPlanGridData();

/**
 * 신규 행이 추가되었을때
 * @param params 파라미터
 */
const onNewRowAdded = (params) => {

}

/**
 * 그리드 데이터
 */
const items = ref(gridModel.items);

/**
 * 쿼리 정보
 */
const requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetPlan' ,
  pageCount: 100 ,
  skip: 0 ,
  searchFields: ['isAbove500K'] ,
  searchKeywords: [ 'false' ],
  sortFields: [ 'approvalDate' ],
  sortOrders: [ 'desc' ],
}
</script>

<template>
  <common-grid
    :is-use-insert="gridModel.isUseInsert"
    :input-colum-defined="gridModel.columDefined"
    :is-use-buttons="true"
    :input-row-data="items" @onNewRowAdded="onNewRowAdded"
    :query-request="requestQuery"
  />
</template>

<style scoped lang="css">
</style>
