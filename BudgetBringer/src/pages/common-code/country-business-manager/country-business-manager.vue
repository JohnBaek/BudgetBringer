
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet class="pa-5">
        <v-row>
          <v-col cols="12" md="12">
            <country-business-manager-grid/>
          </v-col>
        </v-row>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import CountryBusinessManagerGrid from "./country-business-manager-grid.vue";
</script>
