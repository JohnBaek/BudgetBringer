<script setup lang="ts">
import {onMounted, ref} from "vue";
import {UserManagementGridData} from "./user-management-grid-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {messageService} from "../../../services/message-service";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {ResponseData} from "../../../models/responses/response-data";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";
import {RequestUserChangePassword} from "../../../models/requests/users/request-user-change-password";
import CommonGrid from "../../../shared/grids/common-grid.vue";

/**
 * 그리드 모델
 */
const gridModel = new UserManagementGridData();
/**
 * 쿼리 정보
 */
let requestQuery :RequestQuery = {
  apiUri : '/api/v1/Admin/Users' ,
  pageCount: 1000 ,
  skip: 0 ,
  searchFields: [] ,
  searchKeywords: [],
  sortFields: [ 'displayName' ],
  sortOrders: [ 'asc' ],
}
/**
 *
 */
const updateDialogReference = ref(false);
/**
 * 그리드 래퍼런스
 */
const gridReference = ref(null);
/**
 * 데이터 추가 원본 요청 데이터
 */
const modelReference = ref<RequestUserChangePassword>(new RequestUserChangePassword());

/**
 * 마운트핸들링
 */
onMounted(() => {
});

/**
 * 유효성 여부를 검증한다.
 */
const isValidModel = () => {
  if(modelReference.value.password === ''
  || modelReference.value.reWritePassword === ''
  || modelReference.value.id === ''
  ) {
    return false;
  }


}
/**
 * 데이터 수정 팝업을 요청한다.
 * @param item 수정할 데이터
 */
const showUpdateDialog = (item: ResponseBudgetPlan) => {
  modelReference.value.id = item.id;
  updateDialogReference.value = true;
}
/**
 * 데이터를 수정한다.
 */
const requestUpdateData = () => {
  // 일치하지않는 패스워드인경우
  if(modelReference.value.password !== modelReference.value.reWritePassword
  ) {
    messageService.showWarning("패스워드를 일치해서 입력해주세요");
    return false;
  }

  // 유효하지 않은경우
  if(isValidModel() == false) {
    messageService.showWarning("입력하지 않은 데이터가 있습니다");
    return;
  }

  communicationService.inCommunication();
  HttpService.requestPut<ResponseData<any>>(`${requestQuery.apiUri}`, modelReference.value).subscribe({
    next(response) {
      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      messageService.showSuccess(`데이터가 수정 되었습니다.`);
    } ,
    error() {
      updateDialogReference.value = false;
      communicationService.offCommunication();
    } ,
    complete() {
      gridReference.value.doRefresh();
      updateDialogReference.value = false;
      communicationService.offCommunication();
    },
  });
}
</script>

<template>
  <common-grid :is-use-insert="gridModel.isUseInsert"
               :input-colum-defined="gridModel.columDefined"
               :is-use-buttons="true"
               :query-request="requestQuery"
               @onUpdate="showUpdateDialog"
               :useButtons="['update','refresh']"
               ref="gridReference"
  />
  <!--데이터 수정 다이얼로그-->
  <v-dialog v-model="updateDialogReference" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>사용자 비밀번호 수정</h4>
      </v-card-title>
      <v-card-subtitle class="">비밀번호를 수정합니다. 엔터키를 누르면 수정됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <v-text-field type="password" required v-model="modelReference.password" label="Password" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-text-field type="password" required v-model="modelReference.reWritePassword" label="Re-Write" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
</style>
