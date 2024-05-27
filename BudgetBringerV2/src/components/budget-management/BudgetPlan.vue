<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue'
import { BudgetPlanApiService } from '@/services/apis/BudgetPlanApiService'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import type { ResponseBudgetPlan } from '@/models/responses/budgets/response-budget-plan'
import { firstValueFrom } from 'rxjs'
import { toClone } from '@/services/utils/ObjectUtils'
import { FilterMatchMode } from 'primevue/api'

// Communication Store
const communicationStore = useCommunicationStore();

// Data Table Ref
const dataTable = ref();

// Count of Total Records
const totalRecords = ref(200);

// Data Items
const items = ref(Array<ResponseBudgetPlan>());

// Weather or not is Bottom
const isBottom = ref(false);

// Request Container
let requestContainer =  {
  skip:0, pageCount:100,
  searchKeywords:[], searchFields:[],
  sortFields:[], sortOrders:[]
};

/**
 * Request To Server
 */
const loadData = async () => {
  // In Transmissions
  if(communicationStore.communication || communicationStore.transmission)
    return;

  const client = new BudgetPlanApiService();
  const response = await firstValueFrom(client.getListAsync(requestContainer));

  // Errors
  if(response.error)
    return;

  // Item Loaded done
  if(items.value.length >= response.totalCount)
    return;

  requestContainer.skip += response.items.length;
  totalRecords.value = response.totalCount;

  // Add Items
  items.value = items.value.concat(toClone(response.items));
}

/**
 * Scroll event callback
 * @param event
 */
const onScroll = async (event: Event) => {
  const { scrollTop, clientHeight, scrollHeight } = event.target as HTMLElement;

  // Scroll Bottom
  if (scrollTop + clientHeight >= scrollHeight - 10) {
    isBottom.value = true;
    if (requestContainer.skip < totalRecords.value) {
      await loadData();
    }
  }
  // Not Scroll Bottom
  else {
    isBottom.value = false;
  }
}

onMounted(async () => {
  // Add Scroll Event
  dataTable.value.$el.children[0].addEventListener("scroll", onScroll);
  await loadData();
})

onUnmounted(() => {
  if (dataTable.value) {
    // Remove Scroll Event
    dataTable.value.removeEventListener('scroll', onScroll);
  }
});

const filters = ref({
  baseYearForStatistics: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
  businessUnitName: { value: null, matchMode: FilterMatchMode.STARTS_WITH },
});

const requestFilter = (data:any) => {
  console.log('data',data)
}

</script>

<template>
  <Panel>
    <DataTable
      v-model:filters="filters"
      filterDisplay="row"
      :value="items"
      dataKey="id"
      scrollable
      scrollHeight="600px"
      tableStyle="min-width: 50rem"
      ref="dataTable"
    >
      <Column field="baseYearForStatistics" header="통계기준일" style="width: 20%">
        <template #body="{ data }">
          {{ data.baseYearForStatistics }}
        </template>
        <template #filter="{ filterModel }">
          <InputText v-model="filterModel.value" type="text" @keyup.enter="requestFilter(filterModel)" class="p-column-filter" placeholder="통계기준일 검색" />
        </template>
      </Column>

      <Column field="businessUnitName" header="비지니스 유닛" style="width: 20%">
        <template #body="{ data }">
          {{ data.businessUnitName }}
        </template>
        <template #filter="{ filterModel, filterCallback }">
          <MultiSelect v-model="filterModel.value" @change="filterCallback()" :options="representatives" optionLabel="name" placeholder="Any" class="p-column-filter" style="min-width: 14rem" :maxSelectedLabels="1">
            <template #option="slotProps">
              <div class="flex align-items-center gap-2">
                <img :alt="slotProps.option.name" :src="`https://primefaces.org/cdn/primevue/images/avatar/${slotProps.option.image}`" style="width: 32px" />
                <span>{{ slotProps.option.name }}</span>
              </div>
            </template>
          </MultiSelect>
        </template>
      </Column>

    </DataTable>
    <div class="loading-bar" v-if="communicationStore.transmission && totalRecords > items.length">
      <div><i class="pi pi-spin pi-spinner" style="font-size: 2rem"></i></div>
    </div>
  </Panel>
</template>

<style scoped>
.loading-bar {
  text-align: center;
  padding: 10px;
}
</style>