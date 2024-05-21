<script setup lang="ts">
import {onMounted, ref} from "vue";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {BudgetApprovedGridData} from "./budget-approved-grid-data";
import {messageService} from "../../../services/message-service";
import {communicationService} from "../../../services/communication-service";
import {ResponseData} from "../../../models/responses/response-data";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {firstValueFrom, Observable} from "rxjs";
import {CommonButtonDefinitions} from "../../../shared/grids/common-grid-button";
import {RequestUploadFile} from "../../../models/requests/files/request-upload-file";
import {ResponseList} from "../../../models/responses/response-list";
import {RequestBudgetPlan} from "../../../models/requests/budgets/request-budget-plan";
import {RequestCostCenter} from "../../../models/requests/budgets/request-cost-center";
import CommonGridDialog from "../../../shared/grids/common-grid-dialog.vue";
import {RequestBudgetApproved} from "../../../models/requests/budgets/request-budget-approved";
import CommonImportDialog from "../../../shared/common-import-dialog.vue";

/**
 * Grid Model
 */
const gridModel = new BudgetApprovedGridData();
/**
 * Props
 */
const props = defineProps({
  isAbove500k: { Type: String,  required: true,},
  title: {Type: String, required: true,},
});
/**
 * 그리드 래퍼런스
 */
const gridReference = ref(null);
/**
 * Common dialog
 */
const dialog = ref();
const importRef = ref(null);
/**
 * 마운트핸들링
 */
onMounted(async () => {
  gridModel.requestQuery.searchKeywords.push(props.isAbove500k.toString());
  gridModel.requestQuery.searchFields.push("isAbove500k");
  await gridModel.initializeListAsync();
});
const dataModel = ref<RequestBudgetApproved>(new RequestBudgetApproved());
const importFileDownload = async() => {
  // Request to Server
  HttpService.requestGetFileAutoNotify(`${gridModel.requestQuery.apiUri}/Import/Excel/File`).subscribe({
    next(response) {
      if(response == null)
        return;

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
  });
}
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

  const responseData = await firstValueFrom<ResponseList<RequestBudgetApproved>>(
    HttpService.requestPost<ResponseData<any>>(`${gridModel.requestQuery.apiUri}/Import/Excel`, param) as Observable<ResponseList<RequestBudgetApproved>>);
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

  await delay(2000);
  importRef.value.updateStep(99);
  console.log('gridModel.columDefined',gridModel.columDefined);

  importRef.value.updateItems(gridModel.columDefined, responseData.items);
  communicationService.offTransmission();
}
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
}
const delay =  (ms) => new Promise(resolve => setTimeout(resolve, ms));
</script>

<template>
  <common-grid
               :input-colum-defined="gridModel.columDefined"
               :query-request="gridModel.requestQuery"
               :grid-title="((props.isAbove500k as String).toLowerCase() == 'true') ? '예산승인_Above_500K_Budget' : '예산승인_Below_500K_Budget'"
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
  <common-import-dialog ref="importRef" @submit="submit($event)"></common-import-dialog>
</template>

<style scoped lang="css">
</style>
