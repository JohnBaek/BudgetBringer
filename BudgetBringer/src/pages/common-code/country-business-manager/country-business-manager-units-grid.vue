<script setup lang="ts">
import {onMounted, ref ,watch} from "vue";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {messageService} from "../../../services/message-service";
import {AgGridVue} from "ag-grid-vue3";
import {ResponseData} from "../../../models/responses/response-data";
import {communicationService} from "../../../services/communication-service";
import {
  ResponseProcessBusinessUnitSummary
} from "../../../models/responses/process/process-business-unit/response-process-business-unit-summary";
import {BusinessUnitGridData} from "../business-unit/business-unit-grid-data";
import {ResponseBusinessUnit} from "../../../models/responses/budgets/response-business-unit";
import {RequestBusinessUnit} from "../../../models/requests/budgets/request-business-unit";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import BusinessUnit from "../business-unit/business-unit.vue";
import {ResponseList} from "../../../models/responses/response-list";

/**
 * Prop 정의
 */
const props = defineProps({
  /**
   * 타이틀 정보
   */
  title : {
    type: String ,
    required: false
  } ,
  countryBusinessManager : {
    type: ResponseCountryBusinessManager ,
    required: false
  } ,
});
/**
 * 그리드 모델
 */
const gridModel = new BusinessUnitGridData();
/**
 * 쿼리 정보
 */
const requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetProcess' ,
  pageCount: 10000 ,
  skip: 0 ,
  searchFields: [] ,
  searchKeywords: [],
  sortFields: [],
  sortOrders: [],
}
/**
 * 데이터 추가 다이얼로그
 */
const addDialogReference = ref(false);
/**
 * 삭제 다이얼로그
 */
const removeDialogReference = ref(false);
/**
 *
 */
const updateDialogReference = ref(false);

/**
 * 마운트 핸들링
 */
onMounted(() => {
  gridModel.columDefined = [
    // 승인일
    {
      field: "name",
      headerClass: 'ag-grids-custom-header',
      headerName:"Value" ,
      showDisabledCheckboxes: true,
      filter: 'agTextColumnFilter',
      floatingFilter: true,
      width:250,
    },
  ];

  loadData();
});

const columDefined = [
  // 승인일
  {
    field: "name",
    headerClass: 'ag-grids-custom-header',
    headerName:"BusinessUnit" ,
    showDisabledCheckboxes: true,
    filter: 'agTextColumnFilter',
    floatingFilter: true,
    width:250,
  },
]


const countryBusinessManagerRef = ref(new ResponseCountryBusinessManager());
const businessUnits = ref([]);
const targetBusinessUnits = ref([]);
const loadData = () => {
  /**
   * 쿼리 정보
   */
  const requestQuery :RequestQuery = {
    apiUri : '/api/v1/BusinessUnit' ,
    pageCount: 100 ,
    skip: 0 ,
    searchFields: [] ,
    searchKeywords: [],
    sortFields: [ 'regDate' ],
    sortOrders: [ 'desc' ],
  }
  communicationService.inCommunication();

  // 데이터를 입력한다.
  HttpService.requestGet<ResponseList<ResponseBusinessUnit>>(requestQuery.apiUri).subscribe({
    next(response) {

      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      businessUnits.value = response.items;
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
      communicationService.offCommunication();
    } ,
    complete() {
      // 커뮤니케이션을 종료한다.
      communicationService.offCommunication();
    },
  });
}

/**
 * businessUnits 의 변경감지
 */
watch(() => props.countryBusinessManager, (countryBusinessManager) => {
  countryBusinessManagerRef.value = countryBusinessManager as ResponseCountryBusinessManager;

  const ids = countryBusinessManagerRef.value.businessUnits.map(i => i.id);
  selectedBusinessUnit.value = null;
  targetBusinessUnits.value = businessUnits.value.filter(businessUnit => !ids.includes(businessUnit.id)).slice();

}, {
  deep: true  // 객체 내부 또는 배열의 변화까지 감지하기 위해 deep 옵션 사용
});


const add = () => {
  addDialogReference.value = true;
  modelReference.value = new RequestBusinessUnit();
}

const remove = () => {
  removeDialogReference.value = true;
}

const update = () => {
}

const selectedRows = ref([]);

/**
 * 그리드의 셀렉트가 변경되었을때
 */
const onSelectionChanged = () => {
  // 선택된 Row 를 업데이트한다.
  selectedRows.value = gridApi.value.getSelectedRows();
};

/**
 * 데이터 추가 원본 요청 데이터
 */
const modelReference = ref<RequestBusinessUnit>(new RequestBusinessUnit());

/**
 * Grid API
 */
const gridApi = ref();

/**
 * gridReady 이벤트 핸들러
 * @param params 파라미터
 */
const onGridReady = (params) => {
  gridApi.value = params.api;
};


/**
 * 유효성 여부를 검증한다.
 */
