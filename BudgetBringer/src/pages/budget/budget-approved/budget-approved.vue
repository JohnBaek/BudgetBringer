
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet class="pa-5">
        <!--그리드 탭-->
        <v-tabs v-model="tab">
          <!--Above 500K Budget-->
          <v-tab value="above">
            CAPEX Above CHK500K
          </v-tab>
          <!--Below 500K Budget-->
          <v-tab value="below">
            CAPEX Below CHK500K
          </v-tab>
        </v-tabs>

        <!--탭 개별 화면-->
        <v-window v-model="tab">
          <!--Above 500K Budget-->
          <v-window-item  value="above">
            <budget-approved-grid isAbove500k="true"  title="CAPEX Above CHK500K"/>
          </v-window-item>

          <!--Below 500K Budget-->
          <v-window-item value="below">
            <budget-approved-grid isAbove500k="false"  title="CAPEX Below CHK500K"/>
          </v-window-item>
        </v-window>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetApprovedGrid from "./budget-approved-grid.vue";

/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(null);
</script>
