
<template>
  <div style="display: flex; flex-direction: column; height: 900px">
    <div style="width: 100%; flex: 1 1 auto;">
      <ag-grid-vue :style="{width, height}"
                   :class="themeClass"
                   :columnDefs="colDefs"
                   :rowData="rowData"
                   :defaultColDef="defaultColDef"
      ></ag-grid-vue>
    </div>
  </div>
</template>

<style scoped>
</style>

<script setup="ts">
class Test {
  ApprovalDate;
  Sector;
  Bu;
  Cc;
  Cbm;
  Description;
  FvBudget ;
  IsBelow_500_k = false;
}

import {AgGridVue} from "ag-grid-vue3";
import {ref} from "vue";
import { ClientSideRowModelModule } from "@ag-grid-community/client-side-row-model";
import { ModuleRegistry} from '@ag-grid-community/core';
ModuleRegistry.registerModules([ClientSideRowModelModule]);

/**
 * ag-grid 높이
 */
const height = '100%';

/**
 * ag-grid 너비
 */
const width = '100%';

/**
 * ag-grid 테마
 */
const themeClass = "ag-theme-quartz";

// TODO 테스트 데이터
let test = new Test();
test.ApprovalDate = '2024 품의예정';
test.Sector = 10;
test.Bu = 'H&N';
test.Cc = 1100;
test.Cbm = 'Yuri Hong';
test.Description = '수은분석기';
test.FvBudget = 60000;
test.IsBelow_500_k = false;

/**
 * ag-Grid 로우데이터
 * @type
 */
const rowData = ref([
]);

// TODO 로우데이터를 임의로 추가한다.
for (let i=0; i<1; i++)
  rowData.value.push(test);


const defaultColDef = ref({
  width: 200,
  editable: true,
});

const numberValueFormatter = (params) => {
  return new Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(params.value);
};

const colDefs = ref([
  { field: "IsBelow_500_k"  , headerName:"500K Below" , width:115,  editable: true ,
    cellEditor: "agCheckboxCellEditor",
  },
  { field: "ApprovalDate"  , headerName:"Approval Date" ,  editable: true
  },
  {
    field: "Sector",
    headerName:"Sector",
    editable: true,
    cellEditor: "agSelectCellEditor",
    cellEditorParams: {
      values: [
        10
        ,500
        ,20
        ,10
      ]
    }
  },
  { field: "Bu", headerName:"BU"  ,  editable: true,
    cellEditor: "agSelectCellEditor",
    cellEditorParams: {
      values: [
        'H&N',
        'NR'
      ]
    }
  },
  { field: "Cc", headerName:"CC"  ,  editable: true,
    cellEditor: "agSelectCellEditor",
    cellEditorParams: {
      values: [
        1100,
        2000,
        3200
      ]
    }
  },
  { field: "Cbm", headerName:"CBM"  ,  editable: true,
    cellEditor: "agSelectCellEditor",
    cellEditorParams: {
      values: [
        'Yuri Hong',
        'BC Hong'
      ]
    }
  },
  { field: "Description", headerName:"Description"  ,  editable: true},
  {
    field: "FvBudget", headerName:"FvBudget"  , pinned: "right", valueFormatter: numberValueFormatter , editable: true
  },
]);
</script>
