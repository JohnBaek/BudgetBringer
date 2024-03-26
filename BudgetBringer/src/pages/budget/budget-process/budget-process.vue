
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet rounded>
        <v-container>
          <v-tabs v-model="tab">
            <v-tab value="Owner">
              P&L Owner
            </v-tab>
            <v-tab value="Bu">
              BU
            </v-tab>
            <v-tab value="Below">
              Below CHF500K Approved
            </v-tab>
          </v-tabs>
        </v-container>

        <v-window v-model="tab">
          <v-window-item value="Owner">
            <v-container>
              <v-row>
                <v-col cols="12" md="4">
                  <budget-process-grid-pl-owner500k-below
                    :full-date="fullDate"
                    :year="year"
                    title="CAPEX below CHF500K"
                    sub-title="500K 아래의 정보"
                  >
                  </budget-process-grid-pl-owner500k-below>
                </v-col>
                <v-col cols="12" md="4">
                  <budget-process-grid-pl-owner500k-below
                    :full-date="fullDate"
                    :year="year"
                    title="CAPEX above CHF500K"
                    sub-title="500K 이상의 정보"
                  >
                  </budget-process-grid-pl-owner500k-below>
                </v-col>
                <v-col cols="12" md="4">
                  <budget-process-grid-pl-owner500k-below
                    :full-date="fullDate"
                    :year="year"
                    title="Total Amount"
                    sub-title="전체 합산 정보 "
                  >
                  </budget-process-grid-pl-owner500k-below>
                </v-col>
              </v-row>
            </v-container>
          </v-window-item>
        </v-window>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetProcessGridPlOwner500kBelow from "./budget-process-grid-pl-owner-500k-below.vue";
import BudgetPlanGridBelow500k from "../budget-plan/budget-plan-grid-below-500k.vue";
import BudgetPlanGridAbove500k from "../budget-plan/budget-plan-grid-above-500k.vue";

/**
 * 오늘 날짜
 * @type {Date}
 */
const today = new Date();

/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(null);

/**
 * yyyy-MM-dd 형식으로 날짜 정보를 가져온다.
 */
const getFullDate = ()  =>  {
  const yyyy = today.getFullYear();
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // 월은 0부터 시작하므로 1을 더해줍니다.
  const dd = String(today.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
}

/**
 * yyyy 형식으로 날짜 정보를 가져온다.
 */
const getYear = ()  =>  {
  return parseInt(today.getFullYear());
}


/**
 * 전체 날짜
 */
const fullDate = getFullDate();

/**
 * 년도
 */
const year = getYear();
</script>
