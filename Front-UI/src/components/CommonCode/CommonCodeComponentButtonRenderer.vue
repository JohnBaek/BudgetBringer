
<template>
  <div>
    <v-btn icon="mdi-minus" color="black" size="sm" @click="deleteItem"></v-btn>
  </div>

  <v-dialog
    v-model="dialog"
    width="auto"
  >
    <v-card
      min-width="250"
      title="하위 코드 삭제"
      text="삭제하시겠습니까?"
    >
      <template v-slot:actions>
        <v-btn
          class="ms-auto"
          text="확인"
          @click="dialog = false"
        ></v-btn>
      </template>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import {ref} from "vue";

const dialog =  ref(false);
const deleteItem = () => {
  dialog.value = true;
}
</script>

<style scoped>
</style>
