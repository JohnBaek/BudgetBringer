<script setup lang="ts">
import { CommonGridColumn, CommonGridTextFilters } from '@/components/common/grid/CommonGridColumn'
import type { ColumnFilterModelType } from 'primevue/column'

// Props
const props = defineProps({
  column: { type: CommonGridColumn, required: true }
})
/**
 * When do filtering
 * @param model
 */
const changeFilter = (model: ColumnFilterModelType) => {
  console.log('changeFilter',model)
}
</script>

<template>
  <Column :field="'baseYearForStatistics'"  :filterMatchModeOptions="CommonGridTextFilters" :header="'통계'" >
    <template #body="{ data }">
      {{ data['baseYearForStatistics'] }}
    </template>
  </Column>
<!--  <Column :field="column.field"  :filterMatchModeOptions="CommonGridTextFilters" :header="column.header" >-->
<!--    <template #body="{ data }">-->
<!--      {{ data[column.field] }}-->
<!--    </template>-->
<!--    <template #filter="{ filterModel }">-->
<!--      <InputText v-model="filterModel.value" type="text" @keyup.enter="changeFilter(filterModel)" class="p-column-filter" :placeholder="`${column.header} 검색`" />-->
<!--    </template>-->
<!--  </Column>-->
</template>

<style scoped>
</style>