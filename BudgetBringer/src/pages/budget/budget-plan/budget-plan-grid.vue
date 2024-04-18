<script setup lang="ts">
import {onMounted, ref} from "vue";
import {BudgetPlanGridData} from "./budget-plan-grid-data";
import CommonGrid from "../../../shared/grids/common-grid.vue";
import {RequestQuery} from "../../../models/requests/query/request-query";
import {RequestBudgetPlan} from "../../../models/requests/budgets/request-budget-plan";
import CommonSelect from "../../../shared/common-select.vue";
import {messageService} from "../../../services/message-service";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {EnumResponseResult} from "../../../models/enums/enum-response-result";
import {ResponseData} from "../../../models/responses/response-data";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";
import {firstValueFrom} from "rxjs";

/**
 * 그리드 모델
 */
const gridModel = new BudgetPlanGridData();
/**
 * 쿼리 정보
 */
let requestQuery :RequestQuery = {
  apiUri : '/api/v1/BudgetPlan' ,
  pageCount: 100 ,
  skip: 0 ,
  searchFields: [] ,
  searchKeywords: [],
  sortFields: [ 'regDate' ],
  sortOrders: [ 'desc' ],
}
/**
 * props 정의
 */
const props = defineProps({
  /**
   * isAbove500k 정보
   */
  isAbove500k: {
    Type: String,
    required: true,
  },
  /**
   * title 정보
   */
  title: {
    Type: String,
    required: true,
  },
});
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
 * 그리드 래퍼런스
 */
const gridReference = ref(null);
/**
 * 데이터 추가 원본 요청 데이터
 */
const modelReference = ref<RequestBudgetPlan>(new RequestBudgetPlan());
/**
 * 컨트리 비지니스 매니저 리스트
 */
let countryBusinessManagers : ResponseCountryBusinessManager [] = [];
/**
 * 비지니스 유닛 리스트
 */
let businessUnitsReference = ref([]);
/**
 * 삭제할 데이터
 */
let removeItems : Array<ResponseBudgetPlan> = [];
/**
 * 수정할 데이터
 */
let updateItem: ResponseBudgetPlan;

/**
 * 마운트핸들링
 */
onMounted(() => {
  requestQuery.searchKeywords.push(props.isAbove500k.toString());
  requestQuery.searchFields.push("isAbove500k");
});

/**
 * countryBusinessManager 가 변경 되었을때
 * @param countryBusinessManagerId
 */
const onChangeCountryBusinessManager = (countryBusinessManagerId: any) => {

  console.log('onChangeCountryBusinessManager',countryBusinessManagerId);

  // 선택된 값 초기화
  modelReference.value.businessUnitId = "";

  // 대상 CBM 을 찾는다.
  const _countryBusinessManagers = countryBusinessManagers.filter(i => i.id === countryBusinessManagerId);

  // 비지니스 유닛을 초기화한다.
  businessUnitsReference.value = [];

  // 하나이상의 CBM 을 찾은경우
  if(_countryBusinessManagers.length > 0)
    // 비지니스 유닛을 업데이트한다.
    businessUnitsReference.value = _countryBusinessManagers[0].businessUnits;
}

/**
 * CBM 데이터가 업데이트 되었을때
 * @param items 이벤트 객체
 */
const onDataUpdatedCBM = (items: any) => {
  countryBusinessManagers = items;
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
  communicationService.inCommunication();

  // 데이터를 입력한다.
  HttpService.requestPost<ResponseData<ResponseBudgetPlan>>(requestQuery.apiUri , modelReference.value).subscribe({
    next(response) {

      // 요청에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      messageService.showSuccess(`데이터가 등록되었습니다.`);
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
      communicationService.offCommunication();
    } ,
    complete() {
      // 다이얼로그를 닫는다.
      addDialogReference.value = false;

      // 데이터를 다시 로드한다.
      gridReference.value.doRefresh();

      // 커뮤니케이션을 종료한다.
      communicationService.offCommunication();
    },
  });
}

/**
 * 유효성 여부를 검증한다.
 */
const isValidModel = () => {
  if(modelReference.value.approvalDate === ''
  || modelReference.value.sectorId === ''
  || modelReference.value.businessUnitId === ''
  || modelReference.value.costCenterId === ''
  || modelReference.value.countryBusinessManagerId === '') {
    return false;
  }
}

/**
 * 데이터 팝업을 요청한다.
 * @param items 삭제할 데이터
 */
const showRemoveDialog = (items : Array<ResponseBudgetPlan>) => {
  removeDialogReference.value = true;

  // 삭제할 데이터를 보관
  removeItems = items;
}

/**
 * 추가 팝업을 요청한다.
 */
const showAddDialog = () => {
  addDialogReference.value = true;
  modelReference.value = new RequestBudgetPlan();
  modelReference.value.isAbove500K = (props.isAbove500k as String).toLowerCase() == "true";
}

/**
 * 데이터 수정 팝업을 요청한다.
 * @param item 수정할 데이터
 */
