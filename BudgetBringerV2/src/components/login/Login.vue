<script setup lang="ts">
import { ref } from 'vue'
import { RequestLogin } from '@/models/requests/login/request-login'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import { AuthenticationApiService } from '@/services/apis/AuthenticationApiService'
import router from '@/router'
import Logo from '@/components/Logo.vue'

// Request Model
const model = ref(new RequestLogin());

// Communication Store
const communicationStore = useCommunicationStore();

// Rest API
const restApi: AuthenticationApiService = new AuthenticationApiService();

/**
 * Try Login
 */
const requestLoginAsync = async () => {
  if(model.value.isInvalid())
    return;

  // Request to server
  restApi.tryLoginAsync(model.value).subscribe(async (response) => {
    if (response.success) {
      await router.push('/budget/statistics');
      console.log('route')
    }
  });
}
</script>

<template>
  <div class="center-screen">
    <div class="card">
      <div class="flex flex-column">
        <Logo />
        <div class="p-col-12 p-md-6 p-lg-4">
          <InputGroup>
            <InputText placeholder="아이디"  class="h-3rem" v-model="model.loginId" @keyup.enter="requestLoginAsync" :disabled="communicationStore.communication"/>
          </InputGroup>
        </div>
        <div class="p-col-12 p-md-6 p-lg-4 mt-3">
          <InputGroup>
            <InputText placeholder="패스워드"  type="password" class="h-3rem" v-model="model.password" @keyup.enter="requestLoginAsync" :disabled="communicationStore.communication"/>
          </InputGroup>
        </div>
        <div class="p-col-12 p-md-6 p-lg-4 mt-3">
          <Button style="width: 100%;" class="w-30rem h-3rem" @click="requestLoginAsync" :disabled="model.loginId === '' || model.password === '' || communicationStore.communication ">
            <div class="text-center" style="width: 100%" >
              <div v-if="!communicationStore.communication">로그인</div>
              <i class="pi pi-spin pi-spinner" style="font-size: 2rem" v-if="communicationStore.communication"></i>
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