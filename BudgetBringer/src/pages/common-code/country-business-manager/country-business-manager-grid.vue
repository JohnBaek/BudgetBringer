<script setup lang="ts">
import {ref} from "vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {CountryBusinessManagerGridData} from "./country-business-manager-grid-data";
import {RequestCountryBusinessManager} from "../../../models/requests/budgets/request-country-business-manager";
import CommonGridDialog from "../../../shared/grids/common-grid-dialog.vue";
/**
 * Grid Model
 */
const gridModel = new CountryBusinessManagerGridData();
/**
 * props 정의
 */
const title : string = '컨트리 비지니스 매니저';
/**
 * 그리드 래퍼런스
 */
const gridReference = ref(null);
/**
 * 데이터 추가 원본 요청 데이터
 */
const dataModel = ref<RequestCountryBusinessManager>(new RequestCountryBusinessManager());
const dialog = ref();
/**
 * outgoing
 */
const emits = defineEmits<{
  (e: 'click', any) : void,
}>();
/**
 * Click cell
 * @param businessUnits
 */
const click = (businessUnits: []) => {
  emits('click', businessUnits);
}
</script>

<template>
  <common-grid
    :input-colum-defined="gridModel.columDefined"
    :query-request="gridModel.requestQuery"
    @onAdd="dialog.showAddDialog()"
    @onUpdate="dialog.showUpdateDialog($event.id)"
    @onRemove="dialog.showRemoveDialog($event)"
    @on-double-clicked="dialog.showUpdateDialog($event.id)"
    @on-cell-clicked="click($event)"
    ref="gridReference"
  />
  <common-grid-dialog
    :input-colum-defined="gridModel.columDefined"
    :request-query="gridModel.requestQuery"
    v-model="dataModel"
    :title="title"
    :model-empty-value="new RequestCountryBusinessManager()"
    @submit="gridReference.doRefresh()"
    ref="dialog"
  />
</template>

<style scoped lang="css">
</style>
