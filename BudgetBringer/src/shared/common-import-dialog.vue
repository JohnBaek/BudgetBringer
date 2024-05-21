<script setup lang="ts">
import {onBeforeMount, ref} from "vue";
import {communicationService} from "../services/communication-service";
import {AgGridVue} from "ag-grid-vue3";
import {EnumResponseResult} from "../models/enums/enum-response-result";

const dialog = ref(false);
const fileUploadStep = ref(1);
/**
 * outgoing
 */
const emits = defineEmits<{
  (e: 'submit', Array) : void,
}>();
const columnDefined = ref(null);
const items = ref(null);
const rowSelection = ref(null);
const isRowSelectable = ref(null);
/**
 * incoming calls from parent
 */
defineExpose({
  // Open add dialog
  show() {
    fileUploadStep.value = 1;
    columnDefined.value = [];
    items.value = [];
    dialog.value = true;
  } ,
  hide() {
    dialog.value = false;
  } ,
  updateStep(step) {
    fileUploadStep.value = step;
  },
  increaseStep() {
    fileUploadStep.value++;
  },
  updateItems(cols, datas) {
    columnDefined.value = cols;
    columnDefined.value = [{
      field: 'result',
      headerName: '변환결과'  ,
      width:130 ,
      headerCheckboxSelection: true,
      checkboxSelection: true,
      showDisabledCheckboxes: true,
      cellRenderer: (params) => {
        switch (params.value) {
          case EnumResponseResult.success:
            return "변환성공";
          default:
            return "변환실패"; // 값이 열거형에 없는 경우
        }
      },
    },
      {
        field: 'message',
        headerName: '메세지'  ,
        width:100 ,
      }
    ].concat(columnDefined.value);

    items.value = datas;
  }
});
onBeforeMount(() => {
  rowSelection.value = "multiple";
  isRowSelectable.value = (params) => {
    console.log('params',params)
    return !!params.data && params.data.enabled === true;
  };
});
const onGridReady = (params) => {
  gridApi.value = params.api;
};
/**
 * Grid API
 */
const gridApi = ref();
const gridSelectionChanged = () => {
  // 선택된 Row 를 업데이트한다.
  selectedRows.value = gridApi.value.getSelectedRows();
};
const selectedRows = ref([]);
/**
 * Subscribe
 */
const inCommunication = ref(false);
communicationService.subscribeCommunication().subscribe((communication) =>{
  inCommunication.value = communication;
});
</script>

<template>
  <v-dialog v-model="dialog" width="100%" >
    <v-card elevation="1" rounded class="mb-10 pa-5" >
      <v-card-title class="mt-5"><b>파일업로드</b>
      </v-card-title>
      <v-card-item v-if="fileUploadStep != 99">
        <v-timeline align="start" side="end" direction="horizontal">
          <v-timeline-item
            :dot-color="fileUploadStep == 1 ? 'green' : 'grey'"
            size="small"
          >
            <div class="d-flex">
              <strong class="me-4"></strong>
              <div :class="fileUploadStep == 1 ? 'text-black' : 'text-grey'">
                <strong>업로드 진행중</strong>
                <div class="text-caption">
                  서버로 파일을 업로드중입니다.
                  <v-progress-circular size="x-small" indeterminate v-if="fileUploadStep == 1"></v-progress-circular>
                </div>
              </div>
            </div>
          </v-timeline-item>

          <v-timeline-item
            :dot-color="fileUploadStep == 2 ? 'green' : 'grey'"
            size="small"
          >
            <div class="d-flex">
              <strong class="me-4"></strong>
              <div :class="fileUploadStep == 2 ? 'text-black' : 'text-grey'">
                <strong>분석중</strong>
                <div class="text-caption">
                  파일을 분석중입니다.
                  <v-progress-circular size="x-small" indeterminate v-if="fileUploadStep == 2"></v-progress-circular>
                </div>
              </div>
            </div>
          </v-timeline-item>

          <v-timeline-item
            :dot-color="fileUploadStep >= 3 ? 'green' : 'grey'"
            size="small"
          >
            <div class="d-flex">
              <strong class="me-4"></strong>
              <div :class="fileUploadStep >= 3 ? 'text-black' : 'text-grey'">
                <strong>분석완료</strong>
                <div class="text-caption">
                  잠시후 결과가 아래에 표시됩니다.
                </div>
              </div>
            </div>
          </v-timeline-item>
        </v-timeline>
      </v-card-item>

      <v-card-item v-if="fileUploadStep >= 3">
        <div>
          <v-chip color="primary">변환 성공 <b>   &nbsp;{{ items.filter(i => i.result == EnumResponseResult.success).length}}</b></v-chip>
          <v-chip color="error">변환 실패 <b>  &nbsp;{{ items.filter(i => i.result != EnumResponseResult.success).length}}</b></v-chip>
        </div>
      </v-card-item>

      <v-card-item v-if="fileUploadStep == 99">
        <ag-grid-vue
          @grid-ready="onGridReady"
          style="width: 100%; height: 900px;"
          :columnDefs="columnDefined"
          :rowData="items"
          :rowSelection="rowSelection"
          :suppressRowClickSelection="true"
          :isRowSelectable="isRowSelectable"
          class="ag-theme-alpine"
          :selected-rows="selectedRows"
          @selection-changed="gridSelectionChanged"
        >
        </ag-grid-vue>

      </v-card-item>

      <v-row class="mt-3 justify-end">
        <v-col style="display: flex; justify-content: flex-end;">
          <!--Cancel-->
          <v-btn width="100" elevation="1" class="mr-2" @click="dialog = false">
            <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
            <template v-if="!inCommunication">
              <pre class="text-grey"><b> 취소 </b></pre>
            </template>
          </v-btn>

          <!--Submit-->
          <v-btn :disabled="inCommunication || fileUploadStep != 99 || selectedRows.length == 0" width="100" elevation="1" color="primary" @click="emits('submit',selectedRows)">
            <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
            <template v-if="!inCommunication">
              <v-icon>mdi-checkbox-marked-circle</v-icon>
              <pre><b> 업로드 </b></pre>
            </template>
          </v-btn>
        </v-col>
      </v-row>
    </v-card>

  </v-dialog>
</template>

<style scoped lang="css">

</style>
