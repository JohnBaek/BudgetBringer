<script setup lang="ts">
import { defineComponent } from 'vue';

defineComponent({
  inheritAttrs: false
});
</script>

<!--로고 공통 컨테이너-->
<template>
  <div v-bind="$attrs">
    <v-icon size="small" color="#ff9e22">mdi-checkbox-marked-circle</v-icon>
    <span class="ml-1">
      <b>Budget Bringer</b>
    </span>
  </div>
</template>

<style scoped lang="css">
</style>
