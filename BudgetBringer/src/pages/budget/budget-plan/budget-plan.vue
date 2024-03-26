
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet class="pa-5">
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
            <budget-plan-grid-above500k />
          </v-window-item>

          <!--Below 500K Budget-->
          <v-window-item value="below">
            <budget-plan-grid-below500k />
          </v-window-item>
        </v-window>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetPlanGrid from "./budget-plan-grid-above-500k.vue";
import BudgetPlanGridAbove500k from "./budget-plan-grid-above-500k.vue";
import BudgetPlanGridBelow500k from "./budget-plan-grid-below-500k.vue";

/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(null);
</script>
