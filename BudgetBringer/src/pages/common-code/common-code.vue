<script setup lang="ts">
import {ref} from "vue";
import {messageService} from "../../services/message-service";
import {AgGridVue} from "ag-grid-vue3";
import CommonCodeGridRenderer from "./common-code-grid-renderer.vue";
import {log} from "node:util";
import {BudgetPlanGridData} from "../budget/budget-plan/budget-plan-grid-data";

/**
 * 통신중 여부
 */
const inCommunication = ref(false);

/**
 * 상위코드 추가 다이얼로그
 */
const rootCodeAddDialog = ref(false);

/**
 * 상위코드 삭제 다이얼로그
 */
const rootCodeDeleteDialog = ref(false);

/**
 * 하위코드 추가 다이얼로그
 */
const underCodeAddDialog = ref(false);

/**
 * 입력 데이터
 */
let inputRow = {};

/**
 * Top 인서트 Pine
 */
const pinnedTopRowData = [inputRow];


/**
 * 상위코드 추가에서 사용하는 v-model
 */
const addRootCodeModel = ref({
  name : '' ,
  description: ''
});

/**
 * 언더코드 추가에서 사용하는 v-model
 */
const addUnderCodeModel = ref({
  name : '' ,
  description: ''
});



/**
 * 그리드 데이터 모델
 */
const gridData = ref([

]);

/**
 * 그리드 컬럼 모델
 */
const gridColumDef = ref([
  { field: "code"  , headerName:"코드" , flex:2 ,  editable: true},
  { field: "description", headerName:"설명" ,flex:3 ,  editable: true},
  { field: "", cellRenderer: CommonCodeGridRenderer},
]);

/**
 * 기본 그리드 컬럼 설정
 */
const defaultColDef = {
  flex: 1,
  editable: true,
  valueFormatter: (params) =>
    isEmptyPinnedCell(params) ?
      createPinnedCellPlaceholder(params) : undefined,
}


const getRowStyle = ({ node }) =>
  node.rowPinned ? { fontWeight: 'bold', color:'gray' } : {};


/**
 * 빈 탑 핀 컬럼 셀
 * @param params
 */
const isEmptyPinnedCell = (params) => {
  console.log(params.node);

  console.log('isEmptyPinnedCell',(params.node.rowPinned === 'top' && params.value == null) ||
    (params.node.rowPinned === 'top' && params.value === ''))


  return (
    (params.node.rowPinned === 'top' && params.value == null) ||
    (params.node.rowPinned === 'top' && params.value === '')
  );
}

/**
 * 빈 핀 컬럼의 컬럼명을 조정한다.
 * @param colDefined
 */
const createPinnedCellPlaceholder = (colDefined : any) => {
  console.log('createPinnedCellPlaceholder==',colDefined) ;

  return colDefined.colDef.headerName + ' 입력..';
}

/**
 * 컬럼 에디팅이 완료된경우
 * @param params
 */
const onCellEditingStopped = (params) => {
  console.log('onCellEditingStopped')

  if (isPinnedRowDataCompleted(params)) {
    console.log({ from: 'pinned row actions' });
    // save data
    gridData.value = [...gridData.value, inputRow];
    //reset pinned row
    inputRow = {};
    params.api.setPinnedTopRowData([inputRow]);
  }
}

/**
 * 데이터 입력 완료 판별
 * @param params 파라미터
 */
const isPinnedRowDataCompleted = (params) => {
  // 최상위 로우가 아닌경우
  if (params.rowPinned !== 'top'){
    return;
  }
  return gridColumDef.value.filter(i => i.field != '').every((def) => inputRow[def.field]);
}


/**
 * 루트 코드를 추가한다.
 */
