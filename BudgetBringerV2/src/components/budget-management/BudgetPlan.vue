<script setup lang="ts">
import { RequestQuery } from '@/models/requests/query/request-query'
import { CommonGridColumn } from '@/components/common/grid/CommonGridColumn'
import CommonGridCore from '@/components/common/grid/CommonGridCore.vue'

// Request Resources
const query: RequestQuery = new RequestQuery('/api/v1/BudgetPlan');
const columns: Array<CommonGridColumn> = [
  new CommonGridColumn({ field: 'baseYearForStatistics', header: '통계 기준년도', filterComponentType: 'Text', isUseFilter: true }) ,
  new CommonGridColumn({ field: 'approvalDate', header: 'Approval Date', filterComponentType: 'Text', isUseFilter: true }) ,
  new CommonGridColumn({ field: 'isIncludeInStatistics', header: '통계포함여부', filterComponentType: 'TagSelect', isUseFilter: true , filterItems: [{name:"포함",value:true,tag:'success'}, {name:"미포함",value:false,tag:'danger'}, ]}) ,
];
</script>

<template>
  <Panel>
    <CommonGridCore
      :query="query"
      :columns="columns"
    >
    </CommonGridCore>
  </Panel>
</template>

<style scoped>
.loading-bar {
  text-align: center;
  padding: 10px;
}
</style>