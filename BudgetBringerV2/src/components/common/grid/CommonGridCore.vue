<script setup lang="ts">
import { RequestQuery } from '@/models/requests/query/request-query'
import { CommonGridColumn, CommonGridTextFilters, ParseFilters } from '@/components/common/grid/CommonGridColumn'
import { onMounted, onUnmounted, ref } from 'vue'
import { firstValueFrom } from 'rxjs'
import { toClone } from '@/services/utils/ObjectUtils'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import { ApiClient } from '@/services/apis/ApiClient'
import type { ResponseList } from '@/models/responses/response-list'
import Column, { type ColumnFilterModelType } from 'primevue/column'

// Props
const props = defineProps({
  query: { type: RequestQuery, required: true } ,
  columns: { type: Array<CommonGridColumn>, required: true }
});

// Stores
const communicationStore = useCommunicationStore();

// Client
let client: ApiClient = new ApiClient('/api/v1/BudgetPlan');

// Query
let query: RequestQuery = props.query;

// grid items
const items = ref([]);

// grid item counts
const itemCounts = ref(200);

// grid ref
const grid = ref();

// grid filters
const gridFilters = ref(ParseFilters(props.columns));

// grid Weather or not is Bottom
const isBottom = ref(false);

/**
 * Scroll event callback
 * @param event
 */
const onScroll = async (event: Event) => {
  const { scrollTop, clientHeight, scrollHeight } = event.target as HTMLElement;

  // Scroll Bottom
  if (scrollTop + clientHeight >= scrollHeight - 10) {
    isBottom.value = true;
    if (query.skip < itemCounts.value) {
      await loadData();
    }
  }
  // Not Scroll Bottom
  else {
    isBottom.value = false;
  }
}

// onMounted
onMounted(async () => {
  // Add Scroll Event
  let scrollableElement = grid.value.$el.querySelector('.p-datatable-wrapper');

  console.log(scrollableElement)
  if (scrollableElement) {
    scrollableElement.addEventListener('scroll', onScroll);
  }
  await loadData();
})

// onUnmounted
onUnmounted(() => {
  if (grid.value) {
    let scrollableElement = grid.value.$el.querySelector('.p-datatable-wrapper');
    if (scrollableElement) {
      scrollableElement.removeEventListener('scroll', onScroll);
    }
  }
});

/**
 * Request To Server
 */
const loadData = async () => {
  // Stop when In Communication with server
  if(communicationStore.communication || communicationStore.transmission)
    return;

  // Request List
  communicationStore.inTransmission();
  const response = await firstValueFrom(client.requestGetAsync<ResponseList<any>>(query));

  // Errors
  if(response.error)
    return;

  // Item Loaded done
  if(items.value.length >= response.totalCount)
    return;

  query.skip += response.items.length;
  itemCounts.value = response.totalCount;

  // Add Items
  items.value = items.value.concat(toClone(response.items));
  communicationStore.offTransmission();
}

const changeFilter = (model: ColumnFilterModelType) => {
  console.log('changeFilter', model);
}
</script>

<template>
  <DataTable
    data-key="id"
    scrollable
    scrollHeight="600px"
    tableStyle="min-width: 50rem"
    filter-display="row"
    ref="grid"
    :value="items"
    v-model:filters="gridFilters"
  >
    <div v-for="column of columns" :key="column.field">
      <!-- Text -->
      <div v-if="column.filterComponentType === 'Text'">
        <Column :field="column.field"  :header="column.header" :filterMatchModeOptions="CommonGridTextFilters">
          <template #body="{ data }">
            {{ data[column.field] }}
          </template>
          <template #filter="{ filterModel }">
            <InputText  v-model="filterModel.value"
                        type="text"
                        class="p-column-filter"
                        @keyup.enter="changeFilter(filterModel)"
                        :placeholder="`${column.header} 검색`"
            />
          </template>
        </Column>
      </div>
      <div v-if="column.filterComponentType === 'TagSelect'">
        <Column :field="column.field"  :header="column.header" :showFilterMenu="false" >
          <template #body="{ data }">
            <div class="flex align-items-center gap-2">
              <Tag :value="column.filterItems.filter(i => i.value == data[column.field])[0].name" :severity="column.filterItems.filter(i => i.value == data[column.field])[0].tag" />
            </div>
          </template>
          <template #filter="{ filterModel }">
            <MultiSelect v-model="filterModel.value" @change="changeFilter(filterModel)" :options="column.filterItems" :optionLabel="'name'" :placeholder="column.header" class="p-column-filter" style="min-width: 14rem" :maxSelectedLabels="1">
              <template #option="slotProps">
                <div class="flex align-items-center gap-2">
                  <Tag :value="slotProps.option.name" :severity="slotProps.option.tag" />
                </div>
              </template>
            </MultiSelect>
          </template>
        </Column>
      </div>
    </div>
  </DataTable>

  <div class="loading-bar" v-if="communicationStore.transmission && itemCounts > items.length">
    <div><i class="pi pi-spin pi-spinner" style="font-size: 2rem"></i></div>
  </div>
</template>

<style scoped>
.loading-bar {
  text-align: center;
  padding: 10px;
}
</style>