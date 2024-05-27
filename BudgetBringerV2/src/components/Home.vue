<script setup lang="ts">
import HomeMenuBar from '@/components/HomeMenuBar.vue'
import { useRoutingStore } from '@/services/stores/RoutingStore'

// Stores
const routingStore = useRoutingStore();
</script>

<template>
  <HomeMenuBar></HomeMenuBar>
  <div class="card mt-2">
    <Panel>
      <template #header>
        <div class="header-content">
          <div style="padding-left:13px;">
            <h2><i :class="routingStore.currentRoute?.icon"></i> {{ routingStore.currentRoute?.label }}</h2>
            <h4 class="subtitle">{{ routingStore.currentRoute?.subTitle }}</h4>
          </div>
        </div>
      </template>
      <div style="padding:13px">
        <router-view />
      </div>
    </Panel>
  </div>
</template>

<style scoped>
.header-content {
  display: flex;
  flex-direction: column;
}
.subtitle {
  margin-top: -10px; /* 필요한 경우 간격 조정 */
  font-weight:normal;
  color: darkgrey;
}
</style>