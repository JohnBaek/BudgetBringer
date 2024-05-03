<script setup lang="ts">
import {ref} from "vue";
import {communicationService} from "../services/communication-service";

/**
 * outgoing
 */
const emits = defineEmits<{
  (e: 'cancel') : any,
  (e: 'submit') : any,
}>();
/**
 * Subscribe
 */
const inCommunication = ref(false);
communicationService.subscribeCommunication().subscribe((communication) =>{
  inCommunication.value = communication;
});

</script>

<!--공통 컨펌 다이얼로그-->
<template>
  <v-dialog width="900" class="responsive-dialog">
    <v-card elevation="1" rounded>
      <!-- Header Area -->
      <v-card-title class="mt-5">
          <slot name="header-area">
          </slot>
      </v-card-title>

      <v-divider class="mt-5"></v-divider>
      <v-container class="pa-10">
        <!-- Contents Area -->
        <slot name="contents-area"></slot>
      </v-container>
      <v-divider ></v-divider>

      <!--Default Buttons-->
      <v-container >
        <v-row>
          <v-col class="right-align">
            <!--Cancel-->
            <v-btn width="100" elevation="1" class="mr-2" @click="emits('cancel')">
              <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
              <template v-if="!inCommunication">
                <pre class="text-grey"><b> 취소 </b></pre>
              </template>
            </v-btn>

            <!--Submit-->
            <v-btn :disabled="inCommunication" width="100" elevation="1" color="primary" @click="emits('submit')">
              <v-progress-circular size="small" indeterminate v-if="inCommunication"></v-progress-circular>
              <template v-if="!inCommunication">
                <v-icon>mdi-checkbox-marked-circle</v-icon>
                <pre><b> 확인 </b></pre>
              </template>
            </v-btn>
          </v-col>
        </v-row>
      </v-container>

    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">
/* 기본 스타일 */
.responsive-dialog .v-dialog {
  width: 90%; /* 모바일 기기에 적합한 기본 너비 */
}

/* 태블릿과 데스크톱에 적용될 미디어 쿼리 */
@media (min-width: 600px) {
  .responsive-dialog .v-dialog {
    width: 75%; /* 태블릿 크기에 맞는 너비 */
  }
}

@media (min-width: 900px) {
  .responsive-dialog .v-dialog {
    width: 50%; /* 데스크톱 크기에 맞는 너비 */
  }
}

@media (min-width: 1200px) {
  .responsive-dialog .v-dialog {
    width: 30%; /* 큰 데스크톱 화면에 맞는 너비 */
  }
}
.right-align {
  display: flex;
  justify-content: flex-end;
}


</style>
