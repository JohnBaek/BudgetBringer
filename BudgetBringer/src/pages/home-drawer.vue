<script setup lang="ts">
import CommonLogo from "../shared/common-logo.vue";
import {inject, onBeforeMount, ref} from "vue";
import {DrawerLink} from "./models/drawer-link";
import {RoutingStore} from "../stores/routing-store";
import LoginDialogConfirmLogout from "./login/login-dialog-confirm-logout.vue";

/**
 * Drawer 상태
 */
const drawer = ref(null);

/**
 * Pinia 라우팅 스토어
 */
const routingStore = RoutingStore();

/**
 * 메뉴정보
 */
const links = ref([]);

/**
 * 미니 Drawer
 */
const miniDrawer = inject('miniDrawer');
const miniDrawers = ref(null);

/**
 * emit 정의
 */
const emits = defineEmits<{
  // 메뉴가 변경될때
  (e: 'changeMenu', linkInformation :DrawerLink ): DrawerLink
}>();

/**
 * 페이지를 이동한다.
 * @param drawerLink Drawer 링크 정보
 */
const clickMenu = (drawerLink: DrawerLink) => {
  // changeMenu 를 notify 한다.
  emits('changeMenu' , drawerLink);

  // drawer 를 닫는다.
  miniDrawer.value = false;
}



/**
 * 로그아웃 다이얼로그
 */
const logout = ref(false);

/**
 * 마운팅 되기전 핸들링
 */
onBeforeMount(() =>{
  links.value = routingStore.getRoutingList();
})
</script>

<template>
  <!--Drawer 사용 풀 페이지-->
  <v-navigation-drawer v-model="drawer">
    <!--좌상단-->
    <v-container class="mt-5 mb-5">
      <common-logo class="mb-3"></common-logo>
      <v-spacer></v-spacer>
      <span class="mb-5"></span>
      <span class="text-grey">
        <b class="text-black">관리자님</b> 안녕하세요.
      </span>
    </v-container>

    <!--구분선-->
    <v-divider></v-divider>

    <!--링크 리스트-->
    <v-list>
      <v-list-item
        v-for="(item, key) in links"
        :key="key"
        link
        @click="clickMenu(item)"
      >
        <v-list-item-title>
          <v-icon class="mb-1">{{item.icon}}</v-icon>
          <v-label class="ml-5 text-shades-black"><b>{{item.title}}</b></v-label>
        </v-list-item-title>
      </v-list-item>
    </v-list>

    <template v-slot:append>
      <div class="pa-2 mb-5">
        <v-btn block prepend-icon="mdi-logout" variant="outlined" @click="logout = !logout">
          <b>로그아웃</b>
        </v-btn>
      </div>
    </template>
  </v-navigation-drawer>

  <!--Drawer 사용 풀페이지가 아닌 경우-->
  <v-layout v-if="!drawer">
    <v-app-bar  fixed
                scroll-behavior="elevate"
                elevation="0">
      <template v-slot:prepend>
        <v-container>
          <common-logo/>
        </v-container>
      </template>
      <v-spacer></v-spacer>
      <!--네비게이션 아이콘-->
      <v-app-bar-nav-icon @click="miniDrawer = !miniDrawer" v-if="!miniDrawer"></v-app-bar-nav-icon>
      <v-app-bar-nav-icon @click="miniDrawer = !miniDrawer" v-if="miniDrawer" icon="mdi-close"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-main>
      <!--미니 Drawer-->
      <div v-if="miniDrawer" >
        <v-list v-for="(item, i) in links"
                :key="i"
                :value="item"
                @click="clickMenu(item)"
        >
          <v-list-item>
            <v-btn  variant="flat">
              <v-list-item-title class="d-flex align-center">
                <v-icon>{{item.icon}}</v-icon>
                <h3 class="ml-2">{{item.title}}</h3>
              </v-list-item-title>
            </v-btn>
          </v-list-item>
        </v-list>
        <v-list>
          <v-list-item>
            <v-row>
              <v-col sm="12" md="6" lg="2">
                <v-btn block prepend-icon="mdi-logout" align="center" variant="outlined" @click="logout = !logout">
                  <b>로그아웃</b>
                </v-btn>
              </v-col>
            </v-row>
          </v-list-item>
        </v-list>
      </div>
    </v-main>
  </v-layout>

  <!--로그아웃 다이얼로그-->
  <LoginDialogConfirmLogout v-model="logout" />
</template>

<style scoped lang="css">
.overlay {
  position: fixed;
  background: white;
  top: 100px;
  left: 0;
  width: 100%;
  height: 100%;
  justify-content: left;
  align-items: center;
  z-index: 5;
}

.content {
  background-color: white;
  padding: 20px;
  border-radius: 10px;
}
</style>
