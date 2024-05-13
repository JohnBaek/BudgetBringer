import {RequestQuery} from "../../models/requests/query/request-query";
import {toClone} from "../../services/utils/object-util";
import {CommonChartDefinitions} from "./common-grid-chart-definitions";
import CommonGridSkeletonRenderer from "./common-grid-skeleton-renderer.vue";

export type chartType = 'bar';

/**
 * CommonGrid Model
 */
export abstract class CommonGridModel {
  /**
   * ag-grid Column Definitions
   */
  public columDefined : any [];
  public columDefinedSkeleton : any [];
  /**
   * ag-chart Column Definitions
   */
  public chartDefined: any [];
  /**
   * Request Query Model
   */
  requestQuery : RequestQuery = new RequestQuery("",0,100);
  /**
   * Number Formatter.
   * Uses in ag-grid Column Renderer
   * @param params
   */
  numberValueFormatter = (params) => {
    // 넘버형식인 경우
    if( typeof params.value === 'number')
      return new Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(params.value);

    // 그 외
    return 0;
  }
  /**
   * Set Skeleton object from Column definitions
   */
  setSkeleton = () => {
    this.columDefinedSkeleton = toClone(this.columDefined);
    this.columDefinedSkeleton.forEach(item => {
      item.cellRenderer = CommonGridSkeletonRenderer
    });
  }
  /**
   * Set chart definition by Column Definitions
   * @param chartType chart Types
   * @param chartXKey X axis Key
   */
  setChart = (chartType: chartType , chartXKey: string) => {
    // Clear chart
    this.chartDefined = [];
    // Process all columns only flag isUseInChart is true
    this.columDefined.filter(i => i.isUseInChart).forEach(item => {
      // Add chart
      this.chartDefined.push(
        CommonChartDefinitions.createChartLegend(chartType, chartXKey, item.field, item.headerName)
      );
    });
  }
}
