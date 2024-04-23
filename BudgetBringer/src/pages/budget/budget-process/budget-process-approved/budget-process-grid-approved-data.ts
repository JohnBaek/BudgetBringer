import {CommonGridModel} from "../../../../shared/grids/common-grid-model";
import {RequestQuery} from "../../../../models/requests/query/request-query";
import CommonGridSkeletonRenderer from "../../../../shared/grids/common-grid-skeleton-renderer.vue";

/**
 * 진생상황 BudgetProcessGridBusinessUnit 그리드 모델
 */
export class BudgetProcessGridProcessApproved extends CommonGridModel{
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
   * @param year
   * @param requestQuery
   */
  constructor( date: string , year : number ,  requestQuery: RequestQuery  ) {
    super();
    this.date = date;
    this.year = year;
    this.requestQuery = requestQuery;
    this.columDefined = [
      {
        field: "countryBusinessManagerName",
        headerName: date,
        headerClass: 'ag-grids-custom-header',
        sortable: false,
      },
      {
        headerName:'Spending & PO issue Amount',
        field: "poIssueAmountSpending",
        headerClass: 'ag-grids-custom-header',
        valueFormatter: this.numberValueFormatter,
        width:250 ,
        sortable: false,
      },
      {
        headerName:'PO issue Amount',
        field: "poIssueAmount",
        headerClass: 'ag-grids-custom-header',
        valueFormatter: this.numberValueFormatter,
        width:250 ,
        sortable: false
      },
      {
        headerName:'Not PO issue Amount',
        field: "notPoIssueAmount",
        headerClass: 'ag-grids-custom-header',
        valueFormatter: this.numberValueFormatter,
        width:250 ,
        sortable: false
      },
      {
        headerName:'ApprovedAmount',
        field: "approvedAmount",
        headerClass: 'ag-grids-custom-header',
        valueFormatter: this.numberValueFormatter,
        width:250 ,
        sortable: false
      },
    ]
    this.columDefinedSkeleton = JSON.parse(JSON.stringify(this.columDefined));
    this.columDefinedSkeleton.forEach(item => {
      item.cellRenderer = CommonGridSkeletonRenderer
    });
  }
}