const showUpdateDialog = (item: ResponseBudgetPlan) => {
  communicationService.inCommunication();
  updateItem = item;

  // 서버에서 대상하는 데이터를 조회한다.
  HttpService.requestGet<ResponseData<ResponseBudgetPlan>>(`${requestQuery.apiUri}/${item.id}`).subscribe({
    async next(response) {
      // 요청에 실패한경우
      if (response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // 대상 비지니스 매니저를 요청한다.
      const _responseCountryBusinessManager = await firstValueFrom(
        HttpService.requestGet(`/api/v1/CountryBusinessManager/${item.countryBusinessManagerId}`)
      ) as ResponseData<ResponseCountryBusinessManager> ;

      // 비지니스 유닛을 업데이트
      businessUnitsReference.value = _responseCountryBusinessManager.data.businessUnits;

      // 데이터를 업데이트한다.
      modelReference.value = Object.assign(modelReference.value, item);

      // 팝업을 연다.
      updateDialogReference.value = true;
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
    } ,
    complete() {
      communicationService.offCommunication();
    },
  });
}

/**
 * 데이터를 삭제한다.
 */
const requestRemoveData = () => {
  // 모든 데이터에 대해 처리
  for (const data of removeItems) {
    communicationService.inCommunication();
    HttpService.requestDelete<ResponseData<any>>(`${requestQuery.apiUri}/${data.id}`).subscribe({
      next(response) {
        // 요청에 실패한경우
        if(response.result !== EnumResponseResult.success) {
          messageService.showError(`[${response.code}] ${response.message}`);
          return;
        }
        messageService.showSuccess(`데이터가 삭제되었습니다.`);
      } ,
      error(err) {
        messageService.showError('Error loading data'+err);
      } ,
      complete() {
        removeDialogReference.value = false;
        gridReference.value.doRefresh();
        communicationService.offCommunication();
      },
    });
  }
}

/**
 * 데이터를 수정한다.
 */
const requestUpdateData = () => {
  // 유효하지 않은경우
  if(isValidModel() == false) {
    messageService.showWarning("입력하지 않은 데이터가 있습니다");
    return;
  }


  communicationService.inCommunication();
  HttpService.requestPut<ResponseData<any>>(`${requestQuery.apiUri}/${updateItem.id}`, modelReference.value).subscribe({
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
               :query-request="requestQuery"
               :grid-title="((props.isAbove500k as String).toLowerCase() == 'true') ? '예산계획_Above_500K_Budget' : '예산계획_Below_500K_Budget'"
               @onAdd="showAddDialog"
               @onRemove="showRemoveDialog"
               @onUpdate="showUpdateDialog"
               ref="gridReference"
  />
  <!--데이터 추가 다이얼로그-->
  <v-dialog v-model="addDialogReference" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>{{ props.title }} 예산추가</h4>
      </v-card-title>
      <v-card-subtitle class="">예산을 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <common-select required v-model="modelReference.countryBusinessManagerId" @onChange="onChangeCountryBusinessManager" @onDataUpdated="onDataUpdatedCBM" title="name" value="id" label="Country Business Manager" requestApiUri="/api/v1/CountryBusinessManager" />
          <v-select required v-model="modelReference.businessUnitId" label="Business Unit" item-title="name" item-value="id" :items="businessUnitsReference" :disabled="businessUnitsReference.length === 0"></v-select>
          <common-select required v-model="modelReference.costCenterId" title="value" value="id" label="Cost Center" requestApiUri="/api/v1/CostCenter" />
          <common-select required v-model="modelReference.sectorId" title="value" value="id" label="Sector" requestApiUri="/api/v1/Sector" />
          <v-text-field required v-model="modelReference.approvalDate" label="Approval Date" variant="outlined" @keyup.enter="requestAddData()"></v-text-field>
          <v-text-field v-model="modelReference.description" label="Description" variant="outlined" @keyup.enter="requestAddData()"></v-text-field>
          <v-text-field v-model="modelReference.budgetTotal" label="BudgetTotal" variant="outlined" @keyup.enter="requestAddData()"></v-text-field>
          <v-text-field v-model="modelReference.ocProjectName" label="OcProjectName" variant="outlined" @keyup.enter="requestAddData()"></v-text-field>
          <v-text-field v-model="modelReference.bossLineDescription" label="BossLine Description" variant="outlined" @keyup.enter="requestAddData()"></v-text-field>
          <v-btn variant="outlined" @click="requestAddData()" class="mr-2" color="info" >추가</v-btn>
          <v-btn variant="outlined" @click="addDialogReference = false" class="mr-2" color="error">취소</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>

  <!--데이터 수정 다이얼로그-->
  <v-dialog v-model="updateDialogReference" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><h4>{{ props.title }} 예산수정</h4>
      </v-card-title>
      <v-card-subtitle class="">예산을 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" md="12" class="mt-5">
          <common-select required v-model="modelReference.countryBusinessManagerId" @onChange="onChangeCountryBusinessManager" @onDataUpdated="onDataUpdatedCBM" title="name" value="id" label="Country Business Manager" requestApiUri="/api/v1/CountryBusinessManager" />
          <v-select required v-model="modelReference.businessUnitId" label="Business Unit" item-title="name" item-value="id" :items="businessUnitsReference" :disabled="businessUnitsReference.length === 0"></v-select>
          <common-select required v-model="modelReference.costCenterId" title="value" value="id" label="Cost Center" requestApiUri="/api/v1/CostCenter" />
          <common-select required v-model="modelReference.sectorId" title="value" value="id" label="Sector" requestApiUri="/api/v1/Sector" />
          <v-text-field required v-model="modelReference.approvalDate" label="Approval Date" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-text-field v-model="modelReference.description" label="Description" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-text-field v-model="modelReference.budgetTotal" label="BudgetTotal" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-text-field v-model="modelReference.ocProjectName" label="OcProjectName" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-text-field v-model="modelReference.bossLineDescription" label="BossLine Description" variant="outlined" @keyup.enter="requestUpdateData()"></v-text-field>
          <v-btn variant="outlined" @click="requestUpdateData()" class="mr-2" color="info" >수정</v-btn>
          <v-btn variant="outlined" @click="updateDialogReference = false" class="mr-2" color="error">취소</v-btn>
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
