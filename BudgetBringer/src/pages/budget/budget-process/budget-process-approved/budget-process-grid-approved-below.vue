<script setup lang="ts">
import {RequestQuery} from "../../../../models/requests/query/request-query";
import {BudgetProcessGridProcessApproved} from "./budget-process-grid-approved-data";
import BudgetProcessApprovedGrid from "../budget-process-approved-grid.vue";

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
const gridModel = new BudgetProcessGridProcessApproved(
  props.fullDate as string ,
  props.year as number ,

  new RequestQuery('/api/v1/BudgetProcess/Approved/Below',0, 100000)
);
</script>

<template>
  <budget-process-approved-grid excel-title="ApprovedBelow" :grid-model="gridModel" />
</template>

<style scoped lang="css">
</style>
