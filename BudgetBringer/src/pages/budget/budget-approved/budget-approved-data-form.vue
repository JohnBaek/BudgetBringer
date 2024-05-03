<script setup lang="ts">

import CommonSelect from "../../../shared/common-select.vue";
import {RequestBudgetApproved} from "../../../models/requests/budgets/request-budget-approved";
import {onMounted, ref, watch} from "vue";
import {ApprovalStatusDescriptions} from "../../../models/enums/enum-approval-status";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";
import CommonFileUpload from "../../../shared/common-file-upload.vue";
import {ResponseBusinessUnit} from "../../../models/responses/budgets/response-business-unit";
/**
 * props
 */
const props = defineProps({
  modelValue: { type: RequestBudgetApproved, required: true } ,
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
  (e: 'update:data', RequestBudgetApproved) : RequestBudgetApproved ,
}>();

/**
 * Copy already Uploaded files
 */
const uploadedFiles = ref([]);

// Update Uploaded Files
uploadedFiles.value = JSON.parse(JSON.stringify(model.value.attachedFiles));
console.log('form',uploadedFiles.value)
model.value.attachedFiles = [];

/**
 * Watch
 */
watch(model, (newValue) => {
  emits("update:data", newValue);
}, { deep: true });
/**
 * Status of Budget Approveds
 */
const statusOptions = ref();
/**
 * Mount
 */
onMounted(() => {
  statusOptions.value = Object.entries(ApprovalStatusDescriptions).map(([status, description]) => ({
    status: parseInt(status),
    description
  }));
});
// Country business Managers
let countryBusinessManagers : ResponseCountryBusinessManager [] = [];
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
/**
 * Dispatch Submit
 */
const dispatchSubmit = () => {
  emits('submit');
}
let tempBusinessUnitId = '';
onMounted(() => {
  if(model.value.businessUnitId !== '') {
    tempBusinessUnitId = model.value.businessUnitId;
    model.value.businessUnitId = '';
  }
})
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
</script>

<template>
  <v-row>
    <v-col cols="12" md="6">
      <common-select required label="Country Businesses Manager" v-model="model.countryBusinessManagerId" @onChange="onChangeCountryBusinessManager" @onDataUpdated="onCountryBusinessManagerUpdated" title="name" value="id" requestApiUri="/api/v1/CountryBusinessManager" />
    </v-col>
    <v-col cols="12" md="6">
      <v-select density="compact" v-model="model.approvalStatus" :items="statusOptions" item-value="status" item-title="description" label="승인 상태"></v-select>
    </v-col>
    <v-col cols="12" md="6">
      <v-select density="compact" label="Business Unit" variant="outlined" outlined required v-model="model.businessUnitId" placeholder="값을 선택해주세요" item-title="name" item-value="id" :items="businessUnits" :disabled="businessUnits.length === 0"></v-select>
    </v-col>
    <v-col cols="12" md="6">
      <common-select density="compact"  required v-model="model.costCenterId" title="value" value="id" label="Cost Center" requestApiUri="/api/v1/CostCenter" />
    </v-col>
    <v-col cols="12" md="6">
      <common-select required v-model="model.sectorId" title="value" value="id" label="Sector" requestApiUri="/api/v1/Sector" />
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact"  required v-model="model.approvalDate" label="Approval Date" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact"  v-model="model.poNumber" label="PoNumber" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact"  v-model="model.description" label="Description" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact" type="number"  v-model="model.actual" label="Actual" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact" type="number"  v-model="model.approvalAmount" label="ApprovalAmount" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact"  v-model="model.ocProjectName" label="OcProjectName" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="6">
      <v-text-field density="compact"  v-model="model.bossLineDescription" label="BossLine Description" variant="outlined" @keyup.enter="dispatchSubmit()"></v-text-field>
    </v-col>
    <v-col cols="12" md="12">
      <common-file-upload :uploaded-files="uploadedFiles" v-model="model.attachedFiles"></common-file-upload>
    </v-col>
  </v-row>
</template>

<style scoped lang="css">
</style>
