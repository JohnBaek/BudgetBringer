<script setup lang="ts">
import {ref} from "vue";
import {BudgetProcessGridPLOwner} from "./budget-process-grid-pl-owner-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import BudgetProcessGrid from "./budget-process-grid.vue";

/**
 * From the parent.
 */
const props = defineProps({
  // Date String 'yyyy-MM-dd'
  fullDate : { Type: String ,  required: true } ,
  // Year String 'yyyy'
  year : {Type: Number , required: true} ,
  // Title of Grid
  title : {Type: String , required: false} ,
  // SubTitle of Grid
  subTitle : {Type: String , required: true} ,
});
/**
 * Defines GridModel .
 */
const gridModel = new BudgetProcessGridPLOwner(
  props.fullDate as string ,
  props.year as number ,
  new RequestQuery('/api/v1/BudgetProcess/ProcessOwner',0, 100000)
);
/**
 * Grid items
 */
const items = ref([]);
</script>

<template>
  <budget-process-grid
    :items="items"
    :grid-model="gridModel"
  >
  </budget-process-grid>
</template>

<style scoped lang="css">
</style>
