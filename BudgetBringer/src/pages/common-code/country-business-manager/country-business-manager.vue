
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <v-sheet class="pa-5">
        <v-row>
          <!-- 각 컴포넌트를 별도의 v-col로 감싸고, cols 속성을 사용해 화면 크기에 따라 크기 조정 -->
          <v-col cols="12" md="6">
            <country-business-manager-grid @onCellClicked="cbmCellClicked"/>
          </v-col>
          <v-col cols="12" md="6">
            <country-business-manager-units-grid :countryBusinessManager="countryBusinessManagerRef"/>
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
import CountryBusinessManagerUnitsGrid from "./country-business-manager-units-grid.vue";

/**
 * CBM 에 해당하는 businessUnits
 */
const countryBusinessManagerRef = ref([]);

const cbmCellClicked = (data) => {
  countryBusinessManagerRef.value = data;
}
</script>
