<script setup lang="ts">
import {onMounted, ref} from "vue";
import {BudgetPlanGridData} from "./budget-plan-grid-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {RequestBudgetPlan} from "../../../models/requests/budgets/request-budget-plan";
import {messageService} from "../../../services/message-service";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {ResponseData} from "../../../models/responses/response-data";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";
import {firstValueFrom} from "rxjs";
import CommonDialog from "../../../shared/common-dialog.vue";
import BudgetPlanDataForm from "./budget-plan-data-form.vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";

// 그리드 모델
const gridModel = new BudgetPlanGridData();

// 쿼리 정보
let requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetPlan' ,
  pageCount: 100 ,
  skip: 0 ,
  searchFields: [] ,
  searchKeywords: [],
  sortFields: [ 'regDate' ],
  sortOrders: [ 'desc' ],
}

// props 정의
const props = defineProps({
  // isAbove500k 정보
  isAbove500k: {
    Type: String,
    required: true,
  },
  // title 정보
  title: {
    Type: String,
    required: true,
  },
});

// 삭제 다이얼로그
const removeDialogReference = ref(false);

// 그리드 래퍼런스
const gridReference = ref(null);

// 데이터 추가 원본 요청 데이터
const requestModel = ref<RequestBudgetPlan>(new RequestBudgetPlan());

// 비지니스 유닛 리스트
let businessUnitsReference = ref([]);

// 삭제할 데이터
let removeItems : Array<ResponseBudgetPlan> = [];

// 수정할 데이터
let updateItem: ResponseBudgetPlan;

// 마운트핸들링
onMounted(() => {
  requestQuery.searchKeywords.push(props.isAbove500k.toString());
  requestQuery.searchFields.push("isAbove500k");
});

/**
 * 데이터를 추가한다.
 */
const requestAddData = () => {
  // 유효하지 않은경우
  if(isValidModel() == false) {
    messageService.showWarning("입력하지 않은 데이터가 있습니다");
    return;
  }

  // 커뮤니케이션 시작
  communicationService.notifyInCommunication();

  // 데이터를 입력한다.
  HttpService.requestPost<ResponseData<ResponseBudgetPlan>>(requestQuery.apiUri , requestModel.value).subscribe({
    next(response) {

      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      messageService.showSuccess(`데이터가 등록되었습니다.`);
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
      communicationService.notifyOffCommunication();
    } ,
    complete() {
      // 다이얼로그를 닫는다.
      addDialog.value = false

      // 데이터를 다시 로드한다.
      gridReference.value.doRefresh();

      // 커뮤니케이션을 종료한다.
      communicationService.notifyOffCommunication();
    },
  });
}

/**
 * 유효성 여부를 검증한다.
 */
const isValidModel = () => {
  if(requestModel.value.approvalDate === ''
  || requestModel.value.sectorId === ''
  || requestModel.value.businessUnitId === ''
  || requestModel.value.costCenterId === ''
  || requestModel.value.countryBusinessManagerId === '') {
    return false;
  }
}

/**
 * 데이터 팝업을 요청한다.
 * @param items 삭제할 데이터
 */
const showRemoveDialog = (items : Array<ResponseBudgetPlan>) => {
  removeDialogReference.value = true;

  // 삭제할 데이터를 보관
  removeItems = items;
}

/**
 * 추가 팝업을 요청한다.
 */
const showAddDialog = () => {
  addDialog.value = true;
  requestModel.value = new RequestBudgetPlan();
  requestModel.value.isAbove500K = (props.isAbove500k as String).toLowerCase() == "true";
}

/**
 * 데이터 수정 팝업을 요청한다.
 * @param item 수정할 데이터
 */
