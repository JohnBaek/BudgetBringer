
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet rounded class="pa-5">
        <v-tabs v-model="tab" class="mb-5">
          <v-tab value="owner">
            P&L Owner
          </v-tab>
          <v-tab value="bu">
            Business Unit
          </v-tab>
          <v-tab value="approvedbelow">
            Status of Purchase Below
          </v-tab>
          <v-tab value="approvedabove">
            Status of Purchase Above
          </v-tab>
        </v-tabs>
        <v-window v-model="tab">
          <v-window-item value="owner">
              <v-row>
                <v-col cols="12" md="12" >
                  <budget-process-grid-pl-owner
                    :full-date="fullDate"
                    :year="year"
                    :yearList="yearList"
                    title="CAPEX below CHF500K"
                    sub-title="500K 아래의 정보"
                  >
                  </budget-process-grid-pl-owner>
                </v-col>
              </v-row>
          </v-window-item>

          <v-window-item value="bu">
            <v-row>
              <v-col cols="12" md="12" >
                <budget-process-grid-bu
                  :full-date="fullDate"
                  :year="year"
                  :yearList="yearList"
                  title="CAPEX below CHF500K"
                  sub-title="500K 아래의 정보"
                >
                </budget-process-grid-bu>
              </v-col>
            </v-row>
          </v-window-item>

          <v-window-item value="approvedbelow">
            <v-row>
              <v-col cols="12" md="12" >
                <budget-process-grid-approved-below
                  :full-date="fullDate"
                  :year="year"
                  :yearList="yearList"
                  title="CAPEX below CHF500K"
                  sub-title="500K 아래"
                  :masterDetail="true"
                >
                </budget-process-grid-approved-below>
              </v-col>
            </v-row>
          </v-window-item>

          <v-window-item value="approvedabove">
            <v-row>
              <v-col cols="12" md="12" >
                <budget-process-grid-approved-above
                  :full-date="fullDate"
                  :year="year"
                  title="CAPEX above CHF500K"
                  sub-title="500K 이상"
                  :masterDetail="true"
                >
                </budget-process-grid-approved-above>
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
import BudgetProcessGridPlOwner from "./budget-process-owner/budget-process-grid-pl-owner.vue";
import BudgetProcessGridBu from "./budget-process-business-unit/budget-process-grid-bu.vue";
import BudgetProcessGridApprovedBelow from "./budget-process-approved/budget-process-grid-approved-below.vue";
import BudgetProcessGridApprovedAbove from "./budget-process-approved/budget-process-grid-approved-above.vue";

const route = useRoute();
const router = useRouter();

onMounted(() => {
  // tab.value = route.query.tab;
  console.log('route.query.tab',route.query.tab);
  console.log('tab.value',tab.value);

  if(route.query.tab) {
    const requestTab = route.query.tab.toString().toLowerCase();
    if(requestTab){
      console.log('tab.value',tab.value);
      tab.value = requestTab;
    }
  }
  else {
    router.replace({ query: { ...route.query, tab: 'owner' } });
  }
})
/**
 * 오늘 날짜
 * @type {Date}
 */
const today = new Date();
/**
 * 탭 데이터
 * @type {Ref<UnwrapRef<null>>}
 */
const tab = ref(route.query.tab || 'Owner');
watch(tab,(changeTab) => {
  router.replace({ query: { ...route.query, tab: changeTab } });
});

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
/**
 * Generate Year List
 * @param currentYear
 * @returns {[]}
 */
const createYearList = (currentYear) => {
  const startYear = currentYear - 5; // 7년 전
  const endYear = currentYear + 2;
  let yearList = [];

  for (let year = startYear; year <= endYear; year++) {
    yearList.push(year.toString());
  }

  yearList.push('전체년도');

  return yearList.reverse();
}
const yearList = createYearList(parseInt(year));
</script>
