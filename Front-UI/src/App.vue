<template>
  <v-app>
    <v-main>
      <v-container v-if="isProcess">
        <v-row justify="center" align="center" style="min-height: 100vh;">
          <v-col cols="12" sm="8" md="6" lg="4">
            <!-- 타이틀과 설명 -->
            <div class="text-center mb-5">
              <h3 class="display-1 mb-1">
                <v-icon
                  icon="mdi-checkbox-marked-circle"
                  end
                  size="sm"
                ></v-icon>
                Budget Bringer</h3>
            </div>
          </v-col>
        </v-row>
      </v-container>
      <!--프로세스가 끝난후-->
      <router-view v-if="!isProcess"></router-view>
      <v-snackbar
        v-for="message in messageQueue"
        :key="message.id"
        :color="message.type"
        top
        :style="{ right: '50%', transform: 'translateX(50%)' }"
        right
        location="top"
        v-model="message.visible"
        @click="closeSnackBar(message)"
      >
        <!-- 메시지를 왼쪽에 표시하고, 'X' 버튼을 오른쪽에 정렬합니다. -->
        <div class="flex justify-space-between">
          <span><h3>{{ message.content }}</h3></span>
        </div>
      </v-snackbar>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
// 최초 프로세스 여부
import {create} from "node:domain";
import {onMounted, ref} from "vue";
import {MessageInformation, messageQueue, messageService} from "./services/MessageService";
let isProcess  = ref<boolean>(false);

// 타이머 시작
const startTimer = () => {
  const countdownInterval = setTimeout(() => {
      isProcess.value = false;
  }, 1000 * 4);
};


const closeSnackBar = (messageInformation:MessageInformation) => {
  messageInformation.visible = false;
}

// 타이머 보관용
let timer : any;

// 마운트시
onMounted(() => {
  timer = startTimer();
});

</script>
