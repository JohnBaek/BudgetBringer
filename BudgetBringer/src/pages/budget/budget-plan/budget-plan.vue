
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <!--그리드 탭-->
      <v-tabs v-model="tab">
        <!--Above 500K Budget-->
        <v-tab value="above">
          Above 500K (Budget)
        </v-tab>
        <!--Below 500K Budget-->
        <v-tab value="below">
          Below 500K (Budget)
        </v-tab>
      </v-tabs>

      <!--탭 개별 화면-->
      <v-window v-model="tab">
        <!--Above 500K Budget-->
        <v-window-item  value="above">
          <v-container>
            <budget-plan-grid></budget-plan-grid>
          </v-container>
        </v-window-item>

        <!--Below 500K Budget-->
        <v-window-item value="below">
          <budget-plan-grid></budget-plan-grid>
        </v-window-item>
      </v-window>

    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetPlanGrid from "./budget-plan-grid.vue";

/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(null);
</script>
