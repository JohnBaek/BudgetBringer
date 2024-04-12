<script setup lang="ts">

import LogActionGrid from "./log-action-grid.vue";
</script>

<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <log-action-grid isAbove500k="true"  title="Above 500K (Budget)" />
    </div>
  </div>
</template>

<style scoped lang="css">

</style>
