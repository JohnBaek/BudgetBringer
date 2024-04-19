<script setup lang="ts">
import {CommonGridButtonGroupDefinesButtonEmits} from "./common-grid-button-group-defines";

/**
 * From the parent.
 */
const props = defineProps({
  // Only listed in this array will be shown.
  showButtons: { Type: Array<string> , default: ['add','update','delete','refresh', 'excel'] , required: false},
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
</script>

<template>
  <v-btn v-if="props.showButtons.includes('add')" variant="outlined" @click="add()" class="mr-2" color="info">추가</v-btn>
  <v-btn v-if="props.showButtons.includes('delete')" variant="outlined" @click="remove()" class="mr-2" color="error" :disabled="selectedRows.length == 0">삭제</v-btn>
  <v-btn v-if="props.showButtons.includes('update')" variant="outlined" @click="update()" :disabled="selectedRows.length != 1" color="warning">수정</v-btn>
  <v-icon v-if="props.showButtons.includes('refresh')" @click="refresh()" class="ml-3" size="x-large" color="blue" style="cursor: pointer;">mdi-refresh-circle</v-icon>
  <v-icon v-if="props.showButtons.includes('excel')" @click="exportExcel()" class="ml-3" size="x-large" color="green" style="cursor: pointer;">mdi-file-excel-outline</v-icon>
  <v-spacer v-if="props.showButtons.length > 0" class="mt-1"></v-spacer>
  <span class="text-grey" v-if="props.showButtons.includes('delete')">shift 버튼을 누른채로 클릭하면 여러 행을 선택할수 있습니다.</span>
</template>

<style scoped lang="css">
</style>
