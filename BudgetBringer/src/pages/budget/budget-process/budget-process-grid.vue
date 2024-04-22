<script setup lang="ts">
import {onMounted, ref} from "vue";
import {ResponseProcessSummaryDetail} from "../../../models/responses/process/response-process-summary-detail";
import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {ResponseData} from "../../../models/responses/response-data";
import {messageService} from "../../../services/message-service";
import {AgGridVue} from "ag-grid-vue3";

/**
 * From the parent.
 */
const props = defineProps({
  // Options
  gridOptions: { Type: {} ,required: false, default : null } ,
  // Grid Model
  gridModel: { Type: CommonGridModel , required: true } ,
});
/**
 * Grid items ( Response items from server )
 */
const items  = ref([]);
/**
 * Grid API
 */
const gridApi = ref();
/**
 * Grid Column API
 */
const gridColumnApi = ref(null);
/**
 * 그리드 파라미터
 */
const gridParams = ref(null);
/**
 * gridReady 이벤트 핸들러
 * @param params 파라미터
 */
const onGridReady = (params) => {
  gridApi.value = params.api;
  gridColumnApi.value = params.columnApi;
  gridParams.value = params;
  calculateSums();
};
/**
 * 마운트 핸들링
 */
onMounted(() => {
  loadData();
});
/**
 * Set Sum Columns to Grid
 */
const calculateSums = () => {
  // Does not have gridAPI
  if (!gridApi.value)
    return;

  // Get list from grid
  const rowData = [];
  gridApi.value.forEachNode(node => rowData.push(node.data));

  // Get column states
  const columnStates = gridApi.value.getColumnState();
  const sums = [];

  // Process all columns
  for (let i=0; i<columnStates.length; i++) {
    // Get Column
    const colState = columnStates[i];

    // First column
    if(i == 0) {
      sums[colState.colId] = '합계';
      continue;
    }

    // Sum to all values
    sums[colState.colId] = rowData.reduce((acc, row) => {
      const value = row[colState.colId];
      return acc + (typeof value === 'number' ? value : 0);
    }, 0);
  }

  gridApi.value.setPinnedBottomRowData([sums]);
}
/**
 * 데이터를 로드한다.
 */
const loadData = () => {
  communicationService.inCommunication();
  // Request to Server
  HttpService.requestGet<ResponseData<ResponseProcessSummaryDetail<any>>>(
                (props.gridModel as CommonGridModel).requestQuery.apiUri)
                      .subscribe({
    async next(response) {
      // Failed Request
      if (response.error) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // Update list
      items.value = response.data.items;

      // Compute sums
      calculateSums();
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
    } ,
    complete() {
      communicationService.offCommunication();
    },
  });
}
</script>

<template>
  <div v-for="item in items" :key="item.sequence" class="mb-5">
    <h3>{{item.title}}</h3>
    <v-spacer></v-spacer>
    <ag-grid-vue
      style="width: 100%; height: 600px;"
      @grid-ready="onGridReady"
      :grid-options="props.gridOptions"
      :columnDefs="(props.gridModel as CommonGridModel).columDefined"
      :rowData="item.items"
      :pinnedBottomRowData="item.total"
      class="ag-theme-alpine"
    >
    </ag-grid-vue>
  </div>
</template>

<style scoped lang="css">
</style>
