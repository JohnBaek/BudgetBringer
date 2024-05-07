<script setup lang="ts">
import {onMounted, ref} from "vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {BudgetApprovedGridData} from "./budget-approved-grid-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {messageService} from "../../../services/message-service";
import {communicationService} from "../../../services/communication-service";
import {ResponseBudgetApproved} from "../../../models/responses/budgets/response-budget-approved";
import {ResponseData} from "../../../models/responses/response-data";
import {HttpService} from "../../../services/api-services/http-service";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {RequestBudgetApproved} from "../../../models/requests/budgets/request-budget-approved";
import {firstValueFrom} from "rxjs";
import {ApprovalStatusDescriptions} from "../../../models/enums/enum-approval-status";
import CommonDialog from "../../../shared/common-dialog.vue";
import BudgetApprovedDataForm from "./budget-approved-data-form.vue";

/**
 * 그리드 모델
 */
const gridModel = new BudgetApprovedGridData();
/**
 * 쿼리 정보
 */
let requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetApproved' ,
  pageCount: 100 ,
  skip: 0 ,
  searchFields: [] ,
  searchKeywords: [],
  sortFields: [ 'regDate' ],
  sortOrders: [ 'desc' ],
}
/**
 * 스테이터스
 */
const statusOptions = ref();
/**
 * props 정의
 */
const props = defineProps({
  /**
   * isAbove500k 정보
   */
  isAbove500k: {
    Type: String,
    required: true,
  },
  /**
   * title 정보
   */
  title: {
    Type: String,
    required: true,
  },
});
/**
 * 데이터 추가 다이얼로그
 */
const addDialog = ref(false);
/**
 * 삭제 다이얼로그
 */
const removeDialog = ref(false);
/**
 *
 */
const updateDialog = ref(false);
/**
 * 그리드 래퍼런스
 */
const gridReference = ref(null);
/**
 * 데이터 추가 원본 요청 데이터
 */
const requestModel = ref<RequestBudgetApproved>(new RequestBudgetApproved());
/**
 * 비지니스 유닛 리스트
 */
let businessUnitsReference = ref([]);
/**
 * 삭제할 데이터
 */
let removeItems : Array<ResponseBudgetApproved> = [];
/**
 * 수정할 데이터
 */
let updateItem: ResponseBudgetApproved;

/**
 * 마운트핸들링
 */
onMounted(() => {
  requestQuery.searchKeywords.push(props.isAbove500k.toString());
  requestQuery.searchFields.push("isAbove500k");

  statusOptions.value = Object.entries(ApprovalStatusDescriptions).map(([status, description]) => ({
    status: parseInt(status),
    description
  }));
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
  HttpService.requestPost<ResponseData<ResponseBudgetApproved>>(requestQuery.apiUri , requestModel.value).subscribe({
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
      addDialog.value = false;

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
  return !(requestModel.value.approvalDate === ''
    || requestModel.value.sectorId === ''
    || requestModel.value.businessUnitId === ''
    || requestModel.value.costCenterId === ''
    || requestModel.value.countryBusinessManagerId === '');

}

/**
 * 데이터 팝업을 요청한다.
 * @param items 삭제할 데이터
 */
const showRemoveDialog = (items : Array<ResponseBudgetApproved>) => {
  removeDialog.value = true;

  // 삭제할 데이터를 보관
  removeItems = items;
}

/**
 * 추가 팝업을 요청한다.
 */
const showAddDialog = () => {
  addDialog.value = true;
  requestModel.value = new RequestBudgetApproved();
  requestModel.value.isAbove500K = (props.isAbove500k as String).toLowerCase() == "true";
}

/**
 * 데이터 수정 팝업을 요청한다.
 * @param item 수정할 데이터
 */
const showUpdateDialog = (item: ResponseBudgetApproved) => {
  communicationService.notifyInCommunication();
  updateItem = item;

  // 서버에서 대상하는 데이터를 조회한다.
  HttpService.requestGet<ResponseData<ResponseBudgetApproved>>(`${requestQuery.apiUri}/${item.id}`).subscribe({
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

      // 데이터를 업데이트한다.
      requestModel.value = Object.assign(requestModel.value, item);

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
 * When user double clicked the grid cell
 * @param $event
 */
const onDoubleClicked = ($event) => {
  const data = $event as ResponseBudgetApproved;
  showUpdateDialog(data);
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
        removeDialog.value = false;
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

  console.log('requestModel.value',requestModel.value);

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
      gridReference.value.doRefresh();
      updateDialog.value = false;
      communicationService.notifyOffCommunication();
    },
  });
}
/**
 * When form data updated
 * @param $event
 */
const updateRequestModel = ($event: RequestBudgetApproved) => {
  requestModel.value = $event;
}
</script>

<template>
  <common-grid
               :input-colum-defined="gridModel.columDefined"
               :query-request="requestQuery"
               :grid-title="((props.isAbove500k as String).toLowerCase() == 'true') ? '예산승인_Above_500K_Budget' : '예산승인_Below_500K_Budget'"
               @onAdd="showAddDialog"
               @onRemove="showRemoveDialog"
               @onUpdate="showUpdateDialog"
               @onDoubleClicked="onDoubleClicked($event)"
               ref="gridReference"
  />
  <!-- Add Dialog -->
  <common-dialog v-model="addDialog" @cancel="addDialog = false" @submit="requestAddData()">
    <template v-slot:header-area>
      <div v-if="requestModel.isAbove500K"><b> 👍🏻 예산승인추가 </b><div><b class="text-red">500K 이상</b></div></div>
      <div v-if="!requestModel.isAbove500K"><b> 👍🏻 예산승인추가 </b><div><b class="text-blue">500K 이하</b></div></div>
    </template>
    <template v-slot:contents-area>
      <budget-approved-data-form v-model="requestModel" @update:data="updateRequestModel($event)" @submit="requestAddData()" />
    </template>
  </common-dialog>

  <!-- Update Dialog -->
  <common-dialog v-model="updateDialog" @cancel="updateDialog = false" @submit="requestUpdateData()">
    <template v-slot:header-area>
      <div v-if="requestModel.isAbove500K"><b> 👍🏻 예산승인수정 </b><div><b class="text-red">500K 이상</b></div></div>
      <div v-if="!requestModel.isAbove500K"><b> 👍🏻 예산승인수정 </b><div><b class="text-blue">500K 이하</b></div></div>
    </template>
    <template v-slot:contents-area>
      <budget-approved-data-form v-model="requestModel" @update:data="updateRequestModel($event)" @submit="requestUpdateData()" />
    </template>
  </common-dialog>

  <!--삭제 다이얼로그-->
  <v-dialog v-model="removeDialog" width="auto">
    <v-card min-width="250" title="코드 삭제" text="삭제하시겠습니까?">
      <template v-slot:actions>
        <v-btn class="ms-auto" text="확인" @click="requestRemoveData"
        ></v-btn>
      </template>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
</style>
