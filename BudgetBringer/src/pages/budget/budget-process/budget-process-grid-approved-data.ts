import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseProcessApproved} from "../../../models/responses/process/process-approved/response-process-approved";

/**
 * 진생상황 BudgetProcessGridBusinessUnit 그리드 모델
 */
export class BudgetProcessGridProcessApproved extends CommonGridModel<ResponseProcessApproved>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<ResponseProcessApproved>;
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * Insert 그리드 사용여부
   */
  isUseInsert : boolean;



  /**
   * 생성자
   * @param date 전체 날짜 정보
   * @param year 년도 정보
   */
  constructor( date: string , year : number ) {
    super();
    this.columDefined = [
      // 비지니스유닛 명
      {
        field: "countryBusinessManagerName",
        headerName: date,
        headerClass: 'ag-grids-custom-header',
        sortable: false
      },
      {
        headerName:'Spending & PO issue Amount',
        field: "poIssueAmountSpending",
        headerClass: 'ag-grids-custom-header',
        valueFormatter: this.numberValueFormatter,
        width:250 ,
        sortable: false
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
    this.isUseInsert = false;
    this.items = [];
  }
}

