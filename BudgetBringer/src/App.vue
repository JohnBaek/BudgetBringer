

<script lang="ts" setup>
import CommonMessageTemplate from "./shared/common-message-template.vue";
import CommonLogo from "./shared/common-logo.vue";
import {onMounted, onUnmounted, ref} from "vue";
import LoginLogo from "./pages/login/login-logo.vue";
import {communicationService} from "./services/communication-service";

/**
 * 스플래쉬 진행중 여부
 */
let onSplash  = ref<boolean>(true);


/**
 * 스플래쉬 타이머
 */
const splashTimer = () =>
  {
    setTimeout(() => {
      onSplash.value = false;
    }, 1000 * 3);
  }

;

/**
 * 타이머 캐시 보관용
 */
let splashTimerCache : any;

/**
 * 통신중 여부
 */
let inCommunication = ref(false);

/**
 * 통신중 여부
 */
let inTransmission = ref(false);

/**
 * 구독중인 정보
 */
let subscribes = [];

/**
 * 마운트 핸들링
 */
onMounted(() => {
  // 스플래쉬 이벤트
  splashTimerCache = splashTimer();

  // 구독을 처리한다.
  subscribes.push(
    // 통신중 여부 구독
    communicationService.communicationSubject.subscribe(communication => {
      // 유효한 데이터가 아닌경우
      if(communication == null)
        return;

      inCommunication.value = communication;
    }),
    communicationService.transmissionSubject.subscribe(transmission => {
      // 유효한 데이터가 아닌경우
      if(transmission == null)
        return;

      inTransmission.value = transmission;
    })
  );
});

const fileUploadDialog = ref(true);

/**
 * 언마운트시
 */
onUnmounted(() => {
  // 스플래쉬 캐시가 존재하는 경우
  if(splashTimerCache)
    clearTimeout(splashTimerCache);

  // 모든 구독정보를 정리한다.
  subscribes.forEach(subs => {
    subs.unsubscribe();
  });
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
  <common-message-template></common-message-template>

  <!--오버레이 - 통신중 플래그를 활성화 할경우-->
  <v-overlay
    v-model="inCommunication"
    persistent>
    <div class="center-container">
      <common-logo />
    </div>
  </v-overlay>

  <!--오버레이 2 - 통신중 플래그를 우회해서 하기위함-->
  <v-overlay
    v-model="inTransmission"
    persistent>
    <div class="center-container">
      <v-progress-circular indeterminate></v-progress-circular>
    </div>
  </v-overlay>
</template>

<style>
  html, body {
    font-family: 'Roboto', sans-serif;
  }
  ul {
    list-style-type: none;
  }
  .center-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    width: 100vw;
  }
  .v-tab__slider {
    height:4px;
  }
  .ag-floating-top-container {
    font-weight: bold; /* 글자를 굵게 */
  }

  .custom-tooltip .tooltip-content {
    visibility: hidden;
    width: 120px;
    background-color: black;
    color: #fff;
    text-align: left;
    border-radius: 6px;
    padding: 5px;
    position: absolute;
    z-index: 1;
    bottom: 100%;
    left: 50%;
    margin-left: -60px;
  }

  .custom-tooltip {
    max-width: 200px;
    word-wrap: break-word;
    white-space: pre-line;
  }
</style>