const isValidModel = () => {
  if(selectedBusinessUnit.value === null){
    return false;
  }
  return true;
}

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
  // communicationService.inCommunication();
  const requestUri = `/api/v1/CountryBusinessManager/${countryBusinessManagerRef.value.id}/BusinessUnit/${selectedBusinessUnit.value}`;
  communicationService.inCommunication();
  // 데이터를 입력한다.
  HttpService.requestPost<ResponseData<ResponseBusinessUnit>>(requestUri,{}).subscribe({
    next(response) {

      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      messageService.showSuccess(`데이터가 등록되었습니다.`);
      countryBusinessManagerRef.value.businessUnits = (response.data as ResponseCountryBusinessManager).businessUnits;
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
      communicationService.offCommunication();
    } ,
    complete() {
      // 다이얼로그를 닫는다.
      addDialogReference.value = false;
      // 커뮤니케이션을 종료한다.
      communicationService.offCommunication();
    },
  });
}

/**
 * 데이터를 삭제한다.
 */
const requestRemoveData = () => {
  console.log('selectedRows.value',selectedRows.value);

  // 유효한 데이터가 아닌경우
  if(selectedRows.value.length == 0) {
    messageService.showWarning("입력하지 않은 데이터가 있습니다");
    return;
  }

  // 모든 삭제할 데이터에 대해 처리한다.
  selectedRows.value.forEach(i => {
    // 커뮤니케이션 시작
    communicationService.inCommunication();

    const requestUri = `/api/v1/CountryBusinessManager/${countryBusinessManagerRef.value.id}/BusinessUnit/${i.id}`;
    HttpService.requestDelete<ResponseData<ResponseData<any>>>(requestUri,{}).subscribe({
      next(response) {

        // 요청에 실패한경우
        if(response.result !== EnumResponseResult.success) {
          messageService.showError(`[${response.code}] ${response.message}`);
          return;
        }
        messageService.showSuccess(`데이터가 삭제되었습니다.`);
        countryBusinessManagerRef.value.businessUnits = countryBusinessManagerRef.value.businessUnits.filter(v => v.id != i.id);
      } ,
      error(err) {
        messageService.showError('Error loading data'+err);
        communicationService.offCommunication();
      } ,
      complete() {
        // 다이얼로그를 닫는다.
        removeDialogReference.value = false;
        // 커뮤니케이션을 종료한다.
        communicationService.offCommunication();
      },
    });
  });

  // // 데이터를 입력한다.
  // HttpService.requestPost<ResponseData<ResponseBusinessUnit>>(requestUri,{}).subscribe({
  //   next(response) {
  //
  //     // 요청에 실패한경우
  //     if(response.result !== EnumResponseResult.success) {
  //       messageService.showError(`[${response.code}] ${response.message}`);
  //       return;
  //     }
  //     messageService.showSuccess(`데이터가 등록되었습니다.`);
  //     countryBusinessManagerRef.value.businessUnits = (response.data as ResponseCountryBusinessManager).businessUnits;
  //   } ,
  //   error(err) {
  //     messageService.showError('Error loading data'+err);
  //     communicationService.offCommunication();
  //   } ,
  //   complete() {
  //     // 다이얼로그를 닫는다.
  //     addDialogReference.value = false;
  //     // 커뮤니케이션을 종료한다.
  //     communicationService.offCommunication();
  //   },
  // });
}

const selectedBusinessUnit = ref();


</script>

<template>
  <v-row class="mt-1 mb-1">
    <v-col>
      <div class="mt-2">
        <v-btn  variant="outlined" @click="add()" class="mr-2" color="info">추가</v-btn>
        <v-btn  variant="outlined" @click="remove()" class="mr-2" color="error" :disabled="selectedRows.length == 0">삭제</v-btn>
        <v-spacer class="mt-1"></v-spacer>
        <span class="text-grey">shift 버튼을 누른채로 클릭하면 여러 행을 선택할수 있습니다.</span>
      </div>
    </v-col>
  </v-row>

  <ag-grid-vue
    style="width: 100%; height: 600px;"
    :columnDefs="columDefined"
    :rowData="countryBusinessManagerRef.businessUnits"
    class="ag-theme-alpine"
    @selection-changed="onSelectionChanged"
    @grid-ready="onGridReady"
    overlayNoRowsTemplate="Country Business Manager 를 선택해주세요"
    :rowSelection="'multiple'"
  >
  </ag-grid-vue>

  <!--데이터 추가 다이얼로그-->
  <v-dialog v-model="addDialogReference" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>BusinessUnit 추가</h4>
      </v-card-title>
      <v-card-subtitle class="">BusinessUnit 을 추가합니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <v-select
            label="BusinessUnit"
            item-title="name"
            item-value="id"
            :items="targetBusinessUnits"
            v-model="selectedBusinessUnit"
          ></v-select>
          <v-btn variant="outlined" @click="requestAddData()" class="mr-2" color="info" >추가</v-btn>
          <v-btn variant="outlined" @click="addDialogReference = false" class="mr-2" color="error">취소</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>

  <!--삭제 다이얼로그-->
  <v-dialog v-model="removeDialogReference" width="auto">
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