const showUpdateDialog = (item: ResponseBudgetPlan) => {
  communicationService.notifyInCommunication();
  updateItem = item;

  // 서버에서 대상하는 데이터를 조회한다.
  HttpService.requestGet<ResponseData<ResponseBudgetPlan>>(`${requestQuery.apiUri}/${item.id}`).subscribe({
    async next(response) {
      // 요청에 실패한경우
      if (response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // 대상 비지니스 매니저를 요청한다.
      const _responseCountryBusinessManager = await firstValueFrom(
        HttpService.requestGet(`/api/v1/CountryBusinessManager/${item.countryBusinessManagerId}`)
      ) as ResponseData<ResponseCountryBusinessManager> ;

      // 비지니스 유닛을 업데이트
      businessUnitsReference.value = _responseCountryBusinessManager.data.businessUnits;
      requestModel.value = Object.assign(requestModel.value, response.data);

      // 팝업을 연다.
      updateDialog.value = true;
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
    } ,
    complete() {
      communicationService.notifyOffCommunication();
    },
  });
}

/**
 * 데이터를 삭제한다.
 */
const requestRemoveData = () => {
  // 모든 데이터에 대해 처리
  for (const data of removeItems) {
    communicationService.notifyInCommunication();
    HttpService.requestDelete<ResponseData<any>>(`${requestQuery.apiUri}/${data.id}`).subscribe({
      next(response) {
        // 요청에 실패한경우
        if(response.result !== EnumResponseResult.success) {
          messageService.showError(`[${response.code}] ${response.message}`);
          return;
        }
        messageService.showSuccess(`데이터가 삭제되었습니다.`);
      } ,
      error(err) {
        messageService.showError('Error loading data'+err);
      } ,
      complete() {
        removeDialogReference.value = false;
        gridReference.value.doRefresh();
        communicationService.notifyOffCommunication();
      },
    });
  }
}

/**
 * 데이터를 수정한다.
 */
const requestUpdateData = () => {
  // 유효하지 않은경우
  if(isValidModel() == false) {
    messageService.showWarning("입력하지 않은 데이터가 있습니다");
    return;
  }

  communicationService.notifyInCommunication();
  HttpService.requestPut<ResponseData<any>>(`${requestQuery.apiUri}/${updateItem.id}`, requestModel.value).subscribe({
    next(response) {
      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      messageService.showSuccess(`데이터가 수정 되었습니다.`);
    } ,
    error() {
      updateDialog.value = false;
      communicationService.notifyOffCommunication();
    } ,
    complete() {
      updateDialog.value = false;
      gridReference.value.doRefresh();
      communicationService.notifyOffCommunication();
    },
  });
}
/**
 * When user double clicked the grid cell
 * @param $event
 */
const onDoubleClicked = ($event) => {
  const data = $event as ResponseBudgetPlan;
  showUpdateDialog(data);
}
/**
 * When form data updated
 * @param $event
 */
const updateRequestModel = ($event: RequestBudgetPlan) => {
  requestModel.value = $event;
}
const addDialog = ref(false);
const updateDialog = ref(false);
</script>

<template>
  <common-grid
               :input-colum-defined="gridModel.columDefined"
               :query-request="requestQuery"
               :grid-title="((props.isAbove500k as String).toLowerCase() == 'true') ? '예산계획_Above_500K_Budget' : '예산계획_Below_500K_Budget'"
               @onAdd="showAddDialog"
               @onRemove="showRemoveDialog"
               @onUpdate="showUpdateDialog"
               @onDoubleClicked="onDoubleClicked($event)"
               ref="gridReference"
  />

  <!-- Add Dialog -->
  <common-dialog v-model="addDialog" @cancel="addDialog = false" @submit="requestAddData()">
    <template v-slot:header-area>
      <div v-if="requestModel.isAbove500K"><b> 💵 예산계획추가 </b><div><b class="text-red">500K 이상</b></div></div>
      <div v-if="!requestModel.isAbove500K"><b> 💵 예산계획추가 </b><div><b class="text-blue">500K 이하</b></div></div>
    </template>
    <template v-slot:contents-area>
      <budget-plan-data-form v-model="requestModel" @update:data="updateRequestModel($event)" @submit="requestAddData()" />
    </template>
  </common-dialog>

  <!-- Update Dialog -->
  <common-dialog v-model="updateDialog" @cancel="updateDialog = false" @submit="requestUpdateData()">
    <template v-slot:header-area>
      <div v-if="requestModel.isAbove500K"><b> 💵 예산계획수정 </b><div><b class="text-red">500K 이상</b></div></div>
      <div v-if="!requestModel.isAbove500K"><b> 💵 예산계획수정 </b><div><b class="text-blue">500K 이하</b></div></div>
    </template>
    <template v-slot:contents-area>
      <budget-plan-data-form v-model="requestModel" @update:data="updateRequestModel($event)" @submit="requestUpdateData()" />
    </template>
  </common-dialog>

  <!-- Delete Dialog-->
  <v-dialog v-model="removeDialogReference" width="auto">
    <v-card min-width="250" title="삭제" text="삭제하시겠습니까?">
      <template v-slot:actions>
        <v-btn class="ms-auto" text="확인" @click="requestRemoveData"
        ></v-btn>
      </template>
    </v-card>
  </v-dialog>
</template>

<style  lang="css">

</style>
