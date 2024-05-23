<script setup lang="ts">
import { ref } from 'vue'
import { RequestLogin } from '@/models/requests/login/request-login'

// Is Communication To Sever?
const inCommunication = ref(false);

// Request Model
const model = ref(new RequestLogin('',''));

/**
 * Try Login
 */
const requestLogin = async () => {
  const response = await fetch('');
}

// const authenticationStore = AuthenticationState();
</script>

<template>
  <div class="center-screen">
    <div class="card">
      <div class="flex flex-column">
        <div class="flex align-items-center justify-content-center">
          <i class="mdi mdi-checkbox-marked-circle" style="font-size: 2rem"></i>
        </div>
        <div class="flex align-items-center justify-content-center">
          <h2> Capex Budget 관리</h2>
        </div>

        <div class="p-col-12 p-md-6 p-lg-4">
          <InputGroup>
            <InputText placeholder="아이디"  class="h-3rem" v-model="model.loginId"/>
          </InputGroup>
        </div>
        <div class="p-col-12 p-md-6 p-lg-4 mt-3">
          <InputGroup>
            <InputText placeholder="패스워드"  type="password" class="h-3rem" v-model="model.password"/>
          </InputGroup>
        </div>
        <div class="p-col-12 p-md-6 p-lg-4 mt-3">
          <Button style="width: 100%;" class="w-30rem h-3rem" :disabled="model.loginId === '' || model.password === ''">
            <div class="text-center" style="width: 100%" v-if="!inCommunication">
              로그인
              <i class="pi pi-spin pi-spinner" style="font-size: 2rem" v-if="inCommunication"></i>
            </div>
          </Button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.center-screen {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh; /* Full viewport height */
}
.text-center {
  text-align: center;
}
</style>