const addRootCode = () => {
  // 입력이 올바르지 않은경우
  if(addRootCodeModel.value.name === '' || addRootCodeModel.value.description === '') {
    messageService.showWarning('코드명 설명 두가지의 값을 입력해주세요');
    return;
  }

  inCommunication.value = true;

  // TODO 서비스 테스트를 위한 가짜 시뮬레이션
  setTimeout(() => {
    inCommunication.value = false;
    rootCodeAddDialog.value = false;

    rootCodes.value.push({
      id: Math.floor(Math.random() * 20000000).toString()
      , title : addRootCodeModel.value.name
      , description: addRootCodeModel.value.description
    });

    messageService.showSuccess('추가 되었습니다.');
  },1000);
}

/**
 * 하위 코드를 추가한다.
 */
const addUnderCode = () => {
  // 입력이 올바르지 않은경우
  if(addUnderCodeModel.value.name === '' || addUnderCodeModel.value.description === '') {
    messageService.showWarning('코드명 설명 두가지의 값을 입력해주세요');
    return;
  }

  inCommunication.value = true;

  // TODO 서비스 테스트를 위한 가짜 시뮬레이션
  setTimeout(() => {
    inCommunication.value = false;
    underCodeAddDialog.value = false;

    gridData.value.push({
        code : addRootCodeModel.value.name
      , description: addRootCodeModel.value.description
    });

    messageService.showSuccess('추가 되었습니다.');
  },1000);
}


/**
 * 코드를 업데이트한다.
 */
const updateRootCodeInfo = () => {
  inCommunication.value = true;

  // TODO 서비스 테스트를 위한 가짜 시뮬레이션
  setTimeout(() => {
    inCommunication.value = false;
    messageService.showSuccess('수정 되었습니다.');
  },1000);
}


/**
 * 상위 로트코드를 삭제한다.
 */
const deleteRootCode = () => {
  inCommunication.value = true;
  rootCodeDeleteDialog.value = false;

  // TODO 서비스 테스트를 위한 가짜 시뮬레이션
  setTimeout(() => {
    inCommunication.value = false;

    // 아이템 제외
    rootCodes.value = rootCodes.value.filter(i => i.id !== currentRootCode.value.id).slice();

    messageService.showSuccess('삭제 되었습니다.');
  },1000);
}

/**
 * 테스트를 위한 더미 코드
 */
const rootCodes = ref([
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
]);

/**
 * 현재 선택한 코드정보
 */
let currentRootCode = ref({
  id:''
  , title : ''
  , description: ''
});

/**
 * 좌측 루트 코드 선택시
 * @param item 코드 아이템
 */
const selectedRootCode = (item:any) => {
  // 동일한 데이터인경우
  if(currentRootCode.value.id === item.id)
    return;

  currentRootCode.value = item;

  // 가짜 데이터
  if(item.id === '1'){
    gridData.value = [
      { code: "H&N", description: "H&N 부서" },
      { code: "NR", description: "NR 부서" },
    ]
  }else {
    gridData.value = [
      { code: "1000", description: "" },
    ]
  }


};
</script>

