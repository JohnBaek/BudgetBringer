<script setup lang="ts">
import {BudgetProcessGridPLOwner} from "./budget-process-grid-pl-owner-data";
import {RequestQuery} from "../../../../models/requests/query/request-query";
import BudgetProcessGrid from "../budget-process-grid.vue";

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
  yearList : {Type: []}
});
/**
 * Defines GridModel .
 */
const gridModel = new BudgetProcessGridPLOwner(
  props.fullDate as string ,
  props.year as number ,
  new RequestQuery('/api/v1/BudgetProcess/ProcessOwner',0, 100000)
);
</script>

<template>
  <budget-process-grid excel-title="PL_Owner" :grid-model="gridModel" :year-list="yearList" :year="year" />
</template>

<style scoped lang="css">
</style>
