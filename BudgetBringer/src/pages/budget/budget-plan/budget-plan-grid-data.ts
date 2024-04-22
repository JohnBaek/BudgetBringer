import {CommonGridModel} from "../../../shared/grids/common-grid-model";

/**
 * 예산 그리드 모델
 */
export class BudgetPlanGridData extends CommonGridModel{
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
        filter: 'agTextColumnFilter',
        floatingFilter: true,
        width:130,
      },
      // 섹터
      {
        field: "sectorName",
        headerClass: 'ag-grids-custom-header',
        headerName:"Sector",
        floatingFilter: true,
        width:100,
        filter: 'agTextColumnFilter',
      },
      // 부서
      {
        field: "businessUnitName",
        headerClass: 'ag-grids-custom-header',
        headerName:"BU",
        width:100,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
      },
      // CC
      {
        field: "costCenterName",
        headerClass: 'ag-grids-custom-header',
        headerName:"CC"  ,
        width:100,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
      },
      // 국가별 매니저
      {
        field: "countryBusinessManagerName",
        headerClass: 'ag-grids-custom-header',
        headerName:"CBM"  ,
        width:130,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
      },
      // 설명
      {
        field: "description",
        headerClass: 'ag-grids-custom-header',
        headerName:"Description"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
      },
      {
        field: "ocProjectName",
        headerClass: 'ag-grids-custom-header',
        headerName:"OcProjectName"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
      },
      {
        field: "bossLineDescription",
        headerClass: 'ag-grids-custom-header',
        headerName:"BossLineDescription"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
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
  }
}

