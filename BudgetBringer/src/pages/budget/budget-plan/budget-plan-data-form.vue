<script setup lang="ts">
import {ref} from "vue";
import {RequestBudgetPlan} from "../../../models/requests/budgets/request-budget-plan";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import {ResponseBusinessUnit} from "../../../models/responses/budgets/response-business-unit";
import CommonSelect from "../../../shared/common-select.vue";

/**
 * props
 */
const props = defineProps({
  modelValue: { type: RequestBudgetPlan, required: true } ,
});
/**
 * Data Model
 */
const model = ref(props.modelValue);
/**
 * outgoing
 */
const emits = defineEmits<{
  (e: 'submit') : void,
  (e: 'update:data', RequestBudgetPlan) : RequestBudgetPlan ,
}>();
/**
 * Watch
 */
watch(model, (newValue) => {
  emits("update:data", newValue);
}, { deep: true });
/**
 * Dispatch Submit
 */
const dispatchSubmit = () => {
  emits('submit');
}
/**
 * Country Business Managers
 */
let countryBusinessManagers: ResponseCountryBusinessManager [] = [];
/**
 * BusinessUnits
 */
const businessUnits = ref<Array<ResponseBusinessUnit>>(  []);
/**
 * When countryBusinessManager list Updated
 * @param items
 */
const onCountryBusinessManagerUpdated = (items: any) => {
  countryBusinessManagers = items;
}
/**
 * When countryBusinessManager selected
 * @param countryBusinessManagerId
 */
const onChangeCountryBusinessManager = (countryBusinessManagerId: any) => {
  // Reset
  model.value.countryBusinessManagerId = '';

  // Try find Business Units in countryBusinessManager
  const _countryBusinessManagers = countryBusinessManagers.filter(i => i.id === countryBusinessManagerId);

  // Reset Business Units
  businessUnits.value = [];

  // Found Business Units ?
  if(_countryBusinessManagers.length > 0)
    businessUnits.value = _countryBusinessManagers[0].businessUnits;
}
</script>

<template>
  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">통계 포함 여부</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-label class="mb-5 mr-5" v-if="model.isIncludeInStatistics">
        <b class="text-blue">포함</b>
      </v-label>
      <v-label class="mb-5 mr-5" v-if="!model.isIncludeInStatistics">
        <b class="text-grey">미포함</b>
      </v-label>
      <v-switch v-model="model.isIncludeInStatistics" color="primary" required></v-switch>
    </v-col>
  </v-row>
  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Country Businesses Manager</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <common-select required v-model="model.countryBusinessManagerId" @onChange="onChangeCountryBusinessManager" @onDataUpdated="onCountryBusinessManagerUpdated" title="name" value="id" requestApiUri="/api/v1/CountryBusinessManager" />
    </v-col>
  </v-row>
  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Business Unit</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-select
        density="compact" variant="outlined" outlined required v-model="model.businessUnitId" item-title="name" item-value="id" :items="businessUnits" :disabled="businessUnits.length === 0"></v-select>
    </v-col>
  </v-row>
  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Cost Center</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <common-select required v-model="model.costCenterId" title="value" value="id"  requestApiUri="/api/v1/CostCenter" />
    </v-col>
  </v-row>
  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Sector</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <common-select required v-model="model.sectorId" title="value" value="id"  requestApiUri="/api/v1/Sector" />
    </v-col>
  </v-row>

  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Approval Date</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-text-field density="compact" placeholder="값을 입력해주세요" required v-model="model.approvalDate"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
  </v-row>

  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Description</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-text-field density="compact" placeholder="값을 입력해주세요" v-model="model.description"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
  </v-row>

  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Budget Total</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-text-field density="compact" placeholder="값을 입력해주세요" v-model="model.budgetTotal" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
  </v-row>

  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">OC Project Name</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-text-field density="compact" placeholder="값을 입력해주세요" v-model="model.ocProjectName"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
  </v-row>

  <v-row>
    <v-col class="d-flex justify-start">
      <v-label class="mb-5">Boss Line Description</v-label>
    </v-col>
    <v-col class="d-flex justify-end">
      <v-text-field density="compact" placeholder="값을 입력해주세요" v-model="model.bossLineDescription"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
  </v-row>
</template>

<style scoped lang="css">
</style>
