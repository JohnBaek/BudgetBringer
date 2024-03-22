
<template>
  <v-app-bar
    fixed
    scroll-behavior="elevate"
    elevation="1"
  >
    <div class="logo ml-10" >
      <!--상단로고-->
      <h3>
        <v-icon
          icon="mdi-checkbox-marked-circle"
          end
          size="25"
        >
        </v-icon> Budget Bringer
      </h3>
    </div>

    <!--메뉴 Drawer-->
    <v-spacer class="hamburger-menu"></v-spacer>
    <div class="display-1 ma-2 hamburger-menu">
      <v-app-bar-nav-icon @click="drawerShowMenu()" v-if="!drawer" class="mr-3"></v-app-bar-nav-icon>
      <v-btn variant="text" @click="drawerCloseMenu()" v-if="drawer" class="mt-1">
        <v-icon class="ma-3">mdi-close</v-icon>
      </v-btn>
    </div>
    <!-- 메뉴 -->
    <v-container class="top-menu ">
      <v-spacer></v-spacer>
      <v-row>
        <v-col
          v-for="(item, i) in menus"
          :key="i"
          :value="item"
          cols="auto"
          >
          <v-btn
            variant="text" @click="routePage(item)" >
            <h3> {{item.title}}</h3>
          </v-btn>
        </v-col>
      </v-row>

      <!-- 메뉴 항목들 -->
      <!-- 오른쪽 콘텐츠와 메뉴 아이템 사이의 공간을 균등하게 나눕니다 -->
    </v-container>
  </v-app-bar>

  <!--메뉴 이동 Navigation Bar-->
  <v-main class="main-content">
    <v-container >
      <v-layout row wrap>
        <!-- 다른 컨텐츠 -->
        <div >
          <v-list
            v-if="drawer"
            v-for="(item, i) in menus"
            :key="i"
            :value="item"
          >
            <v-list-item>
              <template v-slot:prepend>
                <v-btn variant="text" @click="routePage(item)" >
                  <h3> <v-icon>{{item.icon}}</v-icon>  {{item.title}}</h3>
                </v-btn>
              </template>
            </v-list-item>
          </v-list>
        </div>
      </v-layout>
    </v-container>
  </v-main>

  <!--라우팅 뷰어-->
  <div v-if="!drawer">
    <v-card  elevation="10" class="ml-10 mr-10 rounded-lg" >
      <v-card-item>
        <v-card-title class="mt-5 mb-1"><h3>
          <v-icon size="small">{{currentRouted.icon}}</v-icon>&nbsp;{{currentRouted.title}}
        </h3></v-card-title>
        <v-card-subtitle class="mb-5" >{{currentRouted.description}}</v-card-subtitle>
        <v-divider></v-divider>
        <router-view></router-view>
      </v-card-item>
    </v-card>
  </div>
</template>

<style scoped>
.main-content {
  padding-top: 0px;
}

/* 기본적으로 햄버거 메뉴를 숨깁니다 */
.hamburger-menu {
  display: none;
}

/* 상단 메뉴를 표시합니다 */
.top-menu {
  display: block;
}

.logo {
  width: 230px;
}

.right-menu {
  display: flex;
  justify-content: flex-end;
}

/* 화면 크기가 패드 크기 이하가 되면 햄버거 메뉴를 표시하고 상단 메뉴를 숨깁니다 */
@media (max-width: 768px) { /* 패드 크기를 예로 듭니다 */
  .hamburger-menu {
    display: block;
  }
  .top-menu {
    display: none;
  }


}
</style>

<script setup lang="ts">
import { ref, watch } from 'vue';
import {createRouter, createWebHistory, useRouter} from 'vue-router';
import {RoutingInformation} from "../models/routing-information";

// Drawer 여부
const drawer = ref<boolean>(false);

// 라우터
const router = useRouter();

// 메뉴 정보
const menus = ref([
  new RoutingInformation('공통코드', '공통코드를 관리합니다.', '/common-code', 'mdi-code-tags') ,
  new RoutingInformation('예산계획', '예산계획을 세우고 작성합니다.','/budget/plan', 'mdi-notebook') ,
  new RoutingInformation('예산승인', '계획된 예산을 승인합니다.','/budget/approved', 'mdi-check') ,
  new RoutingInformation('예산진행현황', '예산 사용 진행 현황에대해서 확인합니다.','/budget/process', 'mdi-currency-usd') ,
]);

console.log(11,menus.value)

// 현재 라우팅정보
let currentRouted = ref<RoutingInformation>(new RoutingInformation('예산계획', '예산계획을 세우고 작성합니다.','/budget/plan', 'mdi-notebook'));


/**
 * 메뉴를 보여줄때
 */
const drawerShowMenu = () => {
  drawer.value = true;
};

/**
 * 메뉴를 닫을때
 */
const drawerCloseMenu = () => {
  drawer.value = false;
};

/**
 * 페이지로 라우팅한다.
 * @param routingInformation 라우팅 정보
 */
const routePage = (routingInformation :RoutingInformation) => {
  // 동일한 라우트 주소로 요청하는 경우
  if(currentRouted.value.route === routingInformation.route)
    return;

  // 메뉴를 닫고
  drawerCloseMenu();

  // 라우팅 한다.
  router.push(routingInformation.route);
  currentRouted.value = routingInformation;
}
</script>
