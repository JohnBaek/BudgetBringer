
<template>
  <div>
    <v-btn icon="mdi-minus" size="sm" color="red-lighten-2" @click="deleteItem"></v-btn>
  </div>

  <v-dialog v-model="dialog" width="auto">
    <v-card min-width="250" title="하위 코드 삭제" text="삭제하시겠습니까?">
      <template v-slot:actions>
        <v-btn class="ms-auto"
                text="확인"
                @click="dialog = false"
        >
        </v-btn>
      </template>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import {ref} from "vue";

/**
 * 다이얼로그 표시 여부
 */
const dialog =  ref(false);

/**
 * 아이템 삭제
 */
const deleteItem = () => {
  dialog.value = true;
}
</script>

<style scoped>
</style>
