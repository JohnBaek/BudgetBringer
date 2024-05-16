
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
            <budget-plan-grid isAbove500k="true"  title="Above 500K (Budget)"/>
          </v-window-item>

          <!--Below 500K Budget-->
          <v-window-item value="below">
            <budget-plan-grid isAbove500k="false"  title="Below 500K (Budget)"/>
          </v-window-item>
        </v-window>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetPlanGrid from "./budget-plan-grid.vue";
import {CommonButtonDefinitions} from "../../../shared/grids/common-grid-button";

/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(null);

</script>
