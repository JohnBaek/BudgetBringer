<script setup lang="ts">
import {onMounted, ref} from "vue";
import {CommonCodeGridData, CommonCodeGridDataModel} from "./common-code-grid-data";
import CommonGrid from "../../shared/grids/common-grid.vue";
import {messageService} from "../../services/message-service";

/**
 * 통신중 여부
 */
const inCommunication = ref(false);

/**
 * 그리드 모델
 */
const gridModel = new CommonCodeGridData();

/**
 * Prop 정의
 */
const props = defineProps({
  /**
   * 사용자가 선택한 상위 코드 정보
   */
  currentRootCode: {
    Type: CommonCodeGridDataModel ,
    required: true
  },
});

/**
 * 사용자가 선택한 상위 코드 정보
 */
const rootCode = ref(new CommonCodeGridDataModel());

/**
 * 언더코드 추가에서 사용하는 v-model
 */
const addUnderCodeModel = ref(new CommonCodeGridDataModel());


/**
 * 신규 행이 추가되었을때
 * @param params 파라미터
 */
const onNewRowAdded = (params) => {
  console.log('onNewRowAdded',params);
}

/**
 * 추가 버튼 클릭시
 */
const onAdd = () => {
  const currentRootCode = props.currentRootCode as CommonCodeGridDataModel;

  rootCode.value.id = currentRootCode.id;
  rootCode.value.code = currentRootCode.code;
  rootCode.value.description = currentRootCode.description;

  console.log('rootCode' , rootCode);

  underCodeAddDialog.value = true;
}


/**
 * 하위코드 추가 다이얼로그
 */
const underCodeAddDialog = ref(false);

/**
 * 그리드 데이터
 */
const items = ref(gridModel.items);


/**
 * 하위 코드를 추가한다.
 */
const addUnderCode = () => {
  inCommunication.value = true;

  // TODO 서비스 테스트를 위한 가짜 시뮬레이션
  setTimeout(() => {
    inCommunication.value = false;
    underCodeAddDialog.value = false;

    // 그리드에 데이터 추가
    const add = new CommonCodeGridDataModel();
    add.code = addUnderCodeModel.value.code;
    add.description = addUnderCodeModel.value.description;
    gridModel.items.push(add);

    messageService.showSuccess('추가 되었습니다.');
  },1000);
}
</script>

<template>
  <common-grid
    :is-use-insert="gridModel.isUseInsert"
    :input-colum-defined="gridModel.columDefined"
    :input-row-data="items"
    :is-use-buttons="true"
    height="480px"
    @onNewRowAdded="onNewRowAdded"
    @onAdd="onAdd"
  />

  <!--하위코드 추가 다이얼로그-->
  <v-dialog v-model="underCodeAddDialog" width="auto">
    <v-card elevation="1" rounded class="mb-10 pa-5">
      <v-card-title class=" mt-5"><span><h4>{{ rootCode.code }} - 하위코드 추가</h4> </span>
      </v-card-title>
      <v-card-subtitle class=""><b>{{ rootCode.code }}</b> 의 하위코드를 추가합니다 생성된 코드명은 변경할 수 없습니다. 엔터키를 누르면 등록됩니다.<br>취소를 원하시는 경우 ESC 키를 눌러주세요</v-card-subtitle>
      <v-row dense>
        <v-col cols="12" class="mt-5">
          <v-text-field label="코드명" variant="outlined" v-model="addUnderCodeModel.code" @keyup.enter="addUnderCode()"></v-text-field>
          <v-text-field label="설명" variant="outlined" v-model="addUnderCodeModel.description" @keyup.enter="addUnderCode()"></v-text-field>
        </v-col>
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
</style>
