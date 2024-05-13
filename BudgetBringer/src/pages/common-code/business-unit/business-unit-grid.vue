<script setup lang="ts">
import {ref} from "vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {BusinessUnitGridData} from "./business-unit-grid-data";
import {RequestBusinessUnit} from "../../../models/requests/budgets/request-business-unit";
import CommonGridDialog from "../../../shared/grids/common-grid-dialog.vue";
/**
 * Grid Model
 */
const gridModel = new BusinessUnitGridData();
/**
 * props 정의
 */
const title : string = '비지니스 유닛';
/**
 * 그리드 래퍼런스
 */
const gridReference = ref(null);
/**
 * 데이터 추가 원본 요청 데이터
 */
const dataModel = ref<RequestBusinessUnit>(new RequestBusinessUnit());
const dialog = ref();
</script>

<template>
  <common-grid
               :input-colum-defined="gridModel.columDefined"
               :query-request="gridModel.requestQuery"
               @onAdd="dialog.showAddDialog()"
               @onUpdate="dialog.showUpdateDialog($event.id)"
               @onRemove="dialog.showRemoveDialog($event)"
               ref="gridReference"
  />
  <common-grid-dialog
                      :input-colum-defined="gridModel.columDefined"
                      :request-query="gridModel.requestQuery"
                      v-model="dataModel"
                      :title="title"
                      :model-empty-value="new RequestBusinessUnit()"
                      @submit="gridReference.doRefresh()"
                      ref="dialog"
  />
</template>

<style scoped lang="css">
</style>
