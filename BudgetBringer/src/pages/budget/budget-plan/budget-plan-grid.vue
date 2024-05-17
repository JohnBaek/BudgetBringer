<script setup lang="ts">
import {onMounted, ref} from "vue";
import {BudgetPlanGridData} from "./budget-plan-grid-data";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {RequestBudgetPlan} from "../../../models/requests/budgets/request-budget-plan";
import {messageService} from "../../../services/message-service";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {ResponseData} from "../../../models/responses/response-data";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";
import {firstValueFrom, Observable} from "rxjs";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {CommonButtonDefinitions} from "../../../shared/grids/common-grid-button";
import CommonImportDialog from "../../../shared/common-import-dialog.vue";
import {RequestUploadFile} from "../../../models/requests/files/request-upload-file";
import {ResponseList} from "../../../models/responses/response-list";
import {RequestCostCenter} from "../../../models/requests/budgets/request-cost-center";
import CommonGridDialog from "../../../shared/grids/common-grid-dialog.vue";

// 그리드 모델
const gridModel = new BudgetPlanGridData();
const dialog = ref();
// props 정의
const props = defineProps({
  isAbove500k: { Type: String,  required: true,},
  title: {Type: String, required: true,},
});
const dataModel = ref<RequestBudgetPlan>(new RequestBudgetPlan());
// 그리드 래퍼런스
const gridReference = ref(null);
const importRef = ref(null);

// 마운트핸들링
onMounted(async () => {
  gridModel.requestQuery.searchKeywords.push(props.isAbove500k.toString());
  gridModel.requestQuery.searchFields.push("isAbove500k");
  await gridModel.initializeListAsync();
});

const importFile = async ($event) => {
  importRef.value.show();

  // Create form data
  const formData = new FormData();
  formData.append("formFile", $event);

  communicationService.inTransmission();
  let response = await firstValueFrom<ResponseData<any>>(HttpService.requestPost<ResponseData<any>>('/api/v1/file', formData));

  // 요청에 실패한경우
  if(response.result !== EnumResponseResult.success) {
    messageService.showError(`[${response.code}] ${response.message}`);
    return;
  }

  // 강제딜레이 2초후
  await delay(2000);
  importRef.value.increaseStep();
  messageService.showSuccess("분석중입니다");

  const param: RequestUploadFile = new RequestUploadFile();
  param.name = response.data.name;

  const responseData = await firstValueFrom<ResponseList<RequestBudgetPlan>>(HttpService.requestPost<ResponseData<any>>(`${gridModel.requestQuery.apiUri}/Import/Excel`, param) as Observable<ResponseList<RequestBudgetPlan>>);
  // 요청에 실패한경우
  if(responseData.result !== EnumResponseResult.success) {
    messageService.showError(`[${responseData.code}] ${responseData.message}`);
    return;
  }
  await delay(2000);
  importRef.value.increaseStep();

  responseData.items.forEach(i => {
    i['enabled'] = (i['result'] as EnumResponseResult) === EnumResponseResult.success;
    i.isAbove500K = props.isAbove500k == "true" || props.isAbove500k == true
  });

  console.log('responseData.items',responseData.items);
  await delay(2000);
  importRef.value.updateStep(99);
  console.log('gridModel.columDefined',gridModel.columDefined);

  importRef.value.updateItems(gridModel.columDefined, responseData.items);
  communicationService.offTransmission();
}

const importFileDownload = async() => {
  communicationService.inTransmission();

  console.log('importFileDownload')

  // Request to Server
  HttpService.requestGetFile(`${gridModel.requestQuery.apiUri}/Import/Excel/File`).subscribe({
    next(response) {
      if(response == null)
        return;

      console.log(1)

      // Create URL dummy link
      const url = window.URL.createObjectURL(response);

      // Create Anchor dummy
      const link = document.createElement('a');

      // Simulate Click
      link.href = url;
      link.setAttribute('download', `import-file.xlsx`);
      document.body.appendChild(link);
      link.click();

      // Remove Dummy
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    },
    error(err) {
      console.error('Error loading data', err);
    },
    complete() {
      setTimeout(() => {
        communicationService.offTransmission();
      },2000)
    },
  });
}

