<script setup lang="ts">
import {onMounted, ref} from "vue";
import {BudgetProcessGridPLOwner} from "./budget-process-grid-pl-owner-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {messageService} from "../../../services/message-service";
import {AgGridVue} from "ag-grid-vue3";
import {ResponseData} from "../../../models/responses/response-data";
import {
  ResponseProcessOwnerSummary
} from "../../../models/responses/process/process-owner/response-process-owner-summary";
import {communicationService} from "../../../services/communication-service";

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
 * 쿼리 정보
 */
const requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetProcess' ,
  pageCount: 10000 ,
  skip: 0 ,
  searchFields: [] ,
  searchKeywords: [],
  sortFields: [],
  sortOrders: [],
}


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

  // 서버에서 대상하는 데이터를 조회한다.
  HttpService.requestGet<ResponseData<ResponseProcessOwnerSummary>>(`${requestQuery.apiUri}/ProcessOwner`).subscribe({
    async next(response) {
      // 요청에 실패한경우
      if (response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      items.value = response.data.items;

      // 모든 아이템에 대해처리
      for(const data of items.value) {
        data.total = [
          {
            countryBusinessManagerName: "합계",
            budgetYear: data.items.reduce((sum, item) => sum + item.budgetYear, 0),
            budgetApprovedYearSum: data.items.reduce((sum, item) => sum + item.budgetApprovedYearSum, 0),
            budgetRemainingYear: data.items.reduce((sum, item) => sum + item.budgetRemainingYear, 0),
          },
        ];
      }
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
    } ,
    complete() {
      communicationService.offCommunication();
    },
  });
}


/**
 * 그리드 데이터
 */
const items = ref([]);
</script>

<template>
  <div v-for="item in items" :key="item.sequence" class="mb-5">
    <h3>{{item.title}}</h3>
    <v-spacer></v-spacer>
      <ag-grid-vue
        style="width: 100%; height: 600px;"
        :columnDefs="gridModel.columDefined"
        :rowData="item.items"
        :pinnedBottomRowData="item.total"
        class="ag-theme-alpine"
      >
      </ag-grid-vue>
  </div>
</template>

<style scoped lang="css">
</style>
