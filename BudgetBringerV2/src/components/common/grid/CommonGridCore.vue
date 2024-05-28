<script setup lang="ts">
import { RequestQuery } from '@/models/requests/query/request-query'
import { CommonGridColumn, CommonGridTextFilters } from '@/components/common/grid/CommonGridColumn'
import { onMounted, onUnmounted, ref } from 'vue'
import CommonGridColumnText from '@/components/common/grid/CommonGridColumnText.vue'
import { firstValueFrom } from 'rxjs'
import { toClone } from '@/services/utils/ObjectUtils'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import { ApiClient } from '@/services/apis/ApiClient'
import type { ResponseList } from '@/models/responses/response-list'
import column from 'primevue/column/Column.vue'

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
  grid.value.$el.children[0].addEventListener("scroll", onScroll);
  await loadData();
})

// onUnmounted
onUnmounted(() => {
  if (grid.value) {
    // Remove Scroll Event
    grid.value.removeEventListener('scroll', onScroll);
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
    :lazy="true"
  >
    <CommonGridColumnText :column="column" ></CommonGridColumnText>
    <Column :field="'baseYearForStatistics'"  :filterMatchModeOptions="CommonGridTextFilters" :header="'통계'" >
      <template #body="{ data }">
        {{ data['baseYearForStatistics'] }}
      </template>
    </Column>


<!--    <CommonGridColumnText v-if="column.filterComponentType === 'Text'" :column="column" ></CommonGridColumnText>-->
<!--    &lt;!&ndash; Iterates All columns &ndash;&gt;-->
<!--    <div v-for="column in columns" :key="column.field">-->
<!--      &lt;!&ndash;Filter and Text&ndash;&gt;-->
<!--      <CommonGridColumnText v-if="column.filterComponentType === 'Text'" :column="column" ></CommonGridColumnText>-->
<!--    </div>-->
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