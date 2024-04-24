<script setup lang="ts">
import {onMounted, ref} from "vue";
import {ResponseProcessSummaryDetail} from "../../../models/responses/process/response-process-summary-detail";
import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {communicationService} from "../../../services/communication-service";
import {HttpService} from "../../../services/api-services/http-service";
import {ResponseData} from "../../../models/responses/response-data";
import {messageService} from "../../../services/message-service";
import {AgGridVue} from "ag-grid-vue3";
import { AgChartsVue } from 'ag-charts-vue3';
import CommonGridButtonGroup from "../../../shared/grids/common-grid-button-group.vue";
import {CommonGridButtonGroupDefinesButtonEmits} from "../../../shared/grids/common-grid-button-group-defines";
import {getDateFormatForFile} from "../../../services/utils/date-util";
import html2canvas from "html2canvas";
import { jsPDF } from "jspdf";


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
      // 커뮤니케이션 시작
      communicationService.offTransmission();
    },
  });
}
const exportPdf = async () => {
  communicationService.inTransmission();

  // Get Native Html elements
  const element = document.getElementById('capture-area') as HTMLElement;

  // Transform to canvas
  const canvas = await html2canvas(element, {
    onclone: function (clonedDoc) {
      clonedDoc.getElementById('capture-area').style.padding = '20px';
    }
  });

  const image = canvas.toDataURL('image/png').replace('image/png', 'image/octet-stream');

  // PDF 문서의 크기를 캔버스 크기에 맞춤
  const pdfWidth = canvas.width;
  const pdfHeight = canvas.height;

  // PDF 문서 생성 (단위를 'px'로 설정하여 픽셀 기반의 정확한 크기 조절 가능)
  const pdf = new jsPDF({
    orientation: 'p',
    unit: 'px',
    format: [pdfWidth, pdfHeight]
  });

  // 이미지를 PDF에 추가
  pdf.addImage(image, 'PNG', 0, 0, pdfWidth, pdfHeight);
  pdf.save(`${getDateFormatForFile()}_${props.excelTitle}.pdf`);
  communicationService.offTransmission();
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
  gridApi.value.setGridOption('pinnedBottomRowData', [sums])
}
/**
 * 데이터를 로드한다.
 */
const loadData = () => {
  communicationService.inCommunication();
  // Request to Server
  HttpService.requestGet<ResponseData<ResponseProcessSummaryDetail<any>>>(
      props.gridModel.requestQuery.apiUri).subscribe({
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
      communicationService.offCommunication();
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
          @exportPdf="exportPdf()"
          @chart="toChart()"
          @grid="toGrid()"
        />
      </div>
    </v-col>
  </v-row>
  <div id="capture-area" >
    <!--GridMode-->
    <div v-show="showGrid">
      <div v-show="inCommunication">
        <div v-for="item in [1,2,3]" :key="item" class="mb-5" >
          <h3> <SkeletonLoader  :width="70" :height="30" /></h3>
          <v-spacer></v-spacer>
          <ag-grid-vue
            style="width: 100%; height: 600px;"
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
            style="width: 100%; height: 600px;"
            @grid-ready="onGridReady"
            :grid-options="props.gridOptions"
            :columnDefs="(props.gridModel as CommonGridModel).columDefined"
            :rowData="item.items"
            :pinnedBottomRowData="item.total"
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
