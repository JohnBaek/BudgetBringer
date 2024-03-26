import {CommonGridModel} from "../../../shared/grid/common-grid-model";
import CommonGridRendererSkeleton from "../../../shared/grid/common-grid-renderer-skeleton.vue";

/**
 * 진생상황 P&L Owner 그리드 모델
 */
export class BudgetProcessGridPLOwner extends CommonGridModel<BudgetProcessGridPLOwnerDataModel>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<BudgetProcessGridPLOwnerDataModel>;
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
      // 날짜
      {
        field: "Date",
        headerName: date,
        headerClass: 'ag-grid-custom-header',
        width:"120",
        cellRendererFramework: CommonGridRendererSkeleton,
      },
      // 예산
      {
        field: "Currency",
        headerName:`KRW` ,
        headerClass: 'ag-grid-custom-header',
        width:"80",
        cellRendererFramework: CommonGridRendererSkeleton,
      },
      // 예산
      {
        headerName:`${year.toString()}FY` ,
        headerClass: 'ag-grid-custom-header',
        children: [
          {
            field: "Budget",
            width:"100",
            headerClass: 'ag-grid-custom-header',
          }
        ]
      },
      // 승인 예산
      {
        headerName:`${year.toString()}&${(year-1).toString()} FY` ,
        headerClass: 'ag-grid-custom-header',
        children: [
          {
            width:"150",
            field: "Approved",
            headerClass: 'ag-grid-custom-header',
            headerName:'ApprovedAmount'
          }
        ]
      },
      // 남은 예산
      {
        headerName:`${year.toString()}FY` ,
        headerClass: 'ag-grid-custom-header',
        children: [
          {
            width:"150",
            field: "RemainingBudget",
            headerClass: 'ag-grid-custom-header',
          }
        ]
      },
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}

/**
 * 예산 그리드 데이터 모델
 */
export class BudgetProcessGridPLOwnerDataModel {
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

