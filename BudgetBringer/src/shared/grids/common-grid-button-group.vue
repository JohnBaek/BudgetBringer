<script setup lang="ts">
import {CommonGridButtonGroupDefinesButtonEmits} from "./common-grid-button-group-defines";
import {ref} from "vue";
import {communicationService} from "../../services/communication-service";
import {CommonButtonDefinitions, CommonGridButton} from "./common-grid-button";

/**
 * From the parent.
 */
const props = defineProps({
  // Only listed in this array will be shown.
  showButtons: { Type: Array<CommonGridButton> ,
    default: [
      CommonButtonDefinitions.add ,
      CommonButtonDefinitions.remove ,
      CommonButtonDefinitions.update ,
      CommonButtonDefinitions.refresh ,
    ],
  required: false},
  // Items in grid
  selectedRows: { Type: Array<any> , default: [] , required: false},
});

/**
 * Defines dispatches to parent.
 */
const emits = defineEmits<CommonGridButtonGroupDefinesButtonEmits>();
/**
 * dispatch add
 */
const add = () => {
  emits('onAdd');
}
/**
 * dispatch remove
 */
const remove = () => {
  emits('onRemove',null);
}
/**
 * dispatch update
 */
const update = () => {
  emits('onUpdate',null);
}
/**
 * dispatch exportExcel
 */
const exportExcel = () => {
  emits('onExportExcel');
}
/**
 * dispatch refresh
 */
const refresh = () => {
  emits('onRefresh');
}
/**
 * dispatch exportPdf
 */
const exportPdf = () => {
  emits('exportPdf');
}

let onChart = ref(true);
const toChart = () => {
  emits('chart');
  onChart.value = false;
}
const toGrid = () => {
  emits('grid');
  onChart.value = true;
}
const inCommunication = ref(false);
communicationService.subscribeCommunication().subscribe((communication) =>{
  inCommunication.value = communication;
});
</script>

<template>
  <v-btn v-if="props.showButtons.includes(CommonButtonDefinitions.add)" class="mr-2"  :disabled="inCommunication" width="100" elevation="1" color="info" @click="add()">
    <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
    <template v-if="!inCommunication">
      <v-icon>mdi-checkbox-marked-circle</v-icon>
      <pre><b> 추가 </b></pre>
    </template>
  </v-btn>

  <v-btn v-if="props.showButtons.includes(CommonButtonDefinitions.remove)" class="mr-2"  :disabled="inCommunication || (selectedRows.length === 0)" width="100" elevation="1" color="error" @click="remove()">
    <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
    <template v-if="!inCommunication">
      <v-icon>mdi-delete-circle</v-icon>
      <pre><b> 삭제 </b></pre>
    </template>
  </v-btn>

  <v-btn v-if="props.showButtons.includes(CommonButtonDefinitions.update)" class="mr-2"  :disabled="inCommunication || (selectedRows.length === 0 || selectedRows.length > 1)" width="100" elevation="1" color="warning" @click="update()">
    <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
    <template v-if="!inCommunication">
      <v-icon>mdi-checkbox-multiple-marked-circle</v-icon>
      <pre><b> 수정 </b></pre>
    </template>
  </v-btn>
  <v-icon v-if="props.showButtons.includes(CommonButtonDefinitions.refresh)" @click="refresh()" class="ml-3" size="x-large" color="blue" style="cursor: pointer;">mdi-refresh-circle</v-icon>
  <v-icon v-if="props.showButtons.includes(CommonButtonDefinitions.exportExcel)" @click="exportExcel()" class="ml-3" size="x-large" color="green" style="cursor: pointer;">mdi-file-excel-outline</v-icon>
  <v-icon v-if="props.showButtons.includes(CommonButtonDefinitions.exportPDF)" @click="exportPdf()" class="ml-3" size="x-large" color="red" style="cursor: pointer;">mdi-file-pdf-box</v-icon>
  <v-icon v-if="props.showButtons.includes(CommonButtonDefinitions.toChart) && onChart" @click="toChart()" class="ml-3" size="x-large" color="blue" style="cursor: pointer;">mdi-chart-bar</v-icon>
  <v-icon v-if="props.showButtons.includes(CommonButtonDefinitions.toChart) && !onChart" @click="toGrid()" class="ml-3" size="x-large" color="grey" style="cursor: pointer;">mdi-view-list</v-icon>
  <v-spacer v-if="props.showButtons.length > 0" class="mt-1"></v-spacer>
  <span class="text-grey" v-if="props.showButtons.includes(CommonButtonDefinitions.remove)">shift 버튼을 누른채로 클릭하면 여러 행을 선택할수 있습니다.</span>
</template>
<style scoped lang="css">
</style>
