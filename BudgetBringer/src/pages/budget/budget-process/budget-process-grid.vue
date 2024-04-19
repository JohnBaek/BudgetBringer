<script setup lang="ts">
import {onMounted, computed, ref} from "vue";
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
 * 마운트 핸들링
 */
onMounted(() => {
  loadData();
});
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

      // Compute Total
      const computedTotals = computed(() => (props.gridModel as CommonGridModel).calculateTotals({
        items: items.value,
        calculateFields: (props.gridModel as CommonGridModel).calculateFields
      }));

      // Set total Columns
      for (let i=0; i<items.value.length; i++) {
        items.value[i].total = [
          computedTotals.value[i]
        ]
      }

      console.log(items.value);
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
