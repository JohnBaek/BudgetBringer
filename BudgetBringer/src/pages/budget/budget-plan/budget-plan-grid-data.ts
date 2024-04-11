import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseBudgetPlan} from "../../../models/responses/budgets/response-budget-plan";

/**
 * 예산 그리드 모델
 */
export class BudgetPlanGridData extends CommonGridModel<ResponseBudgetPlan>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : ResponseBudgetPlan[];
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
   */
  constructor() {
    super();
    this.columDefined = [
      // 승인일
      {
        field: "approvalDate",
        headerClass: 'ag-grids-custom-header',
        headerName:"Approval Date" ,
        showDisabledCheckboxes: true,
        width:130,
      },
      // 섹터
      {
        field: "sectorName",
        headerClass: 'ag-grids-custom-header',
        headerName:"Sector",
        width:100,
      },
      // 부서
      {
        field: "businessUnitName",
        headerClass: 'ag-grids-custom-header',
        headerName:"BU",
        width:100,
      },
      // CC
      {
        field: "costCenterName",
        headerClass: 'ag-grids-custom-header',
        headerName:"CC"  ,
        width:100,
      },
      // 국가별 매니저
      {
        field: "countryBusinessManagerName",
        headerClass: 'ag-grids-custom-header',
        headerName:"CBM"  ,
        width:130,
      },
      // 설명
      {
        field: "description",
        headerClass: 'ag-grids-custom-header',
        headerName:"Description"  ,
        width:200,
      },
      // 예산
      {
        field: "budgetTotal",
        headerClass: 'ag-grids-custom-header',
        headerName:"FvBudget"  ,
        valueFormatter: this.numberValueFormatter,
      },
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}

