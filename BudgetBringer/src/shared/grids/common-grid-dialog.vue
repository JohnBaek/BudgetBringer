<script setup lang="ts">
import {ref, watch} from "vue";
import {communicationService} from "../../services/communication-service";
import {toClone} from "../../services/utils/object-util";
import {RequestQuery} from "../../models/requests/query/request-query";
import {HttpService} from "../../services/api-services/http-service";
import {ResponseData} from "../../models/responses/response-data";
import {firstValueFrom} from "rxjs";
import {messageService} from "../../services/message-service";

const props = defineProps({
  inputColumDefined : {Type: Array<any> , required: true , default: [] },
  requestQuery: { Type: RequestQuery, required: true , default: new RequestQuery("",0,100)} ,
  title : {},
  modelValue: {} ,
  modelEmptyValue: {},
});
const columnDefined = ref(props.inputColumDefined.filter(i => i.useAsModel == true));
/**
 * Data Model
 */
let model = ref();
/**
 * Dialogs
 */
const dialog = ref(false);
const addDialog = ref(false);
const updateDialog = ref(false);
const removeDialog = ref(false);

let removeItems = [];

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
  } ,
  // Open update dialog
  async showUpdateDialog(id: string) {
    dismissAll();
    const response = await firstValueFrom<ResponseData<any>>(HttpService.requestGetAutoNotify<ResponseData<any>>(`${props.requestQuery.apiUri}/${id}`));
    model.value = response.data;
    updateDialog.value = true;
    dialog.value = true;
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
      HttpService.requestDelete<ResponseData<any>>(`${props.requestQuery.apiUri}/${item.id}`, model.value));

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

</script>

<template>
  <!--Data ADD Common Dialog-->
  <v-dialog v-model="dialog" width="800">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><b>{{title}}</b>
      </v-card-title>

      <v-card-subtitle class="" v-if="addDialog">
        <v-chip color="primary">추가</v-chip> {{title}}을 추가합니다.</v-card-subtitle>

      <v-card-subtitle class="" v-if="updateDialog">
        <v-chip color="warning">수정</v-chip> {{title}}을 수정합니다.</v-card-subtitle>

      <v-row class="ma-1">
        <v-col cols="12" :md="columnDefined.length == 1 ? '12' : '6'"
               v-for="(item, key) in columnDefined"
               :key="key">

          <div>
            <div class="mb-1" v-if="!item.isRequired">
              <b>{{ item['headerName'] }} </b>
            </div>
            <div class="mb-1" v-if="item.isRequired">
              <b>{{ item['headerName'] }} <v-chip variant="plain" color="red">필수</v-chip></b>
            </div>
          </div>

          <v-text-field v-if="item.inputType && item.inputType === 'text'"
                        outlined variant="outlined"
                        density="compact"
                        v-model="model[item.field]"
                        :placeholder="`${ item['headerName'] }을(를) 입력해주세요`"
                        @keyup.enter="add()"
          />
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
            class="mr-2" :disabled="inCommunication" width="100" elevation="1" color="info" @click="add()">
            <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
            <template v-if="!inCommunication">
              <v-icon>mdi-checkbox-marked-circle</v-icon>
              <pre><b> 추가 </b></pre>
            </template>
          </v-btn>
        </div>

        <div v-if="updateDialog">
          <v-btn class="mr-2" :disabled="inCommunication" width="100" elevation="1" color="warning" @click="update(model.id)">
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
