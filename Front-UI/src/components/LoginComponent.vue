<template>
  <v-app>
    <v-main>
      <v-container>
        <v-row justify="center" align="center" style="min-height: 100vh;">
          <v-col cols="12" sm="8" md="6" lg="4">

            <!-- 타이틀과 설명 -->
            <div class="text-center mb-5">
              <v-icon
                icon="mdi-checkbox-marked-circle"
                end
              ></v-icon>
              <h1 class="display-1 mb-1">Budget Bringer</h1>
              <span class="subtitle-1 text-grey">예산 관리 솔루션</span>
            </div>

            <!-- 로그인 폼 -->
            <v-form>
              <v-text-field label="아이디" variant="outlined" v-model="loginId"></v-text-field>
              <v-text-field label="패스워드" variant="outlined" type="password" v-model="password"></v-text-field>
              <v-btn
                large
                block
                color="primary"
                :height="50"
                :disabled="loginId === '' || password === ''"
                @click="tryLoginAsync"
              >
                <h3>로그인</h3>
              </v-btn>
            </v-form>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>
<style scoped>
</style>
<script setup lang="ts">

// 로그인 아이디
import {ref} from "vue";
import {loginService} from "../services/login-service";
import {EnumResponseResult} from "../models/Enums/EnumResponseResult";

let loginId = ref<string>('');

// 로그인 패스워드
let password = ref<string>('');

/**
 * 로그인을 시도한다.
 */
const tryLoginAsync = async () => {
  // 로그인을 시도 한다.
  const response = await loginService.tryLoginAsync(loginId.value,password.value);

  // 예외인경우
  if(response.result === EnumResponseResult.error)
    alert(response.message);
}

</script>
