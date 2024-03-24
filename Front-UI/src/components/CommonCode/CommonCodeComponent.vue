<template>
  <v-container fluid>
    <v-row>
      <!-- 리스트 영역 -->
      <v-col cols="12" md="3">
        <div class="scrollable-list bordered-list elevation-3">
          <v-list>
            <v-list-item-title >
              <v-container>
                <v-spacer></v-spacer>
                <v-btn density="compact" size="small" icon="mdi-plus" color="black" class="mb-3" @click="codeAddDialog = true"></v-btn>
              </v-container>
            </v-list-item-title>

            <v-list-item
              @click="selectedItem(item)"
              v-for="item in dummyCodes"
              :key="item.id"
            >
              <v-list-item-title>
                <h3>{{item.title}}</h3>
              </v-list-item-title>
              <v-list-item-subtitle>
                {{item.description}}
              </v-list-item-subtitle>
              <v-divider class="mt-3 mb-3"></v-divider>
            </v-list-item>
          </v-list>
        </div>
      </v-col>
      <v-col cols="12" md="9">
        <div class="elevation-3 pa-3">
          <v-container>
            <!-- 상단 섹션 -->
            <v-card elevation="1" rounded class="mb-10 pa-5">
              <v-card-title class=" mt-5"><h4>상위코드 관리
                <v-btn density="compact" icon="mdi-minus" color="black" size="small" class="mr-3 mb-1" @click="codeDeleteDialog = !codeDeleteDialog"></v-btn>
              </h4>
              </v-card-title>
              <v-card-subtitle class="">상위코드를 수정합니다 생성된 코드명은 변경할 수 없습니다.</v-card-subtitle>
              <v-row dense>
                <v-col cols="12" md="8" class="mt-5">
                  <v-text-field  label="코드명" variant="outlined"  v-model="currentItem.title" disabled></v-text-field>
                  <v-text-field  label="설명" @keyup.enter="codeEditDialog != codeDeleteDialog" variant="outlined"  v-model="currentItem.description" ></v-text-field>
                </v-col>
              </v-row>
              <v-row dense>
                <v-col cols="12" md="4" >
                  <v-row>
                    <v-spacer></v-spacer>
                  </v-row>
                </v-col>
              </v-row>
            </v-card>



            <!-- 테이블 섹션 -->
            <v-row>
              <v-col cols="12">
                <v-card  rounded elevation="1">
                  <v-card-title class="ml-5 mt-5"><h4>하위코드관리</h4></v-card-title>
                  <v-card-subtitle class="ml-5">테이블의 셀을 두번 클릭하시면 데이터를 수정할수 있습니다.</v-card-subtitle>
                  <v-container>
                    <ag-grid-vue
                      :rowData="rowData"
                      :columnDefs="colDefs"
                      style="height: 500px"
                      class="ag-theme-quartz"
                    >
                    </ag-grid-vue>
                  </v-container>
                </v-card>
              </v-col>
            </v-row>
          </v-container>
        </div>
      </v-col>
    </v-row>
  </v-container>

  <v-dialog
    v-model="codeDeleteDialog"
    width="auto"
  >
    <v-card
      min-width="250"
      title="코드 삭제"
      text="삭제하시겠습니까?"
    >
      <template v-slot:actions>
        <v-btn
          class="ms-auto"
          text="확인"
          @click="codeDeleteDialog = false"
        ></v-btn>
      </template>
    </v-card>
  </v-dialog>

  <v-dialog
    v-model="codeEditDialog"
    width="auto"
  >
    <v-card
      min-width="250"
      title="코드 수정"
      text="수정 되었습니다."
    >
      <template v-slot:actions>
        <v-btn
          class="ms-auto"
          text="확인"
          @click="codeEditDialog = false"
        ></v-btn>
      </template>
    </v-card>
  </v-dialog>

  <v-dialog
    v-model="codeAddDialog"
    width="auto"
  >
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>상위코드 추가</h4>
      </v-card-title>
      <v-card-subtitle class="">상위코드를 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="8" class="mt-5">
          <v-text-field  label="코드명" variant="outlined"></v-text-field>
          <v-text-field  label="설명" variant="outlined" ></v-text-field>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import {ref} from "vue";
import {AgGridVue} from "ag-grid-vue3";
import CommonCodeComponentButtonRenderer from "./CommonCodeComponentButtonRenderer.vue";

// 코드삭제 경고 컨펌
const codeDeleteDialog = ref(false);

// 코드수정 완료 컨펌
const codeEditDialog = ref(false);

const codeAddDialog = ref(false);

/**
 * 코드 설명 수정
 */
const showCodeEdit = {

}

/**
 * d
 */
const showCodeDelete = {

}

const dummyCodes = [
  {
    id:'1'
    , title : 'BU'
    , description: '부서코드'
  },
  {
    id:'2'
    , title : 'CBM'
    , description: '국가별 관리자'
  }
]

let currentItem = ref( {
  id:'2'
  , title : '코드명'
  , description: ''
});

const selectedItem = (item:any) => {
  currentItem.value = item;
};

const rowData = ref([
  { code: "H&N", description: "H&N 부서" },
  { code: "NR", description: "NR 부서" },
]);

// Column Definitions: Defines the columns to be displayed.
const colDefs = ref([
  { field: "code"  , headerName:"코드" , flex:2 ,  editable: true},
  { field: "description", headerName:"설명" ,flex:3 ,  editable: true},
  { field: "", cellRenderer: CommonCodeComponentButtonRenderer},
]);
</script>


<style>
.bordered-list {
  border: 1px solid rgba(0, 0, 0, 0.12); /* Vuetify의 테마에 맞는 색상 */
  height: 100vh; /* 뷰포트의 높이에 맞춤 */
  max-height: 100vh; /* 최대 높이를 뷰포트의 높이로 제한 */
  overflow-y: auto; /* 내용이 넘칠 경우 스크롤바 표시 */
}
.scrollable-content {
  height: 100vh; /* 뷰포트의 높이에 맞춤 */
  max-height: 100vh; /* 최대 높이를 뷰포트의 높이로 제한 */
  overflow-y: auto; /* 내용이 넘칠 경우 스크롤바 표시 */
}
</style>

