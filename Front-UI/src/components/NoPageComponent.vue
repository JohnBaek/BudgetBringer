<template>
  <v-app>
    <v-main>
      <v-container>
        <v-row justify="center" align="center" style="min-height: 100vh;">
          <v-col cols="12" sm="8" md="6" lg="4">
            <!-- 타이틀과 설명 -->
            <div class="text-center mb-5">
              <v-icon
                icon="mdi-note-off"
                end
              ></v-icon>
              <h1 class="display-1 mb-1">Page Not Found</h1>
              <span class="subtitle-1 text-grey">존재하지않는 페이지입니다.<br />{{remainingTime}} 초후 로그인화면으로 이동합니다.</span>
            </div>
            <v-btn
              large
              block
              color="grey"
              :height="50"
              @click="$router.push('/login')"
            >
              <b>로그인화면으로 이동</b>
            </v-btn>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>
<style scoped>
</style>
<script setup lang="ts">
import {ref, onMounted, onBeforeUnmount, onUnmounted} from 'vue';
import { useRouter } from 'vue-router';

// 남은시간
const remainingTime = ref(10);

// 라우터
const router = useRouter();

// 타이머 시작
const startTimer = () => {
  const countdownInterval = setInterval(() => {
    if (remainingTime.value > 0) {
      remainingTime.value -= 1;
    } else {
      clearInterval(countdownInterval);
      router.push('/login');
    }
  }, 1000);
};

// 타이머 보관용
let timer : any;

// 마운트시
onMounted(() => {
  timer = startTimer();
});

// 언 마운트시
onUnmounted(() => {
  clearInterval(timer);
});
</script>

<style scoped>
</style>
