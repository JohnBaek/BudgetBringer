<script setup lang="ts">
import router from "../../router";
import {authenticationService} from "../../services/api-services/authentication-service";
import {AuthenticationStore} from "../../services/stores/authentication-store";

  /**
   * 인증 상태 관리
   */
  const authenticationStore = AuthenticationStore();

  /**
   * 로그아웃을 처리한다.
   */
  const logout = () => {
    // 서버로 통신한다.
    authenticationService.logout().subscribe({
      next() {
        // 데이터를 업데이트한다.
        authenticationStore.clearAuthenticated();
      },
      async complete() {
        await router.push('/login');
      },
    })
  }
</script>

<template>
  <v-dialog width="auto">
    <v-card min-width="250" title="로그아웃" text="로그아웃 하시겠습니까?">
      <template v-slot:actions>
        <v-btn class="ms-auto"
               text="확인"
               @click="logout()"
        >
        </v-btn>
      </template>
    </v-card>
  </v-dialog>
</template>

<style scoped lang="css">

</style>
