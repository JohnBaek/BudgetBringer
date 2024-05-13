<script setup lang="ts">
import {ref} from "vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {CostCenterGridData} from "./cost-center-grid-data";
import {messageService} from "../../../services/message-service";
import {communicationService} from "../../../services/communication-service";
import {ResponseData} from "../../../models/responses/response-data";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {ResponseCostCenter} from "../../../models/responses/budgets/response-cost-center";
import {RequestCostCenter} from "../../../models/requests/budgets/request-cost-center";

// GridModel
const gridModel = new CostCenterGridData();

// Title
const title : string = 'CostCenter';

// Add dialog
const addDialog = ref(false);

// Remove Dialog
const removeDialog = ref(false);

// Update Dialog
const updateDialog = ref(false);

// Grid
const grid = ref(null);
/**
 * 데이터 추가 원본 요청 데이터
 */
const modelReference = ref<RequestCostCenter>(new RequestCostCenter());
/**
 * 삭제할 데이터
 */
let removeItems : Array<ResponseCostCenter> = [];
/**
 * 수정할 데이터
 */
let updateItem: ResponseCostCenter;

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
  HttpService.requestPost<ResponseData<ResponseCostCenter>>(gridModel.requestQuery.apiUri , modelReference.value).subscribe({
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
      grid.value.doRefresh();

      // 커뮤니케이션을 종료한다.
      communicationService.notifyOffCommunication();
    },
  });
}

/**
 * 유효성 여부를 검증한다.
 */
const isValidModel = () => {
  if(modelReference.value.value === ''){
    return false;
  }
  return true;
}

/**
 * 데이터 팝업을 요청한다.
 * @param items 삭제할 데이터
 */
const showRemoveDialog = (items : Array<ResponseCostCenter>) => {
  removeDialog.value = true;

  // 삭제할 데이터를 보관
  removeItems = items;
}

/**
 * 추가 팝업을 요청한다.
 */
const showAddDialog = () => {
  addDialog.value = true;
  modelReference.value = new RequestCostCenter();
}

/**
 * 데이터 수정 팝업을 요청한다.
 * @param item 수정할 데이터
 */
const showUpdateDialog = (item: ResponseCostCenter) => {
  communicationService.notifyInCommunication();
  updateItem = item;

  // 서버에서 대상하는 데이터를 조회한다.
  HttpService.requestGet<ResponseData<ResponseCostCenter>>(`${gridModel.requestQuery.apiUri}/${item.id}`).subscribe({
    async next(response) {
      // 요청에 실패한경우
      if (response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // 데이터를 업데이트한다.
      modelReference.value = Object.assign(modelReference.value, item);

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
    HttpService.requestDelete<ResponseData<any>>(`${gridModel.requestQuery.apiUri}/${data.id}`).subscribe({
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
        grid.value.doRefresh();
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
  HttpService.requestPut<ResponseData<any>>(`${gridModel.requestQuery.apiUri}/${updateItem.id}`, modelReference.value).subscribe({
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
      grid.value.doRefresh();
      updateDialog.value = false;
      communicationService.notifyOffCommunication();
    },
  });
}
</script>

<template>
  <common-grid
               :input-colum-defined="gridModel.columDefined"
               grid-title="CostCenter"
               :query-request="gridModel.requestQuery"
               @onAdd="showAddDialog"
               @onRemove="showRemoveDialog"
               @onUpdate="showUpdateDialog"
               ref="grid"
  />
  <!--데이터 추가 다이얼로그-->
  <v-dialog v-model="addDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>{{title}} 추가</h4>
      </v-card-title>
      <v-card-subtitle class="">{{title}} 을 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <v-text-field required v-model="modelReference.value" label="Value" variant="outlined" @keyup.enter="requestAddData()"></v-text-field>
          <v-btn variant="outlined" @click="requestAddData()" class="mr-2" color="info" >추가</v-btn>
          <v-btn variant="outlined" @click="addDialog = false" class="mr-2" color="error">취소</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>

  <!--데이터 수정 다이얼로그-->
  <v-dialog v-model="updateDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>{{title}} 예산수정</h4>
      </v-card-title>
      <v-card-subtitle class="">{{title}}를 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <v-text-field required v-model="modelReference.value" label="Value" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-btn variant="outlined" @click="requestUpdateData()" class="mr-2" color="info" >수정</v-btn>
          <v-btn variant="outlined" @click="updateDialog = false" class="mr-2" color="error">취소</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>

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
