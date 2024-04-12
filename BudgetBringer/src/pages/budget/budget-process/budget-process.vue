
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet rounded class="pa-5">
        <v-tabs v-model="tab" class="mb-5">
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

        <v-window v-model="tab">
          <v-window-item value="Owner">
              <v-row>
                <v-col cols="12" md="12" >
                  <budget-process-grid-pl-owner
                    :full-date="fullDate"
                    :year="year"
                    title="CAPEX below CHF500K"
                    sub-title="500K 아래의 정보"
                  >
                  </budget-process-grid-pl-owner>
                </v-col>
              </v-row>
          </v-window-item>
        </v-window>
      </v-sheet>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
import BudgetProcessGridPlOwner from "./budget-process-grid-pl-owner.vue";


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
