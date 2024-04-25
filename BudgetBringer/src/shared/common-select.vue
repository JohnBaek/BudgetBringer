<script setup lang="ts">
import {RequestQuery} from "../models/requests/query/request-query";
import {onMounted , ref , watch} from "vue";
import {HttpService} from "../services/api-services/http-service";
import {ResponseList} from "../models/responses/response-list";
import {EnumResponseResult} from "../models/enums/enum-response-result";
import {messageService} from "../services/message-service";
/**
 * Props
 */
const props = defineProps({
  requestApiUri: { type: String , required: true ,} ,
  title: {type: String , required: true} ,
  value: {type: String ,required: true}
});
/**
 * Emits
 */
const emits = defineEmits<{
  // 신규 데이터가 추가되었을때
  (e: 'onDataUpdated', params): any,
  // 셀렉터의 선택이 변경되었을때
  (e: 'onChange', params): any,
}>();
/**
 * 쿼리 요청 정보
 */
let queryRequest: RequestQuery = new RequestQuery();
/**
 * 온마운트 핸들링
 */
onMounted(() => {
  // 최초 상태의 조회 조건을 복원한다.
  queryRequest.skip = 0;
  queryRequest.pageCount = 10000;
  queryRequest.apiUri = props.requestApiUri;
  loadData();
});
/**
 * 통신중 여부
 */
const inCommunication = ref(false);
/**
 * 통신중 여부
 */
const items = ref([]);
const data = ref(null);
/**
 * 선택이 변경되었을때
 * @param key
 */
const onModelChange = (key : any) => {
  // 업데이트된 데이터를 Notify 한다.
  emits('onChange' , key);
}
/**
 * 데이터를 로드한다.
 */
const loadData = () => {
  inCommunication.value = true;
  HttpService.requestGet<ResponseList<any>>(queryRequest.apiUri , queryRequest).subscribe({
    next(response) {
      // 로드에 실패한경우
      if(response.result !== EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // 성공한경우
      items.value = response.items;

      // 업데이트된 데이터를 Notify 한다.
      emits('onDataUpdated' , items.value.slice());
    },
    error(err) {
      console.error('Error loading data', err);
    },
    complete() {
      setTimeout(() => {
        inCommunication.value = false;
      },1000);
    },
  });
}
</script>

<template>
  <div class="relative-position" style="position: relative; width: 100%;">
    <v-select
      :items="items"
      :item-title="props.title"
      :item-value="props.value"
      :disabled="inCommunication"
      @update:modelValue="onModelChange"
      placeholder="값을 선택해주세요"
      class="select-with-loader"
      density="compact"
      variant="outlined"
    ></v-select>
    <v-progress-circular
      v-if="inCommunication"
      indeterminate
      color="grey"
      size="30"
      width="5"
      class="center-loader"
    ></v-progress-circular>
  </div>
</template>

<style scoped>
.relative-position {
  width: 100%; /* Ensure the parent container takes up full width */
}

.center-loader {
  position: absolute;
  left: 50%;
  top: 30%;
  transform: translate(-50%, -50%); /* Center the loader */
}

.select-with-loader {
  position: relative;
  width: 100%;
}
</style>
