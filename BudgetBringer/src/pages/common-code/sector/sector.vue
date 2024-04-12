
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <budget-approved-grid isAbove500k="false"  title="CAPEX Below CHK500K"/>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetApprovedGrid from "./sector-grid.vue";

/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(null);
</script>
