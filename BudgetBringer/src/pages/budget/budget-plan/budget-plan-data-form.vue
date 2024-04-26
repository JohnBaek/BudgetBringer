<script setup lang="ts">
import {onMounted, ref} from "vue";
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
let tempBusinessUnitId = '';
onMounted(() => {
  if(model.value.businessUnitId !== '') {
    tempBusinessUnitId = model.value.businessUnitId;
    model.value.businessUnitId = '';
  }
})

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
  if(tempBusinessUnitId !== '') {
    // Try find Business Units in countryBusinessManager
    const _countryBusinessManagers = items.filter(i => i.id === model.value.countryBusinessManagerId);

    console.log('_countryBusinessManagers',_countryBusinessManagers);

    // Reset Business Units
    businessUnits.value = [];

    // Found Business Units ?
    if(_countryBusinessManagers.length > 0) {
      businessUnits.value = _countryBusinessManagers[0].businessUnits;
    }

    model.value.businessUnitId = tempBusinessUnitId;
    tempBusinessUnitId = '';
  }
}
/**
 * When countryBusinessManager selected
 * @param countryBusinessManagerId
 */
const onChangeCountryBusinessManager = (countryBusinessManagerId: any) => {
  // Reset
  model.value.countryBusinessManagerId = countryBusinessManagerId;
  model.value.businessUnitId = null;

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
  <v-switch v-model="model.isIncludeInStatistics" color="primary" required :label="model.isIncludeInStatistics ? '통계에 포함': '통계에 미포함'"></v-switch>
  <common-select required label="Country Businesses Manager" v-model="model.countryBusinessManagerId" @onChange="onChangeCountryBusinessManager" @onDataUpdated="onCountryBusinessManagerUpdated" title="name" value="id" requestApiUri="/api/v1/CountryBusinessManager" />
  <v-select
    density="compact" label="Business Unit" variant="outlined" outlined required
    v-model="model.businessUnitId" placeholder="값을 선택해주세요" item-title="name" item-value="id" :items="businessUnits" :disabled="businessUnits.length === 0"></v-select>
  <common-select required label="Cost Center" v-model="model.costCenterId" title="value" value="id"  requestApiUri="/api/v1/CostCenter"/>
  <common-select required label="Sector" v-model="model.sectorId" title="value" value="id"  requestApiUri="/api/v1/Sector" />
  <v-text-field density="compact" label="Approval Date" placeholder="값을 입력해주세요" required v-model="model.approvalDate"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
  <v-text-field density="compact" placeholder="값을 입력해주세요" label="Description" v-model="model.description"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
  <v-text-field density="compact" placeholder="값을 입력해주세요"
                type="number"
                label="Budget Total"
                v-model="model.budgetTotal" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    <v-text-field density="compact" label="OC PROJECT Name" placeholder="값을 입력해주세요" v-model="model.ocProjectName"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    <v-text-field density="compact" label='Boss Line Description' placeholder="값을 입력해주세요" v-model="model.bossLineDescription"  variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
</template>

<style scoped lang="css">
</style>
