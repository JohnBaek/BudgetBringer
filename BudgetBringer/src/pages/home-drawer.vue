<script setup lang="ts">
import CommonLogo from "../shared/common-logo.vue";
import {inject, onBeforeMount, Ref, ref} from "vue";
import {DrawerLink} from "./models-view/drawer-link";
import {RoutingStore} from "../services/stores/routing-store";
import LoginDialogConfirmLogout from "./login/login-dialog-confirm-logout.vue";
import {AuthenticationStore} from "../services/stores/authentication-store";
import {ResponseUser} from "../models/responses/users/response-user";

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
const links = ref(Array<DrawerLink>());

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
  // 컨테이너(그룹) 인경우
  if(drawerLink.isContainerMenu)
    return;

  // changeMenu 를 notify 한다.
  emits('changeMenu' , drawerLink);

  // drawer 를 닫는다.
  (miniDrawer as Ref<boolean>).value = false;
}

/**
 * 로그인한 사용자 정보
 */
const authenticatedUser = ref<ResponseUser>();

/**
 * 로그아웃 다이얼로그
 */
const logout = ref(false);

/**
 * 마운팅 되기전 핸들링
 */
onBeforeMount(() =>{
  // 모든 라우팅을 가져온다.
  const routes = routingStore.getRoutingList();

  // 인증 상태관리를 가져온다.
  const authenticationStore = AuthenticationStore();

  // 인증상태가 반영된 라우팅 정보
  const routingWithClaims = [];

  // 모든 라우팅정보에 대해 처리한다.
  for (const route of routes) {
    // 권한이 있는경우
    if(authenticationStore.hasPermission((route as DrawerLink).permissions)) {
      routingWithClaims.push(route);
    }
  }
  links.value = routingWithClaims;

  // 로그인한 사용자 정보를 가져온다.
  authenticatedUser.value = authenticationStore.authenticatedUser as ResponseUser;
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
        <b class="text-black">{{(authenticatedUser as ResponseUser).displayName}} 님</b> 안녕하세요.
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
        <!-- 그룹 메뉴가 아닌경우 ( 일반 메뉴 ) -->
        <v-list-item-title v-if="item.isContainerMenu == false">
          <v-icon class="mb-1">{{item.icon}}</v-icon>
          <v-label class="ml-5 text-shades-black"><b>{{item.title}}</b></v-label>
        </v-list-item-title>

        <!-- 그룹메뉴인 경우 -->
        <v-list-group v-if="item.isContainerMenu">
          <template v-slot:activator="{ props }">
            <v-list-item
              v-bind="props"
            >
              <v-list-item-title>
                <v-label class="ml-7 text-shades-black"><b>{{item.title}}</b></v-label>
              </v-list-item-title>
            </v-list-item>
          </template>

          <v-list-item
            v-for="(item, key) in item.childMenus"
            :key="key"
            link
            @click="clickMenu(item)"
          >
            <v-list-item-title>
              <v-label class="ml-3 text-shades-black"><b>{{item.title}}</b></v-label>
            </v-list-item-title>
          </v-list-item>
        </v-list-group>
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
          <v-list-item v-if="item.isContainerMenu == false">
            <v-btn  variant="flat">
              <v-list-item-title class="d-flex align-center">
                <v-icon>{{item.icon}}</v-icon>
                <h3 class="ml-2">{{item.title}}</h3>
              </v-list-item-title>
            </v-btn>
          </v-list-item>

          <!-- 그룹메뉴인 경우 -->
          <v-list-group v-if="item.isContainerMenu">
            <template v-slot:activator="{ props }">
              <v-list-item
                v-bind="props"
              >
                <v-list-item-title>
                  <h3 class="ml-11">{{item.title}}</h3>
                </v-list-item-title>
              </v-list-item>
            </template>

            <v-list-item
              v-for="(item, key) in item.childMenus"
              :key="key"
              link
              @click="clickMenu(item)"
            >
              <v-list-item-title>
                <h3 class="ml-7 mt-2">{{item.title}}</h3>
              </v-list-item-title>
            </v-list-item>
          </v-list-group>
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

.custom-expansion-panel .v-expansion-panel-header {
  border: none !important;
}

</style>
