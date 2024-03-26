import {CommonGridModel} from "../../../shared/grid/common-grid-model";

/**
 * 예산 그리드 모델
 */
export class BudgetPlanGridData extends CommonGridModel<BudgetGridRowDataModel>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<BudgetGridRowDataModel>;
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
        field: "ApprovalDate",
        headerClass: 'ag-grid-custom-header',
        headerName:"Approval Date" ,
        showDisabledCheckboxes: true,
        width:130,
      },
      // 섹터
      {
        field: "Sector",
        headerClass: 'ag-grid-custom-header',
        headerName:"Sector",
        width:100,
      },
      // 부서
      {
        field: "Bu",
        headerClass: 'ag-grid-custom-header',
        headerName:"BU",
        width:100,
      },
      // CC
      {
        field: "Cc",
        headerClass: 'ag-grid-custom-header',
        headerName:"CC"  ,
        width:100,
      },
      // 국가별 매니저
      {
        field: "Cbm",
        headerClass: 'ag-grid-custom-header',
        headerName:"CBM"  ,
        width:130,
      },
      // 설명
      {
        field: "Description",
        headerClass: 'ag-grid-custom-header',
        headerName:"Description"  ,
        width:500,
      },
      // 예산
      {
        field: "FvBudget",
        headerClass: 'ag-grid-custom-header',
        headerName:"FvBudget"  ,
        valueFormatter: this.numberValueFormatter,
      },
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}

/**
 * 예산 그리드 데이터 모델
 */
export class BudgetGridRowDataModel {
  /**
   * 승인일 ( 텍스트 형태도 가능 )
   */
  ApprovalDate: string;
  /**
   * 섹터코드
   */
  Sector:string;
  /**
   * 부서코드
   */
  Bu:string;
  /**
   * CC 코드
   */
  Cc:string;
  /**
   * Country Business Manager
   */
  Cbm:string;
  /**
   * 설명
   */
  Description: string;
  /**
   * 예산
   */
  FvBudget:number;
}

