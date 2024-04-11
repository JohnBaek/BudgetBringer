<script setup lang="ts">
import {RequestQuery} from "../models/requests/query/request-query";
import {onMounted , ref , watch} from "vue";
import {HttpService} from "../services/api-services/http-service";
import {ResponseList} from "../models/responses/response-list";
import {EnumResponseResult} from "../models/enums/enum-response-result";
import {messageService} from "../services/message-service";

/**
 * Prop 정의
 */
const props = defineProps({
  // 쿼리 정보
  requestApiUri: {
    type: String ,
    required: true ,
  } ,
  // 라벨 정보
  label: {
    type: String ,
    required: true
  } ,
  // 타이틀
  title: {
    type: String ,
    required: true
  } ,
  // 값
  value: {
    type: String ,
    required: true
  }
});

/**
 * emit 정의
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
      inCommunication.value = false;
    },
  });
}
</script>

<template>
  <v-select
    :label="props.label"
    :item-title="props.title"
    :item-value="props.value"
    :items="items"
    @update:modelValue="onModelChange"
  ></v-select>
</template>

<style scoped lang="css">

</style>
