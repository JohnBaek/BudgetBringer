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
              <v-text-field label="아이디" variant="outlined"></v-text-field>
              <v-text-field label="패스워드" variant="outlined"></v-text-field>
              <v-btn
                large
                block
                color="primary"
                :height="50"
              >
                <b>로그인 하기</b>
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
</script>
