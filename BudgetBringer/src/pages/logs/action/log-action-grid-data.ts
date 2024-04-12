import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseBudgetApproved} from "../../../models/responses/budgets/response-budget-approved";
import {EnumApprovalStatus} from "../../../models/enums/enum-approval-status";

/**
 * 예산 그리드 모델
 */
export class LogActionGridData extends CommonGridModel<ResponseBudgetApproved>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<ResponseBudgetApproved>;
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * Insert 그리드 사용여부
   */
  isUseInsert : boolean = false;

  /**
   * 생성자
   */
  constructor() {
    super();
    this.columDefined = [
      {
        field: "category",
        headerClass: 'ag-grids-custom-header',
        headerName:"카테고리" ,
        showDisabledCheckboxes: true,
        filter: 'agTextColumnFilter',
        floatingFilter: true,
        width:145,
      },
      {
        field: "contents",
        headerClass: 'ag-grids-custom-header',
        headerName:"내용"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:700,
      },
      {
        field: "regName",
        headerClass: 'ag-grids-custom-header',
        headerName:"등록자"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:145,

      },
      {
        field: "regDate",
        headerClass: 'ag-grids-custom-header',
        headerName:"등록일"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        width:145,
        valueFormatter: function(params) {
          if (params.value) {
            const date = new Date(params.value);
            return date.toLocaleString('ko-KR', {
              year: 'numeric',
              month: '2-digit',
              day: '2-digit',
              hour: '2-digit',
              minute: '2-digit',
              second: '2-digit',
            }).replace(/(\. )|(\.,)/g, ' ');
          }
          return null;
        }
      },
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}
