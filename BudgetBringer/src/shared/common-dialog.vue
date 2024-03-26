<script setup lang="ts">
import {ref} from "vue";

/**
 * emit 정의
 */
const emits = defineEmits<{
  // 확인 버튼 클릭시
  (e: 'confirm' ): boolean
}>();

/**
 * 확인 버튼 클릭
 */
const confirm = () => {
  emits('confirm');
}

/**
 * 다이얼로그
 */
const dialog = ref(false);
</script>

<!--공통 컨펌 다이얼로그-->
<template>
  <v-dialog v-model="dialog">
    <v-container>
      <slot></slot>
      <v-spacer></v-spacer>
      <div>
        <v-btn variant="outlined" @click="confirm()" class="mr-2" color="info">확인</v-btn>
        <v-btn variant="outlined" @click="dialog = false" class="mr-2" color="error">취소</v-btn>
      </div>
    </v-container>
  </v-dialog>
</template>

<style scoped lang="css">
</style>
