<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue'
import { BudgetPlanApiService } from '@/services/apis/BudgetPlanApiService'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import type { ResponseBudgetPlan } from '@/models/responses/budgets/response-budget-plan'
import { firstValueFrom } from 'rxjs'
import { toClone } from '@/services/utils/ObjectUtils'
import { FilterMatchMode } from 'primevue/api'
import { useConfigStore } from '@/services/stores/ConfigStore'
import type { ColumnFilterModelType } from 'primevue/column'
import { RequestQuery } from '@/models/requests/query/request-query'
import { CommonGridColumn } from '@/components/common/grid/CommonGridColumn'
import CommonGridCore from '@/components/common/grid/CommonGridCore.vue'

// Stores
const communicationStore = useCommunicationStore();

// Request Resources
const query: RequestQuery = new RequestQuery('/api/v1/BudgetPlan');
const columns: Array<CommonGridColumn> = [
  new CommonGridColumn({ field: 'baseYearForStatistics', header: '통계 기준년도', filterComponentType: 'Text', isUseFilter: true }) ,
];

const filters = ref({
  baseYearForStatistics: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  businessUnitName: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
});
</script>

<template>
  <Panel>
    <CommonGridCore
      :query="query"
      :columns="columns"
    ></CommonGridCore>

<!--    <DataTable-->
<!--      v-model:filters="filters"-->
<!--      filterDisplay="row"-->
<!--      :value="items"-->
<!--      :lazy="true"-->
<!--      dataKey="id"-->
<!--      scrollable-->
<!--      scrollHeight="600px"-->
<!--      tableStyle="min-width: 50rem"-->
<!--      ref="dataTable"-->
<!--    >-->
<!--      <Column field="baseYearForStatistics"  :filterMatchModeOptions="customFilterOptions" header="통계기준일" style="width: 20%"  >-->
<!--        <template #body="{ data }">-->
<!--          {{ data.baseYearForStatistics }}-->
<!--        </template>-->
<!--        <template #filter="{ filterModel }">-->
<!--          <InputText v-model="filterModel.value" type="text" @keyup.enter="requestFilter(filterModel)" class="p-column-filter" placeholder="통계기준일 검색" />-->
<!--        </template>-->
<!--      </Column>-->

<!--      <Column field="businessUnitName" header="비지니스 유닛" style="width: 20%" :showFilterMenu="false">-->
<!--        <template #body="{ data }">-->
<!--          {{ data.businessUnitName }}-->
<!--        </template>-->
<!--        <template #filter="{ filterModel }">-->
<!--          <MultiSelect v-model="filterModel.value"-->
<!--                       @change="changeFilter(filterModel)"-->
<!--                       :options="configStore.businessUnits"-->
<!--                       optionLabel="name"-->
<!--                       placeholder="비지니스 유닛 선택"-->
<!--                       class="p-column-filter"-->
<!--                       style="min-width: 14rem"-->
<!--                       :maxSelectedLabels="1">-->
<!--            <template #option="slotProps">-->
<!--              <div class="flex align-items-center gap-2">-->
<!--                <span>{{ slotProps.option.name }}</span>-->
<!--              </div>-->
<!--            </template>-->
<!--          </MultiSelect>-->
<!--        </template>-->
<!--      </Column>-->

<!--    </DataTable>-->
  </Panel>
</template>

<style scoped>
.loading-bar {
  text-align: center;
  padding: 10px;
}
</style>