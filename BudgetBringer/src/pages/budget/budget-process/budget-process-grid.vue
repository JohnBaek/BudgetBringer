<script setup lang="ts">
import {onMounted, ref} from "vue";
import {ResponseProcessSummaryDetail} from "../../../models/responses/process/response-process-summary-detail";
import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {ResponseData} from "../../../models/responses/response-data";
import {messageService} from "../../../services/message-service";
import {AgGridVue} from "ag-grid-vue3";
import {AgChartsVue} from 'ag-charts-vue3';
import CommonGridButtonGroup from "../../../shared/grids/common-grid-button-group.vue";
import {CommonGridButtonGroupDefinesButtonEmits} from "../../../shared/grids/common-grid-button-group-defines";
import {getDateFormatForFile} from "../../../services/utils/date-util";
import {exportPdfFile} from "../../../services/utils/pdf-util";

/**
 * From the parent.
 */
interface Props {
  // Options for grid
  gridOptions?: Record<string, any>;
  // Grid Model
  gridModel: CommonGridModel;
  // Excel Title
  excelTitle: string;
  yearList : {Type: []}
  year: string
}
const props = defineProps<Props>();
const year = ref(props.year);
const gridModel = ref(props.gridModel);
let requestQuery = props.gridModel.requestQuery;
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

  const yearValue = year.value;

  // Request to Server
  HttpService.requestGetFile(`${props.gridModel.requestQuery.apiUri}/export/excel/${yearValue}` , requestQuery).subscribe({
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
      // 커뮤니케이션 시작
      communicationService.offTransmission();
    },
  });
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
  gridApi.value.forEachNode(node => rowData.push(node.data));

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

  if(sums['budgetYear'] && sums['remainingYear'])
    sums['ratio'] = (sums['remainingYear'] / sums['budgetYear']) * 100;

  gridApi.value.setGridOption('pinnedTopRowData', [sums])
}
/**
 * 데이터를 로드한다.
 */
const loadData = () => {
  let params = {
    year: year.value ,
  };

  // When Year Conditions changes , Change Year Header Name
  for(let item of gridModel.value.columDefined) {
    // Is Invalid
    if(!item.headerName || ( item.headerName.indexOf('FY') === -1) )
      continue;

    // Get Header Name
    const headerName = item.headerName.split('FY')[1];

    // Change Year Name of Header
    item.headerName = `${year.value}FY${headerName}`
  }

  // Clear Items
  items.value = [];

  communicationService.notifyInCommunication();
  // Request to Server
  HttpService.requestGet<ResponseData<ResponseProcessSummaryDetail<any>>>(
      props.gridModel.requestQuery.apiUri , params).subscribe({
    async next(response) {
      // Failed Request
      if (response.error) {
        messageService.showError(`[${response.code}] ${response.message}`);
        return;
      }
      // Update list
      items.value = response.data.items.slice();
      // Compute sums
      setTimeout(() => { calculateSums(); },100);
    } ,
    error(err) {
      messageService.showError('Error loading data'+err);
    } ,
    complete() {
      communicationService.notifyOffCommunication();
    },
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
}
/**
 * Change to Grid
 */
const toGrid = () => {
  showGrid.value = true;
}
const options = ref([]);
const inCommunication = ref(true);
communicationService.subscribeCommunication().subscribe((communication) =>{
  inCommunication.value = communication;
});

const showGrid = ref(true);
const gridStyle = ref('width: 100%; height: 90px;');
const showGridDetail = ref(false);
const toggleDetail = () => {
  showGridDetail.value = !showGridDetail.value
  applyGridStyle();
};
const applyGridStyle = () => {
  if(showGridDetail.value)
    gridStyle.value = 'width: 100%; height: 600px;';
  else
    gridStyle.value = 'width: 100%; height: 90px;';
}

</script>

<template>
  <v-row class="mt-1 mb-1">
    <v-col>
      <div class="mt-2">
        <!-- Action Buttons -->
        <common-grid-button-group
          :selected-rows="selectedRows"
          :showButtons="['refresh', 'excel' , 'pdf', 'chart']"
          @on-refresh="refresh()"
          @on-export-excel="exportExcel()"
          @exportPdf="exportPDF()"
          @chart="toChart()"
          @grid="toGrid()"
        />
      </div>
    </v-col>

  </v-row>

  <v-row>
    <v-col sm="12" md="6" lg="4">
      <v-select variant="outlined" :items="props.yearList" v-model="year" label="년도선택" @update:modelValue="loadData" >년도 선택</v-select>
    </v-col>
  </v-row>

  <v-label style="cursor: pointer;" @click="toggleDetail" :class="showGridDetail ? 'text-grey' : 'text-blue'"><b>{{ showGridDetail ? '합계 정보 보기' : '상세 정보 보기'}}</b></v-label>
  <div id="capture-area" >

    <!--GridMode-->
    <div v-show="showGrid">
      <div v-show="inCommunication">
        <div v-for="item in [1,2,3]" :key="item" class="mb-5" >
          <h3> <SkeletonLoader  :width="70" :height="30" /></h3>
          <v-spacer></v-spacer>
          <ag-grid-vue
            :style="gridStyle"
            :columnDefs="(props.gridModel as CommonGridModel).columDefinedSkeleton"
            :rowData="[1,2,3,5,6,7,8]"
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
            :grid-options="props.gridOptions"
            :columnDefs="gridModel.columDefined"
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
