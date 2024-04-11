<script setup lang="ts">
import {onMounted, ref} from "vue";
import {BudgetPlanGridData} from "./budget-plan-grid-data";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {RequestBudgetPlan} from "../../../models/requests/budgets/request-budget-plan";
import CommonSelect from "../../../shared/common-select.vue";
import {messageService} from "../../../services/message-service";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {ResponseData} from "../../../models/responses/response-data";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";

/**
 * 그리드 모델
 */
const gridModel = new BudgetPlanGridData();

/**
 * 마운트 핸들링
 */
onMounted(() => {

});

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
  apiUri : '/api/v1/BudgetPlan' ,
  pageCount: 40 ,
  skip: 0 ,
  searchFields: ['isAbove500K'] ,
  searchKeywords: [ 'true' ],
  sortFields: [ 'approvalDate' ],
  sortOrders: [ 'desc' ],
}

/**
 * 데이터 추가 다이얼로그
 */
const addDialog = ref(false);

/**
 * 데이터 추가 원본 요청 데이터
 */
const model = ref<RequestBudgetPlan>(new RequestBudgetPlan());
// 500K 로 설정
model.value.isAbove500K = true;

/**
 * 데이터를 추가한다.
 */
const requestAdd = () => {
  console.log('requestAdd',model);

  // 유효하지 않은경우
  if(isValid() == false) {
    messageService.showWarning("입력하지 않은 데이터가 있습니다");
    return;
  }

  // 커뮤니케이션 시작
  communicationService.inCommunication();

  // 데이터를 입력한다.
  HttpService.requestPost<ResponseData<ResponseBudgetPlan>>(requestQuery.apiUri , model.value).subscribe({
    next(response) {
      console.log('requestAdd',response);

      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      messageService.showSuccess(`데이터가 등록되었습니다.`);
    } ,
    error(err) {
      console.error('Error loading data', err);
    } ,
    complete() {
      addDialog.value = false;
      gridRef.value.doRefresh();
      communicationService.offCommunication();
    },
  });
}

/**
 * 그리드 래퍼런스
 */
const gridRef = ref(null);

/**
 * 컨트리 비지니스 매니저 리스트
 */
let cbmList : ResponseCountryBusinessManager [] = [];

/**
 * CBM 데이터가 업데이트 되었을때
 * @param items 이벤트 객체
 */
const onDataUpdatedCBM = (items: any) => {
  console.log('onDataUpdatedCBM' , items);
  cbmList = items;
}

/**
 * 비지니스 유닛
 */
let businessUnits = ref([]);

/**
 * CBM 이 변경되었을때
 * @param cbmId
 */
const changeCBM = (cbmId: any) => {
  // 선택된 값 초기화
  model.value.businessUnitId = "";

  // 대상 CBM 을 찾는다.
  const cbms = cbmList.filter(i => i.id === cbmId);

  // 비지니스 유닛
  businessUnits.value = [];

  // 하나이상의 CBM 을 찾은경우
  if(cbms.length > 0)
    businessUnits.value = cbms[0].businessUnits;

  console.log('changeCBM' , businessUnits);
}

/**
 * 유효성 여부
 */
const isValid = () => {
  console.log('isValid',model);

  if(model.value.approvalDate === ''
  || model.value.sectorId === ''
  || model.value.businessUnitId === ''
  || model.value.costCenterId === ''
  || model.value.countryBusinessManagerId === '') {
    return false;
  }
}

/**
 * 삭제할 데이터
 */
let removeItems : Array<ResponseBudgetPlan> = [];

/**
 * 데이터 삭제를 요청한다.
 * @param items
 */
const remove = (items : Array<ResponseBudgetPlan>) => {
  // 삭제할 데이터를 보관
  removeItems = items;
}

</script>

<template>
  <common-grid :is-use-insert="gridModel.isUseInsert"
               :input-colum-defined="gridModel.columDefined"
               @onNewRowAdded="onNewRowAdded"
               @onAdd="addDialog = true"
               @onRemove="remove"
               :is-use-buttons="true"
               :query-request="requestQuery"
               ref="gridRef"
  />

  <!--데이터 추가 다이얼로그-->
  <v-dialog v-model="addDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>Above 500k 예산추가</h4>
      </v-card-title>
      <v-card-subtitle class="">예산을 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <common-select required v-model="model.countryBusinessManagerId" @onChange="changeCBM" @onDataUpdated="onDataUpdatedCBM" title="name" value="id" label="Country Business Manager" requestApiUri="/api/v1/CountryBusinessManager" />
          <v-select required v-model="model.businessUnitId" label="Business Unit" item-title="name" item-value="id" :items="businessUnits" :disabled="businessUnits.length === 0"></v-select>
          <common-select required v-model="model.costCenterId" title="value" value="id" label="Cost Center" requestApiUri="/api/v1/CostCenter" />
          <common-select required v-model="model.sectorId" title="value" value="id" label="Sector" requestApiUri="/api/v1/Sector" />
          <v-text-field required v-model="model.approvalDate" label="Approval Date" variant="outlined" @keyup.enter="requestAdd()"></v-text-field>
          <v-text-field v-model="model.description" label="Description" variant="outlined"  @keyup.enter="requestAdd()"></v-text-field>
          <v-text-field v-model="model.budgetTotal" label="BudgetTotal" variant="outlined"  @keyup.enter="requestAdd()"></v-text-field>
          <v-text-field v-model="model.ocProjectName" label="OcProjectName" variant="outlined"  @keyup.enter="requestAdd()"></v-text-field>
          <v-text-field v-model="model.bossLineDescription" label="BossLine Description" variant="outlined"  @keyup.enter="requestAdd()"></v-text-field>
          <v-btn variant="outlined" @click="requestAdd()" class="mr-2" color="info" >추가</v-btn>
          <v-btn variant="outlined" @click="addDialog = false" class="mr-2" color="error">취소</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
</style>
