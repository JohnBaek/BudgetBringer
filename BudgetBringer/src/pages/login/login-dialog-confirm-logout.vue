<script setup lang="ts">
  import router from "../../router";

  /**
   * 로그아웃
   */
  const logout = () => {
    router.push('/login');
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
