<script setup lang="ts">
import {ref} from "vue";
import {BudgetGridRowDataModel, BudgetPlanGridData} from "./budget-plan-grid-data";
import CommonGrid from "../../../shared/grid/common-grid.vue";

// TODO 테스트 데이터
let testData = new BudgetGridRowDataModel();
testData.ApprovalDate = '2024 품의예정';
testData.Sector = '10';
testData.Bu = 'H&N';
testData.Cc = '1100';
testData.Cbm = 'Yuri Hong';
testData.Description = '수은분석기';
testData.FvBudget = 60000;

/**
 * 그리드 모델
 */
const gridModel = new BudgetPlanGridData();

// TODO 로우데이터를 임의로 추가한다.
for (let i=0; i<1; i++)
  gridModel.items.push(testData);


/**
 * 신규 행이 추가되었을때
 * @param params 파라미터
 */
const onNewRowAdded = (params) => {
  console.log('onNewRowAdded',params);
}

/**
 * 그리드 데이터
 */
const items = ref(gridModel.items);
</script>

<template>
  <common-grid
    :is-use-insert="gridModel.isUseInsert"
    :input-colum-defined="gridModel.columDefined"
    :is-use-buttons="true"
    :input-row-data="items" @onNewRowAdded="onNewRowAdded"
  />
</template>

<style scoped lang="css">
</style>
