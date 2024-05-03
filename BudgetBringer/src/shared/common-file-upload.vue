<script setup lang="ts">
import {ResponseFileUpload} from "../models/responses/files/response-upload-file";
import {ref, watch} from "vue";
import {communicationService} from "../services/communication-service";
import {HttpService} from "../services/api-services/http-service";
import {ResponseData} from "../models/responses/response-data";
import {messageService} from "../services/message-service";

// API URI
const apiUri : string = '/api/v1/file';

// Incoming
const definedProps = defineProps({
  // Already Attached files
  uploadedFiles: Array<ResponseFileUpload> ,
  modelValue: { type: Array<any> }
});

// Outgoing
const definedEmits = defineEmits<{
  // Update Uploaded list
  (e: "update:data", Array),
}>();

console.log('definedProps.uploadedFiles',definedProps.uploadedFiles);

// To References : Already Uploaded ( Persisted ) Files
const _uploadedFiles = ref(definedProps.uploadedFiles ?? []);
console.log('_uploadedFiles',_uploadedFiles);

// Store uploaded temp results
const uploadFiles = ref([]);

// Remove Dialog
const dialogRemove = ref(false);

// Upload Field
const fileUploadField = ref();

/**
 * File uploaded List
 */
const model = ref(definedProps.modelValue);
watch(model, (newValue) => {
  definedEmits("update:data", newValue);
}, {deep:true});

/**
 * Persists
 */
let selectedPersistItem: ResponseFileUpload = null;
const showDialogRemovePersist = (item: ResponseFileUpload) => {
  selectedPersistItem = item;
  dialogRemove.value = true;
}
/**
 * Request to Server : Remove Persist file
 */
const requestRemovePersist = () => {
  // Is invalid
  if(!selectedPersistItem) {
    messageService.showError('파일이 선택되지 않았습니니다.');
    return;
  }

  communicationService.notifyInCommunication();
  HttpService.requestDelete<ResponseData<any>>(`${apiUri}/${selectedPersistItem.id}`).subscribe({
    next(response) {
      // 요청에 실패한경우
      if(!response.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      _uploadedFiles.value = _uploadedFiles.value.filter(i => i.id != selectedPersistItem.id).slice();
      messageService.showSuccess(`데이터가 삭제되었습니다.`);
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
    } ,
    complete() {
      dialogRemove.value = false;
      communicationService.notifyOffCommunication();
    },
  });
}
/**
 * Request to Server : Upload Temp file
 */
const requestUploadTempFile = () => {
  const blob = fileUploadField.value[0];
  if(!blob) {
    messageService.showError('파일을 선택해주세요');
    return;
  }

  // Create form data
  const formData = new FormData();
  formData.append("formFile", blob);

  communicationService.notifyInCommunication();
  HttpService.requestPost<ResponseData<ResponseFileUpload>>(`${apiUri}`, formData)
  .subscribe({
    next(response) {
      // 요청에 실패한경우
      if(!response.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // Clear files
      fileUploadField.value = null;

      // Update upload files
      model.value.push(response.data);
      messageService.showSuccess(`파일이 추가되었습니다.`);
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
 * Try remove temp files
 * @param item
 */
const removeTempUploadedFile = (item) => {
  // Try find index
  const index = model.value.findIndex(i => i.id === item.id);

  // Is valid Index
  if (index !== -1) {
    model.value.splice(index, 1);
  }
}
</script>

<template>
  <v-row>

    <v-col md="12" lg="12" sm="12">
      <!-- Shows Previous Attached Files -->
      <v-list density="compact">
        <v-card-title><v-icon color="primary">mdi-checkbox-marked-circle</v-icon> 첨부파일</v-card-title>
        <v-card-subtitle>기존에 첨부했던 파일 입니다.</v-card-subtitle>
        <v-list-item v-for="(item, index) in _uploadedFiles" :key="index" link>
          <v-row dense >
            <v-col md="11" lg="11" sm="11">
              <a :href="'/' + item.url" target="_blank">{{ item.name }}</a>
            </v-col>
            <v-col md="1" lg="1" sm="1" align="right" @click="showDialogRemovePersist(item)">
              <v-icon>mdi-close</v-icon>
            </v-col>
          </v-row>
        </v-list-item>
      </v-list>
    </v-col>
    <v-col md="12" lg="12" sm="12">
      <!-- Will Upload Files (NEW) -->
      <v-list density="compact">
        <v-card-title><v-icon color="green">mdi-checkbox-marked-circle</v-icon> 업로드 대기중</v-card-title>
        <v-card-subtitle>임시 업로드된 파일로 확인 버튼을 누른후에 저장됩니다.</v-card-subtitle>
        <v-list-item v-for="(item, index) in model" :key="index" link>
          <v-row dense >
            <v-col md="11" lg="11" sm="11">
              {{ (item as ResponseFileUpload).originalFileName }}
            </v-col>
            <v-col  md="1" lg="1" sm="1" align="right" @click="removeTempUploadedFile(item)">
              <v-icon>mdi-close</v-icon>
            </v-col>
          </v-row>
        </v-list-item>
      </v-list>
    </v-col>
  </v-row>

  <v-row class="mt-3">
    <v-col cols="100%">
      <v-file-input density="compact" label="파일을 추가하려면 클릭하세요" v-model="fileUploadField" variant="outlined"></v-file-input>
    </v-col>
    <v-col cols="auto">
      <v-btn color="primary" @click="requestUploadTempFile()" :disabled="!fileUploadField || fileUploadField.length == 0">추가</v-btn>
    </v-col>
  </v-row>
  <v-label v-if="uploadFiles.length > 0">최종적으로 확인을 눌러야 파일이 저장됩니다.</v-label>

  <!-- Dialog of Remove-->
  <v-dialog v-model="dialogRemove" width="auto">
    <v-card min-width="250" title="삭제" text="삭제하시겠습니까?">
      <template v-slot:actions>
        <v-btn class="ms-auto" text="확인" @click="requestRemovePersist()" />
      </template>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
</style>
