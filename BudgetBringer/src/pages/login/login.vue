<script setup lang="ts">
import LoginLogo from "./login-logo.vue";
import {RequestLogin} from "../../models/requests/login/request-login";
import {ref} from "vue";
import {authenticationService} from "../../services/api-services/authentication-service";
import {EnumResponseResult} from "../../models/enums/enum-response-result";
import {messageService} from "../../services/message-service";
import router from "../../router";
import {AuthenticationStore} from "../../services/stores/authentication-store";

/**
 * 통신중 여부
 */
const inCommunication = ref(false);

/**
 * 로그인 요청 정보 Ref
 */
const request = ref(new RequestLogin('',''));

/**
 * 인증 상태 관리
 */
const authenticationStore = AuthenticationStore();


/**
 * 로그인 요청
 */
const requestLoginAsync = async () => {
  // 요청정보가 올바르지 않은경우
  if(request.value.loginId === '' && request.value.password === '')
    return;

  // 통신중 플래그를 변경한다.
  inCommunication.value = true;

  // 서버로 통신한다.
  authenticationService.tryLoginAsync(request.value).subscribe({
    async next(response) {
      // 요청에 실패한경우
      if (response.result != EnumResponseResult.success) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }

      // 데이터를 업데이트한다.
      authenticationStore.updateAuthenticated(response.data);
      await router.push('/budget/process');
    },
    complete() {
      inCommunication.value = false;
    },
  })

};
</script>

<template>
  <!--메인 레이아웃-->
  <v-row
    align="center"
    justify="center"
    style="min-height: 100vh;"
  >
    <v-col cols="11" sm="7" md="6" lg="4">
      <!--로고-->
      <LoginLogo />

      <!--공간 여백-->
      <v-container/>

      <!--로그인 폼-->
      <v-form>
        <!--아이디 패스워드-->
        <v-text-field @keyup.enter="requestLoginAsync" label="아이디" variant="outlined" v-model="request.loginId"></v-text-field>
        <v-text-field @keyup.enter="requestLoginAsync" label="패스워드" variant="outlined" type="password" v-model="request.password"></v-text-field>

        <!--로그인 버튼-->
        <v-btn
          large
          block
          color="primary"
          :height="50"
          :disabled="request.loginId === '' && request.password === ''"
          @click="requestLoginAsync"
        >
          <!--로그인 요청 중일경우 Progress Circle 활성화-->
          <v-progress-circular indeterminate v-if="inCommunication"></v-progress-circular>
          <h3 v-if="!inCommunication">로그인</h3>
        </v-btn>
      </v-form>
    </v-col>
  </v-row>

  <!--오버레이 사용 (통신중일때)-->
  <v-overlay v-model="inCommunication"></v-overlay>
</template>

<style scoped lang="css">
</style>
