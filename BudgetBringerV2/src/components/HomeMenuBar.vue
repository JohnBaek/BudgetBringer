<script setup lang="ts">
import { onBeforeMount, ref } from 'vue'
import { type RoutingInformation, useRoutingStore } from '@/services/stores/RoutingStore'
import { useAuthenticationStore } from '@/services/stores/AuthenticationStore'
import type { MenuItem } from 'primevue/menuitem'
import { useRouter } from 'vue-router'
import { useConfirm } from 'primevue/useconfirm'
import { AuthenticationApiService } from '@/services/apis/AuthenticationApiService'
import { useCommunicationStore } from '@/services/stores/CommunicationStore'
import { useMessageStore } from '@/services/stores/MessageStore'

// Stores
const routingStore = useRoutingStore();
const messageStore = useMessageStore();
const authenticationStore = useAuthenticationStore();

// Routing List
const items = ref(Array<MenuItem>());

// Router
const router = useRouter();

// Confirm Service
const confirm = useConfirm();

// Rest API
const restApi: AuthenticationApiService = new AuthenticationApiService();

// onBeforeMount
onBeforeMount(() =>{
  const routes = routingStore.routingList;

  // Process all routes
  for (const route of routes) {
    // 권한이 있는경우
    if(authenticationStore.hasPermission(route.permissions)) {
      items.value.push(route)
    }
  }
})

/**
 * When Click the menu
 * @param routing Clicked routing information
 */
const navigate = (routing: RoutingInformation) =>{
  // Logout
  if(routing && routing.key === 'logout') {
    confirm.require({
      header: '나가기',
      message: 'Capex Budget 관리를 나가시겠어요?',
      icon: 'pi pi-info-circle',
      rejectLabel: '취소',
      acceptLabel: '나갈께요',
      rejectClass: 'p-button-secondary p-button-outlined',
      acceptClass: 'p-button-danger',
      accept: () => {
        restApi.logoutAsync().subscribe(() => {
          messageStore.showSuccess("","로그아웃 되었습니다.");
          router.push('/login');
        })
      },
      reject: () => {}
    });
  }
  // Route
  else {
    // Update Current Routing
    routingStore.currentRoute = routing;
    if(routingStore.currentRoute.route)
      router.push(routingStore.currentRoute.route);
  }
}

</script>

<template>
  <Menubar :model="items">
    <template #item="{ item, props, hasSubmenu }" >
      <div v-if="item.route" @click="navigate((item as RoutingInformation))">
        <div  v-bind="props.action" >
          <span :class="item.icon" />
          <span class="ml-2">{{ item.label }}</span>
        </div>
      </div>
      <a v-else v-bind="props.action">
        <span :class="item.icon" />
        <span class="ml-2">{{ item.label }}</span>
        <span v-if="hasSubmenu" class="pi pi-fw pi-angle-down ml-2" />
      </a>
    </template>

    <template #end>
      <div style="margin-right:20px;">
        <h4><i class="mdi mdi-checkbox-marked-circle" style="font-size: 1.0rem" />Capex Budget 관리</h4>
      </div>

    </template>
  </Menubar>
</template>

<style scoped>
.p-menubar , .p-component {
  padding-left: 25px;
}
</style>