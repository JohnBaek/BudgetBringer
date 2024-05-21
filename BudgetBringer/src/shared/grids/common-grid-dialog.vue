<script setup lang="ts">
import {ref, watch, computed} from "vue";
import {communicationService} from "../../services/communication-service";
import {toClone} from "../../services/utils/object-util";
import {RequestQuery} from "../../models/requests/query/request-query";
import {HttpService} from "../../services/api-services/http-service";
import {ResponseData} from "../../models/responses/response-data";
import {firstValueFrom} from "rxjs";
import {messageService} from "../../services/message-service";
import {CommonDialogColumnModel, EnumModelType} from "./common-dialog-column-model";
import CommonFileUpload from "../common-file-upload.vue";

const props = defineProps({
  inputColumDefined : {Type: Array<any> , required: true , default: [] },
  requestQuery: { Type: RequestQuery, required: true , default: new RequestQuery("",0,100)} ,
  title : {},
  modelValue: {} ,
  modelEmptyValue: {},
  isUseAttach: {Type: Boolean, default: false }
});
const columnDefined = ref(props.inputColumDefined.filter(i => i.useAsModel == true));
/**
 * Data Model
 */
let model = ref();
/**
 * Copy already Uploaded files
 */
const uploadedFiles = ref([]);
/**
 * Dialogs
 */
const dialog = ref(false);
const addDialog = ref(false);
const updateDialog = ref(false);
const removeDialog = ref(false);
let removeItems = [];
const isUseAttachFile = ref(false);
/**
 * incoming calls from parent
 */
defineExpose({
  // Open add dialog
  showAddDialog() {
    model.value = toClone(props.modelEmptyValue);
    dismissAll();
    addDialog.value = true;
    dialog.value = true;
    if(props.isUseAttach) {
      model.value.attachedFiles = [];
    }
  } ,
  // Open update dialog
  async showUpdateDialog(id: string) {
    dismissAll();
    const response = await firstValueFrom<ResponseData<any>>(HttpService.requestGetAutoNotify<ResponseData<any>>(`${props.requestQuery.apiUri}/${id}`));
    model.value = response.data;
    updateDialog.value = true;
    dialog.value = true;
    if(props.isUseAttach) {
      uploadedFiles.value = JSON.parse(JSON.stringify(model.value.attachedFiles));
      model.value.attachedFiles = [];
      isUseAttachFile.value = true;
    }
  } ,
  // Open remove dialog
  async showRemoveDialog(items: []) {
    dismissAll();
    removeItems = [];
    removeItems = items;
    removeDialog.value = true;
  }
});

/**
 * outgoing
 */
const emits = defineEmits<{
  (e: 'submit') : void,
  (e: 'update:data', any) : any ,
}>();
/**
 * Add Data
 */
const add = async () => {
  if(isInvalid.value) {
    messageService.showWarning('입력되지 않은 데이터가 있습니다.');
    return;
  }

  const response = await firstValueFrom<ResponseData<any>>(HttpService.requestPostAutoNotify<ResponseData<any>>(props.requestQuery.apiUri, model.value));
  if(response.success)
    dismissAll();
  // Dispatch Event
  emits('submit');
}
/**
 * Update Data
 */
const update = async (id: string) => {
  if(isInvalid.value) {
    messageService.showWarning('입력되지 않은 데이터가 있습니다.');
    return;
  }

  const response = await firstValueFrom<ResponseData<any>>(
    HttpService.requestPutAutoNotify<ResponseData<any>>(`${props.requestQuery.apiUri}/${id}`, model.value));

  if(response.success)
    dismissAll();

  // Dispatch Event
  emits('submit');
}


/**
 * Remove data
 */
const remove = async () => {
  if(!removeItems || removeItems.length == 0) {
    messageService.showError("삭제할 대상이 선택되지 않았습니다.");
    return;
  }

  communicationService.notifyInCommunication();
  for (let item of removeItems) {
    const response = await firstValueFrom<ResponseData<any>>(
      HttpService.requestDelete<ResponseData<any>>(`${props.requestQuery.apiUri}/${item.id}`));

    if(response.error) {
      messageService.showError(response.message);
      return;
    }
  }
  communicationService.notifyOffCommunication();
  messageService.showSuccess("데이터가 삭제되었습니다.");

  dismissAll();
  // Dispatch Event
  emits('submit');
}
/**
 * Off all dialogs
 */
const dismissAll = () => {
  dialog.value = false;
  addDialog.value = false;
  updateDialog.value = false;
  removeDialog.value = false;
}
/**
 * Watch
 */
watch(model, (newValue) => {
  emits("update:data", newValue);
}, { deep: true });
const inCommunication = ref(false);
communicationService.subscribeCommunication().subscribe((communication) =>{
  inCommunication.value = communication;
});

/**
 * Dialog Configurations
 */
const dialogType = computed(() => {
  if (addDialog.value) {
    return { color: 'primary', text: '추가', action: '추가' };
  }
  if (updateDialog.value) {
    return { color: 'warning', text: '수정', action: '수정' };
  }
  return { color: '', text: '', action: '' }; // 기본 값
});

