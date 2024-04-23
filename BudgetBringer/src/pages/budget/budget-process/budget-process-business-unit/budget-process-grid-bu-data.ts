import {CommonGridModel} from "../../../../shared/grids/common-grid-model";
import {RequestQuery} from "../../../../models/requests/query/request-query";
import CommonGridSkeletonRenderer from "../../../../shared/grids/common-grid-skeleton-renderer.vue";

/**
 * 진생상황 BudgetProcessGridBusinessUnit 그리드 모델
 */
export class BudgetProcessGridBusinessUnit extends CommonGridModel{
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * Date
   */
  public date: string;
  /**
   * Year of Date
   */
  public year : number;
  /**
   * 생성자
   * @param date 전체 날짜 정보
   * @param year 년도 정보
   * @param requestQuery 요청정보
   */
  constructor( date: string , year : number ,  requestQuery: RequestQuery  ) {
    super();
    this.date = date;
    this.year = year;
    this.requestQuery = requestQuery;
    this.columDefined = [
      // 비지니스유닛 명
      {
        field: "businessUnitName",
        headerName: date,
        headerClass: 'ag-grids-custom-header',
      },
      {
        headerName:`${year.toString()}FY` ,
        headerClass: 'ag-grids-custom-header',
        children: [
          {
            headerName:'BudgetYear',
            field: "budgetYear",
            headerClass: 'ag-grids-custom-header',
            valueFormatter: this.numberValueFormatter,
          }
        ]
      },
      // 승인 예산
      {
        headerName:`${year.toString()}&${(year-1).toString()} FY` ,
        headerClass: 'ag-grids-custom-header',
        children: [
          {
            field: "budgetApprovedYearSum",
            headerClass: 'ag-grids-custom-header',
            headerName:'ApprovedAmount',
            valueFormatter: this.numberValueFormatter,
          }
        ]
      },
      // 남은 예산
      {
        headerName:`${year.toString()}FY` ,
        headerClass: 'ag-grids-custom-header',
        children: [
          {
            field: "budgetRemainingYear",
            headerClass: 'ag-grids-custom-header',
            headerName:'RemainingYear',
            valueFormatter: this.numberValueFormatter,
          }
        ]
      },
    ];
    this.columDefinedSkeleton = JSON.parse(JSON.stringify(this.columDefined));
    this.columDefinedSkeleton.forEach(item => {
      item.cellRenderer = CommonGridSkeletonRenderer;
      item.children?.forEach(child => {
        child.cellRenderer = CommonGridSkeletonRenderer;
      })
    });
    this.chartDefined = [
      {   type: 'bar'
        , xKey: 'businessUnitName'
        , yKey: 'budgetYear'
        , yName: 'BudgetYear'
        , tooltip: {
          renderer: function ({ datum, xKey, yKey }) {
            return {
              content: Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(datum[yKey]) ,
              title: datum[xKey]
            };
          },
        },
      },
      {   type: 'bar'
        , xKey: 'businessUnitName'
        , yKey: 'budgetApprovedYearSum'
        , yName: 'ApprovedAmount'
        , tooltip: {
          renderer: function ({ datum, xKey, yKey }) {
            return {
              content: Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(datum[yKey]) ,
              title: datum[xKey]
            };
          },
        },
      },
      {   type: 'bar'
        , xKey: 'businessUnitName'
        , yKey: 'budgetRemainingYear'
        , yName: 'RemainingYear'
        , tooltip: {
          renderer: function ({ datum, xKey, yKey }) {
            return {
              content: Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(datum[yKey]) ,
              title: datum[xKey]
            };
          },
        },
      },
    ]
  }
}

