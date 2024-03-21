
<template>


  <v-layout>
    <div class="display-1 ma-4">
      <!--상단로고-->
      <h3>
        <v-icon
          icon="mdi-checkbox-marked-circle"
          end
          size="25"
        ></v-icon> Budget Bringer</h3>
    </div>
    <v-spacer></v-spacer>
    <!--메뉴 Drawer-->
    <div class="display-1 ma-2">
      <v-app-bar-nav-icon @click="drawerShowMenu()" v-if="!drawer" class="mr-3"></v-app-bar-nav-icon>
      <v-btn variant="text" @click="drawerCloseMenu()" v-if="drawer" class="mt-1">
        <v-icon class="ma-3">mdi-close</v-icon>
      </v-btn>
    </div>
  </v-layout>
  <!--메뉴 이동 Navigation Bar-->
  <v-main >
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

    <!--라우팅 뷰어-->
    <div  class="ml-6 mr-6" v-if="!drawer">
      <h3>{{currentRouted.title}}</h3>
      <div class="mt-3 mb-3"></div>
      <router-view ></router-view>
    </div>
  </v-main>
</template>


<style scoped>
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
  new RoutingInformation('공통코드', '/common-code', 'mdi-code-tags') ,
  new RoutingInformation('예산계획', '/budget/plan', 'mdi-notebook') ,
  new RoutingInformation('예산승인', '/budget/approved', 'mdi-check') ,
  new RoutingInformation('예산진행현황', '/budget/process', 'mdi-currency-usd') ,
]);


// 현재 라우팅정보
let currentRouted = new RoutingInformation('','','');


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
  if(currentRouted.route === routingInformation.route)
    return;

  // 메뉴를 닫고
  drawerCloseMenu();

  // 라우팅 한다.
  router.push(routingInformation.route);
  currentRouted = routingInformation;
}
</script>