/**
 * Check Required fields
 */
const isInvalid = computed (() => {
  const requiredProperties = columnDefined.value.filter(item => (item as CommonDialogColumnModel).isRequired);
  // eslint-disable-next-line vue/no-side-effects-in-computed-properties
  model.value.isAbove500k = props.title.toString().indexOf('Above') > -1;

  for (const item of requiredProperties) {
    const required = (item as CommonDialogColumnModel);
    if(!model.value[required.modelPropertyName])
      return true;
  }
  return false;
});

/**
 * submit
 */
const submit = () => {
  // Invalid valued
  if(isInvalid.value)
    return; 

  // Update dialog displayed
  if(updateDialog.value)
    update(model.value.id);
  // Add dialog displayed
  else if (addDialog.value)
    add();
}

</script>

<template>
  <!--Data ADD Common Dialog-->
  <v-dialog v-model="dialog" width="800" @keyup.enter="submit()">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><b>{{title}}</b>
      </v-card-title>

      <v-card-subtitle>
        <v-chip :color="dialogType.color">{{ dialogType.text }}</v-chip> {{ title }}을(를) {{ dialogType.action }}합니다.
      </v-card-subtitle>

      <v-row class="ma-1">
        <v-col cols="12" :md="columnDefined.length == 1 ? '12' : '6'"
               v-for="(item, key) in columnDefined as Array<CommonDialogColumnModel>"
               :key="key">
          <div>
            <div class="mb-1 text-grey-darken-3" >
              <b>{{ item.headerName }} </b> <v-chip style="height:15px" v-if="item.isRequired" variant="plain" color="red">필수</v-chip>
            </div>
          </div>

          <!-- Text Input -->
          <div v-if="item.inputType === EnumModelType.text">
            <v-text-field outlined variant="outlined"
                          class="custom-field-margin"
                          density="compact"
                          v-model="model[item.modelPropertyName]"
                          :placeholder="`${ item.headerName }을(를) 입력해주세요`"
            />
          </div>

          <!-- Number Input -->
          <div v-else-if="item.inputType === EnumModelType.number">
            <v-text-field type="number"
                          outlined variant="outlined"
                          class="custom-field-margin"
                          density="compact"
                          v-model="model[item.modelPropertyName]"
                          :placeholder="`${ item.headerName }을(를) 입력해주세요`"
            />
          </div>

          <!-- Select -->
          <div v-else-if="item.inputType === EnumModelType.select">
            <v-select
              class="custom-select-margin"
              :item-title="item.selectTitle"
              :item-value="item.selectValue"
              :items="item.selectItems"
              :placeholder="`${ item.headerName }을(를) 선택해주세요`"
              v-model="model[item.modelPropertyName]"
              variant="outlined"
              density="compact"
            ></v-select>
          </div>
        </v-col>
        <v-col cols="12" md="12">
          <common-file-upload :uploaded-files="uploadedFiles" v-model="model.attachedFiles" v-if="isUseAttach"></common-file-upload>
        </v-col>
      </v-row>

      <v-row dense>
        <v-spacer></v-spacer>
        <v-btn class="mr-2"  width="100" elevation="1" color="light-grey" @click="dialog = false">
          <div>
            <pre><b>취소</b></pre>
          </div>
        </v-btn>

        <div v-if="addDialog">
          <v-btn
            v-if="addDialog"
            class="mr-2" :disabled="inCommunication || isInvalid" width="100" elevation="1" color="info" @click="add()">
            <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
            <template v-if="!inCommunication">
              <v-icon>mdi-checkbox-marked-circle</v-icon>
              <pre><b> 추가 </b></pre>
            </template>
          </v-btn>
        </div>

        <div v-if="updateDialog">
          <v-btn class="mr-2" :disabled="inCommunication || isInvalid" width="100" elevation="1" color="warning" @click="update(model.id)">
            <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
            <template v-if="!inCommunication">
              <v-icon>mdi-checkbox-marked-circle</v-icon>
              <pre><b> 수정 </b></pre>
            </template>
          </v-btn>
        </div>
      </v-row>
    </v-card>
  </v-dialog>

  <!--삭제 다이얼로그-->
  <v-dialog v-model="removeDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><b>{{title}}</b>
      </v-card-title>

      <v-card-subtitle class="">
        <v-chip color="error">삭제</v-chip> <b>{{removeItems.length}} 개의</b> {{title}}을 삭제합니다. 삭제 하시겠습니까?</v-card-subtitle>
      <div class="mt-5 mb-5"></div>
      <v-row dense>
        <v-spacer></v-spacer>
        <v-btn class="mr-2"  width="100" elevation="1" color="light-grey" @click="removeDialog = false">
          <div>
            <pre><b>취소</b></pre>
          </div>
        </v-btn>
        <v-btn class="mr-2" :disabled="inCommunication" width="100" elevation="1" color="error" @click="remove()">
          <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
          <template v-if="!inCommunication">
            <v-icon>mdi-delete-circle</v-icon>
            <pre><b> 삭제 </b></pre>
          </template>
        </v-btn>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">

</style>