<template>
  <v-row dense>
    <v-col cols="12" class="mt-5">
      <v-sheet rounded  class="pa-5">
        <v-card elevation="0" >
          <v-card-title>
            <h4>상위코드 상세정보</h4>
          </v-card-title>
          <v-card-subtitle class="mb-5"><span>상위코드의 상세정보를 관리합니다. 생성된 코드명은 변경할 수 없습니다.</span></v-card-subtitle>
          <v-text-field  label="코드명" variant="outlined"  v-model="currentRootCode.title" disabled></v-text-field>
          <v-text-field  label="설명"  variant="outlined"  v-model="currentRootCode.description" @keyup.enter="updateRootCodeInfo()" :disabled="currentRootCode.id === '' || inCommunication"></v-text-field>
        </v-card>
      </v-sheet>
    </v-col>
  </v-row>

  <!--좌측 패널 (sheets)-->
  <v-row>
    <v-col cols="12" md="3">
      <v-sheet rounded style="min-height: 700px;" class="pa-5">
        <!--상위코드 리스트-->
        <v-card elevation="0">
          <v-card-title>
            <h4>상위코드 목록 관리 <v-btn density="compact" size="small" icon="mdi-plus" color="info" class="mb-1" @click="rootCodeAddDialog = true"></v-btn></h4>
          </v-card-title>
          <v-card-subtitle class="mb-5">상위코드를 관리합니다.</v-card-subtitle>
          <!--개별 상위코드 리스트-->
          <v-list-item  @click="selectedRootCode(item)"
                        v-for="item in rootCodes"
                        :key="item.id"
          >
            <v-sheet class="d-flex">
              <v-sheet class="">
                <h3>{{item.title}}</h3>
                {{item.description}}
              </v-sheet>
              <v-spacer></v-spacer>
              <v-sheet>
                <v-btn density="compact" size="small" icon="mdi-minus" color="red-lighten-2" class="mt-4"  @click="rootCodeDeleteDialog = !rootCodeDeleteDialog"></v-btn>
                <!--수평 중앙정렬-->
              </v-sheet>
            </v-sheet>
            <v-divider class="mt-3 mb-3"></v-divider>
          </v-list-item>
        </v-card>
      </v-sheet>
    </v-col>
    <!--우측 패널-->
    <v-col>
      <v-sheet rounded style="min-height: 700px;" class="pa-5">
        <v-card elevation="0" >
          <v-card-title><h4>하위코드 목록 관리 <v-btn density="compact" size="small" icon="mdi-plus" color="info" class="mb-1" @click="underCodeAddDialog = true"></v-btn></h4></v-card-title>
          <v-card-subtitle class="mb-5">하위 코드를 관리합니다. 생성된 코드명은 변경할 수 없습니다.</v-card-subtitle>
          <ag-grid-vue
            :rowData="gridData"
            :columnDefs="gridColumDef"
            :pinnedTopRowData="pinnedTopRowData"
            :defaultColDef="defaultColDef"
            :getRowStyle="getRowStyle"
            @cell-editing-stopped="onCellEditingStopped"
            style="height: 500px"
            class="ag-theme-quartz"
          >
          </ag-grid-vue>
        </v-card>
      </v-sheet>
    </v-col>
  </v-row>

  <!--상위코드 추가 다이얼로그-->
  <v-dialog v-model="rootCodeAddDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>상위코드 추가</h4>
      </v-card-title>
      <v-card-subtitle class="">상위코드를 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="8" class="mt-5">
          <v-text-field label="코드명" variant="outlined" v-model="addRootCodeModel.name" @keyup.enter="addRootCode()"></v-text-field>
          <v-text-field label="설명" variant="outlined" v-model="addRootCodeModel.description" @keyup.enter="addRootCode()"></v-text-field>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>

  <!--상위코드 삭제 다이얼로그-->
  <v-dialog v-model="rootCodeDeleteDialog" width="auto">
    <v-card min-width="250" title="코드 삭제" text="삭제하시겠습니까?">
      <template v-slot:actions>
        <v-btn class="ms-auto" text="확인" @click="deleteRootCode"
        ></v-btn>
      </template>
    </v-card>
  </v-dialog>

  <!--하위코드 추가 다이얼로그-->
  <v-dialog v-model="underCodeAddDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><span><h4>{{currentRootCode.title}} - 하위코드 추가</h4> </span>
      </v-card-title>
      <v-card-subtitle class=""><b>{{currentRootCode.title}}</b> 의 하위코드를 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" class="mt-5">
          <v-text-field label="코드명" variant="outlined" v-model="addUnderCodeModel.name" @keyup.enter="addUnderCode()"></v-text-field>
          <v-text-field label="설명" variant="outlined" v-model="addUnderCodeModel.description" @keyup.enter="addUnderCode()"></v-text-field>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
.v-card-subtitle span {
  white-space: normal;
  overflow-wrap: break-word;
  text-overflow: clip;
}
</style>
