<script setup lang="ts">
import CommonLogo from "../shared/common-logo.vue";
import {onMounted, onUnmounted, ref} from "vue";
import {useRouter} from "vue-router";

/**
 * 남은시간
 */
const remainingTime = ref(10);

/**
 * 라우터
 */
const router = useRouter();

/**
 * 타이머 시작
 */
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

/**
 * onMounted 핸들링
 */
onMounted(() => {
  timer = startTimer();
});

/**
 * onUnmounted 핸들링
 */
onUnmounted(() => {
  clearInterval(timer);
});
</script>

<template>
  <v-row justify="center" align="center" style="min-height: 100vh;">
    <v-col cols="12" sm="8" md="6" lg="4">
      <v-container class="d-flex flex-column align-center justify-center">
        <common-logo/>
        <h3>Page Not Found!</h3>
        <span class="text-grey mt-3 text-center">존재하지 않는 페이지입니다.<br>{{remainingTime}} 초 후 로그인 화면으로 이동합니다.
        </span>

        <v-btn class="mt-5 " block color="grey" :height="50" @click="router.push('/login')">
          <b>로그인 화면으로 이동</b>
        </v-btn>
      </v-container>
    </v-col>
  </v-row>
</template>

<style scoped lang="css">

</style>
