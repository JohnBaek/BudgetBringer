<script setup lang="ts">
import {onMounted, ref} from "vue";
import {ResponseProcessSummaryDetail} from "../../../models/responses/process/response-process-summary-detail";
import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {ResponseData} from "../../../models/responses/response-data";
import {AgGridVue} from "ag-grid-vue3";
import {CommonGridButtonGroupDefinesButtonEmits} from "../../../shared/grids/common-grid-button-group-defines";
import {getDateFormatForFile} from "../../../services/utils/date-util";
import CommonGridButtonGroup from "../../../shared/grids/common-grid-button-group.vue";
import {AgChartsVue} from 'ag-charts-vue3';
import {exportPdfFile} from "../../../services/utils/pdf-util";
import {CommonButtonDefinitions} from "../../../shared/grids/common-grid-button";

/**
 * From the parent.
 */
interface Props {
  // Grid Model
  gridModel: CommonGridModel;
  // Excel Title
  excelTitle: string;
}
const props = defineProps<Props>();
/**
 * Current selected rows list.
 */
const selectedRows = ref([]);
/**
 * Defines dispatches.
 */
const emits = defineEmits<CommonGridButtonGroupDefinesButtonEmits>();
/**
 * Grid items ( Response items from server )
 */
const items  = ref([]);
/**
 * Grid API
 */
const gridApi = ref();
/**
 * Grid Column API
 */
const gridColumnApi = ref(null);
/**
 * 그리드 파라미터
 */
const gridParams = ref(null);
/**
 * 새로고침 명령
 */
const refresh = () => {
  emits('onRefresh');
  items.value = [];
  loadData();
}

/**
 * Request to server for excel
 */
const exportExcel = () => {
  communicationService.inTransmission();

  // Request to Server
  HttpService.requestGetFile(`${props.gridModel.requestQuery.apiUri}/export/excel` , props.gridModel.requestQuery).subscribe({
    next(response) {
      if(response == null)
        return;

      // Create URL dummy link
      const url = window.URL.createObjectURL(response);

      // Create Anchor dummy
      const link = document.createElement('a');

      // Simulate Click
      link.href = url;
      link.setAttribute('download', `${getDateFormatForFile()}_${props.excelTitle}.xlsx`);
      document.body.appendChild(link);
      link.click();

      // Remove Dummy
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    },
    error(err) {
      console.error('Error loading data', err);
    },
    complete() {
      setTimeout(() => {
        communicationService.offTransmission();
      },2000)
    },
  });
}
/**
 * gridReady 이벤트 핸들러
 * @param params 파라미터
 */
const onGridReady = (params) => {
  gridApi.value = params.api;
  gridColumnApi.value = params.columnApi;
  gridParams.value = params;
  calculateSums();
};
/**
 * 마운트 핸들링
 */
onMounted(() => {
  // // Clone
  // skeletonColumnDefined.value = props.gridModel.columDefined.slice();
  // skeletonColumnDefined.value.forEach(i => i.cellRenderer = "CommonGridCellRendererSkeleton");
  loadData();
});
/**
 * Set Sum Columns to Grid
 */
const calculateSums = () => {
  // Does not have gridAPI
  if (!gridApi.value)
    return;

  // Get list from grid
  const rowData = [];
  gridApi.value.forEachNode(node => {
    rowData.push(node.data);
  });

  // Get column states
  const columnStates = gridApi.value.getColumnState();
  const sums = [];

  // Is not ready
  if(!columnStates)
    return;

  // Process all columns
  for (let i=0; i<columnStates.length; i++) {
    // Get Column
    const colState = columnStates[i];

    // First column
    if(i == 0) {
      sums[colState.colId] = '합계';
      continue;
    }

    // Sum to all values
    sums[colState.colId] = rowData.reduce((acc, row) => {
      const value = row[colState.colId];
      return acc + (typeof value === 'number' ? value : 0);
    }, 0);
  }
  gridApi.value.setGridOption('pinnedTopRowData', [sums])
}
const inCommunication = ref(true);
communicationService.subscribeCommunication().subscribe((communication) =>{
  inCommunication.value = communication;
});

/**
 * 데이터를 로드한다.
 */
const loadData = () => {
  // Request to Server
  HttpService.requestGetAutoNotify<ResponseData<ResponseProcessSummaryDetail<any>>>(props.gridModel.requestQuery.apiUri).subscribe({
    async next(response) {
      // Update list
      items.value = response.data.items;

      // Compute sums
      setTimeout(() => {calculateSums();},100);
    }
  });
}
/**
 * Change to Chart
 */
