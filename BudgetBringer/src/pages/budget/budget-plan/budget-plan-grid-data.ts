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
        headerName:"Approval Date" ,
        editable: true
      },
      // 섹터
      {
        field: "Sector",
        headerName:"Sector",
        editable: true,
        cellEditor: "agSelectCellEditor",
        cellEditorParams: {
          values: [
            10
            ,500
            ,20
            ,10
          ]
        }
      },
      // 부서
      {
        field: "Bu",
        headerName:"BU",
        editable: true,
        cellEditor: "agSelectCellEditor",
        cellEditorParams: {
          values: [
            'H&N',
            'NR'
          ]
        }
      },
      // CC
      {
        field: "Cc",
        headerName:"CC"  ,
        editable: true,
        cellEditor: "agSelectCellEditor",
        cellEditorParams: {
          values: [
            1100,
            2000,
            3200
          ]
        }
      },
      // 국가별 매니저
      {
        field: "Cbm",
        headerName:"CBM"  ,
        editable: true,
        cellEditor: "agSelectCellEditor",
        cellEditorParams: {
          values: [
            'Yuri Hong',
            'BC Hong'
          ]
        }
      },
      // 설명
      {
        field: "Description",
        headerName:"Description"  ,
        editable: true},
      // 예산
      {
        field: "FvBudget",
        headerName:"FvBudget"  ,
        pinned: "right",
        valueFormatter: this.numberValueFormatter,
        editable: true
      },
    ]
    this.isUseInsert = true;
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
  Sector:number;
  /**
   * 부서코드
   */
  Bu:string;
  /**
   * CC 코드
   */
  Cc:number;
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

