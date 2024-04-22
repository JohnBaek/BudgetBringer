<script setup lang="ts">
import HomeDrawer from "./home-drawer.vue";
import {DrawerLink} from "./models-view/drawer-link";
import router from "../router";
import {onBeforeMount, onMounted, provide, ref} from "vue";
import {useRoute} from "vue-router";
import {RoutingStore} from "../stores/routing-store";


/**
 * 모바일 Drawer 가 열렸는지 안열렸는지 여부 : Home Drawer 에서 참조
 */
const miniDrawer = ref(null);
provide('miniDrawer',miniDrawer);

/**
 * 현재 보관중인 링크 정보
 */
let currentLink = ref(new DrawerLink('','','','',[],false,[]));

/**
 * 라우트 서비스
 */
const route = useRoute();

/**
 * onBeforeMount 핸들링
 */
onBeforeMount(() => {
  // 라우팅 정보가 있을경우
  if (currentLink.value.route !== '')
    return;

  // 스토어 정보를 가져온다.
  const routingStore = RoutingStore();

  // URI 없이 요청한경우
  if(route.fullPath === '/' || route.fullPath === '') {
    // 로그인 페이지로 이동
    router.push('/login');
    return;
  }

  // 라우팅 정보를 업데이트한다.
  const result = routingStore.tryUpdateRoute(route.fullPath);

  // 라우팅 정보를 찾는데 성공한경우
  if(result) {
    // 라우팅정보를 업데이트 한다.
    currentLink.value = routingStore.getCurrentRoute();
  }

  // 실패한 경우
  if(!result) {
    // 페이지 없음으로 이동
    router.push('/no-page');
    console.log('페이지 없음으로 이동');
  }
});


/**
 * Drawer 에서 링크클릭으로 메뉴가 변경 되었을 경우
 * @param link 링크 정보
 */
const onChangeMenu = async (link: DrawerLink ) => {
  // 현재 라우팅 정보와 동일한 라우트 정보를 요청할경우
  if (currentLink.value.route === link.route)
    return;

  // 현재 라우팅 정보를 업데이트한다.
  currentLink.value = link;

  // 라우팅 한다.
  await router.push(currentLink.value.route);
}

/**
 * 마운트 핸들링
 */
onMounted(() => {
  // watch(drawerStatus, (status) => {
  //   console.log(status)
  // });
});
</script>

<template>
  <!--좌측 Drawer-->
  <home-drawer @changeMenu="onChangeMenu($event)"></home-drawer>

  <!--모바일 Drawer 가 열린경우 화면을 가린다.-->
  <v-row class="ma-10" v-if="!miniDrawer">
    <v-col cols="12">
      <!--사이트 타이틀및 사이트 헤더-->
      <div class="mt-10">
        <h1>{{currentLink.title}}</h1>
        <span>{{currentLink.description}}</span>
      </div>
    </v-col>
    <v-col>
      <router-view />
    </v-col>
  </v-row>
</template>

