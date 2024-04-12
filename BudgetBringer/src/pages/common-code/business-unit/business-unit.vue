
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet class="pa-5">
        <business-unit-grid/>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BusinessUnitGrid from "./business-unit-grid.vue";
</script>
