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
  modelValue : {} ,
  requestApiUri: { type: String , required: true ,} ,
  title: {type: String , required: true} ,
  value: {type: String ,required: true} ,
  label: {type: String , required: true}
});
const selectedValue = ref(null);
/**
 * Emits
 */
const emits = defineEmits<{
  // 신규 데이터가 추가되었을때
  (e: 'onDataUpdated', params): any,
  // 셀렉터의 선택이 변경되었을때
  (e: 'onChange', params): any,
  (e: 'update:modelValue', params): any,
}>();
/**
 * 쿼리 요청 정보
 */
let queryRequest: RequestQuery = new RequestQuery(props.requestApiUri,0,10000);
/**
 * 온마운트 핸들링
 */
onMounted(() => {
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
/**
 * 선택이 변경되었을때
 * @param key
 */
const onModelChange = (key : any) => {
  // 업데이트된 데이터를 Notify 한다.
  emits('onChange' , key);
  emits('update:modelValue', key);
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
      selectedValue.value = props.modelValue;

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
watch(selectedValue, (newValue) => {
  emits('update:modelValue', newValue); // 선택된 값이 변경되면 부모 컴포넌트에 알림
});
</script>

<template>
  <div class="relative-position" style="position: relative; width: 100%;">
    <v-select
      :items="items"
      :item-title="props.title"
      :item-value="props.value"
      :disabled="inCommunication"
      v-model="selectedValue"
      @update:modelValue="onModelChange"
      :placeholder="inCommunication ? '' : '값을 선택해주세요'"
      class="select-with-loader"
      density="compact"
      variant="outlined"
      :label="props.label"
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
