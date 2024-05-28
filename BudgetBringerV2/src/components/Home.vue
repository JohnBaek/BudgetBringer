<script setup lang="ts">
import HomeMenuBar from '@/components/HomeMenuBar.vue'
import { useRoutingStore } from '@/services/stores/RoutingStore'
import { useConfigStore } from '@/services/stores/ConfigStore'
import { ref } from 'vue'
import Logo from '@/components/Logo.vue'

// Stores
const routingStore = useRoutingStore();

// Whether is ready all systems
const ready = ref(false);

setTimeout(async () => {
  // Set Config information
  await useConfigStore().retrieve();
  ready.value = true;
},1000)
</script>

<template>
  <div v-if="ready === false" class="center-container">
    <Logo />
  </div>

  <div v-if="ready">
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

.center-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  width: 100vw;
}
</style>