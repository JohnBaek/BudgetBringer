<script lang="ts" setup>
import CommonMessageTemplate from "./shared/common-message-template.vue";
import CommonLogo from "./shared/common-logo.vue";
import {onMounted, ref} from "vue";
import LoginLogo from "./pages/login/login-logo.vue";

/**
 * 스플래쉬 진행중 여부
 */
let onSplash  = ref<boolean>(true);

/**
 * 스플래쉬 타이머
 */
const splashTimer = () => {
  const countdownInterval = setTimeout(() => {
    onSplash.value = false;
  }, 1000 * 3);
};

/**
 * 타이머 캐시 보관용
 */
let splashTimerCache : any;

/**
 * 마운트 핸들링
 */
onMounted(() => {
  splashTimerCache = splashTimer();
});
</script>

<template>
  <v-app>
    <!--메인 레이아웃-->
    <v-row
      v-if="onSplash"
      align="center"
      justify="center"
      style="min-height: 100vh;border:1px solid red"
    >
      <v-col>
        <!--로고-->
        <LoginLogo />
      </v-col>
    </v-row>

    <v-main style="background: #f7f8fa">
      <!--메인 라우트 View 포인트-->
      <router-view v-if="!onSplash"/>
    </v-main>
  </v-app>

  <!--메세지 템플릿-->
  <CommonMessageTemplate></CommonMessageTemplate>
</template>

<style>
  html, body {
    font-family: 'Roboto', sans-serif;
  }
  ul {
    list-style-type: none;
  }
</style>