const toChart = () =>{
  showGrid.value = false;
  options.value = [];

  // Process all items
  for (let item of items.value){
    options.value.push({
      data: item.items,
      title:  { text: item.title },
      series: props.gridModel.chartDefined,
      height: 600 ,
      axes : [
        {
          type: "category",
          position: "bottom",
          gridLine: {
            style: [
              {
                stroke: "rgba(219, 219, 219, 1)",
                lineDash: [4, 2],
              },
            ],
          },
        },
        {
          type: "number",
          position: "left",
          label: {
            formatter: function(params) {
              return Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(params.value);
            }
          },
          gridLine: {
            style: [
              {
                stroke: "rgba(219, 219, 219, 1)",
                lineDash: [4, 2],
              },
            ],
          },
        },
      ]
    });
  }
  console.log('options',options);
}
/**
 * Export PDF FILE
 */
const exportPDF =  () => {
  communicationService.inTransmission();
  try {
    // Store Previous Style
    const storeState = showGridDetail.value;
    showGridDetail.value = true;
    applyGridStyle();

    setTimeout(async () => {
      // Export PDF
      await exportPdfFile("capture-area", props.excelTitle);
      communicationService.offTransmission();

      // Recover Previous Grid Detail
      showGridDetail.value = storeState;
      applyGridStyle();
    }, 2000);
  }catch (e) {
    console.error(e);
    communicationService.offTransmission();
  }
}
/**
 * Change to Grid
 */
const toGrid = () => {
  showGrid.value = true;
}
const showGrid = ref(true);
const options = ref([]);
const gridStyle = ref('width: 100%; height: 90px;');
const showGridDetail = ref(false);
const toggleDetail = () => {
  showGridDetail.value = !showGridDetail.value
  applyGridStyle();
};
const applyGridStyle = () => {
  gridStyle.value = (showGridDetail.value) ? 'width: 100%; height: 600px;' : 'width: 100%; height: 90px;';
}
const showButtons = [
  CommonButtonDefinitions.exportExcel,
  CommonButtonDefinitions.exportPDF,
  CommonButtonDefinitions.refresh,
  CommonButtonDefinitions.toChart,
];

</script>

<template>
  <v-row class="mt-1 mb-1">
    <v-col>
      <div class="mt-2">
        <!-- Action Buttons -->
        <common-grid-button-group
          :selected-rows="selectedRows"
          :showButtons="showButtons"
          @on-refresh="refresh()"
          @on-export-excel="exportExcel()"
          @exportPdf="exportPDF()"
          @chart="toChart()"
          @grid="toGrid()"
        >
        </common-grid-button-group>
      </div>
    </v-col>
  </v-row>

  <v-label style="cursor: pointer;" @click="toggleDetail" :class="showGridDetail ? 'text-grey' : 'text-blue'"><b>{{ showGridDetail ? '합계 정보 보기' : '상세 정보 보기'}}</b></v-label>
  <div id="capture-area">
    <!--Skeleton-->
    <div v-show="showGrid">
      <div v-show="inCommunication">
        <div v-for="item in [1,2,3]" :key="item" class="mb-5" >
          <SkeletonLoader  :width="70" :height="30" />
          <v-spacer></v-spacer>
          <ag-grid-vue
            :style="gridStyle"
            :columnDefs="(props.gridModel as CommonGridModel).columDefinedSkeleton"
            :rowData="[1,2,3,5,6,7,8,9,10,11,12,13,14,15,16]"
            class="ag-theme-alpine"
          >
          </ag-grid-vue>
        </div>
      </div>

      <div v-show="!inCommunication">
        <div v-for="item in items" :key="item.sequence" class="mb-5">
          <h3>{{item.title}}</h3>
          <v-spacer></v-spacer>
          <ag-grid-vue
            :style="gridStyle"
            @grid-ready="onGridReady"
            :columnDefs="(props.gridModel as CommonGridModel).columDefined"
            :rowData="item.items"
            :pinnedTopRowData="item.total"
            class="ag-theme-alpine"
          >
          </ag-grid-vue>
        </div>
      </div>
    </div>

    <!--ChartMode-->
    <div v-show="!showGrid">
      <div v-for="item in options" :key="item.sequence" class="mb-5" style="border:1px solid black;">
        <ag-charts-vue :options="item" />
      </div>
    </div>
  </div>
</template>

<style scoped lang="css">
</style>