const delay =  (ms) => new Promise(resolve => setTimeout(resolve, ms));
const submit = async ($event) => {
  const params = $event as Array<RequestBudgetPlan>;
  if (params.length === 0)
    return;

  communicationService.notifyInCommunication();
  const responseData = (await firstValueFrom(HttpService.requestPost<ResponseList<any>>(`${gridModel.requestQuery.apiUri}/Import/list`, params)));

  if(responseData.success)
    messageService.showSuccess(`데이터 등록 성공: ${responseData.items.filter(i => i.success).length}\n데이터 등록 실패: ${responseData.items.filter(i => i.error).length}`);

  if(responseData.error)
    messageService.showError(`[${responseData.code}] ${responseData.message}`);

  gridReference.value.doRefresh();
  importRef.value.hide();
  communicationService.notifyOffCommunication();
  console.log('responseData',responseData)
}
</script>

<template>
  <common-grid
               :input-colum-defined="gridModel.columDefined"
               :query-request="gridModel.requestQuery"
               :grid-title="((props.isAbove500k as String).toLowerCase() == 'true') ? '예산계획_Above_500K_Budget' : '예산계획_Below_500K_Budget'"
               :show-buttons="CommonButtonDefinitions.actionGroup"
               @onAdd="dialog.showAddDialog()"
               @onUpdate="dialog.showUpdateDialog($event.id)"
               @onRemove="dialog.showRemoveDialog($event)"
               @onDoubleClicked="dialog.showUpdateDialog($event.id)"
               @import-file="importFile($event)"
               @import-excel-download="importFileDownload()"
               ref="gridReference"
  />
  <common-grid-dialog
    :input-colum-defined="gridModel.columDefined"
    :request-query="gridModel.requestQuery"
    :is-use-attach="true"
    v-model="dataModel"
    :title="title"
    :model-empty-value="new RequestCostCenter()"
    @submit="gridReference.doRefresh()"
    ref="dialog"
  />

<!--  &lt;!&ndash; Add Dialog &ndash;&gt;-->
<!--  <common-dialog v-model="addDialog" @cancel="addDialog = false" @submit="requestAddData()">-->
<!--    <template v-slot:header-area>-->
<!--      <div v-if="requestModel.isAbove500K"><b> 💵 예산계획추가 </b><div><b class="text-red">500K 이상</b></div></div>-->
<!--      <div v-if="!requestModel.isAbove500K"><b> 💵 예산계획추가 </b><div><b class="text-blue">500K 이하</b></div></div>-->
<!--    </template>-->
<!--    <template v-slot:contents-area>-->
<!--      <budget-plan-data-form v-model="requestModel" @update:data="updateRequestModel($event)" @submit="requestAddData()" />-->
<!--    </template>-->
<!--  </common-dialog>-->

<!--  &lt;!&ndash; Update Dialog &ndash;&gt;-->
<!--  <common-dialog v-model="updateDialog" @cancel="updateDialog = false" @submit="requestUpdateData()">-->
<!--    <template v-slot:header-area>-->
<!--      <div v-if="requestModel.isAbove500K"><b> 💵 예산계획수정 </b><div><b class="text-red">500K 이상</b></div></div>-->
<!--      <div v-if="!requestModel.isAbove500K"><b> 💵 예산계획수정 </b><div><b class="text-blue">500K 이하</b></div></div>-->
<!--    </template>-->
<!--    <template v-slot:contents-area>-->
<!--      <budget-plan-data-form v-model="requestModel" @update:data="updateRequestModel($event)" @submit="requestUpdateData()" />-->
<!--    </template>-->
<!--  </common-dialog>-->

<!--  &lt;!&ndash; Delete Dialog&ndash;&gt;-->
<!--  <v-dialog v-model="removeDialogReference" width="auto">-->
<!--    <v-card min-width="250" title="삭제" text="삭제하시겠습니까?">-->
<!--      <template v-slot:actions>-->
<!--        <v-btn class="ms-auto" text="확인" @click="requestRemoveData"-->
<!--        ></v-btn>-->
<!--      </template>-->
<!--    </v-card>-->
<!--  </v-dialog>-->

  <common-import-dialog ref="importRef" @submit="submit($event)"></common-import-dialog>
</template>

<style  lang="css">

</style>
