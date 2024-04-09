<script setup lang="ts">
import {onMounted, ref} from "vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {BudgetProcessGridPLOwner} from "./budget-process-grid-pl-owner-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {HttpService} from "../../../services/api-services/http-service";
import {ResponseList} from "../../../models/responses/response-list";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {messageService} from "../../../services/message-service";
import router from "../../../router";

/**
 * Prop 정의
 */
const props = defineProps({
  /**
   * 전체 날짜 정보를 받는다.
   */
  fullDate : {
    Type: String ,
    required: true
  } ,

  /**
   * 년도 정보
   */
  year : {
    Type: Number ,
    required: true
  } ,

  /**
   * 타이틀 정보
   */
  title : {
    Type: String ,
    required: false
  } ,


  /**
   * 서브타이틀 정보
   */
  subTitle : {
    Type: String ,
    required: true
  } ,
});


/**
 * 그리드 모델
 */
const gridModel = new BudgetProcessGridPLOwner(props.fullDate as string , props.year as number);

/**
 * 신규 행이 추가되었을때
 * @param params 파라미터
 */
const onNewRowAdded = (params) => {
  console.log('onNewRowAdded',params);
}

/**
 * 쿼리 정보
 */
const requestQuery :RequestQuery = {
  apiUri : 'BudgetPlan' ,
  pageCount: 40 ,
  skip: 0 ,
  searchFields: ['IsAbove500K'] ,
  searchKeywords: [ 'true' ],
  sortFields: [ 'ApprovalDate' ],
  sortOrders: [ 'desc' ],
}

/**
 * 그리드 데이터
 */
const items = ref(gridModel.items);

</script>

<template>
  <span v-if="props.title !== ''"><h3>{{props.title}}</h3></span>
  <v-spacer></v-spacer>
  <span class="text-grey"  v-if="props.subTitle !== ''">{{props.subTitle}}</span>
  <common-grid :is-use-insert="gridModel.isUseInsert"
               :is-use-buttons="false"
               :input-colum-defined="gridModel.columDefined"
               :input-row-data="items"
               height="600px"
               @onNewRowAdded="onNewRowAdded"
               :query-request="requestQuery"
  />
</template>

<style scoped lang="css">
</style>
