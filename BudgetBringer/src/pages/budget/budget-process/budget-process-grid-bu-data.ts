import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {
  ResponseProcessBusinessUnit
} from "../../../models/responses/process/process-business-unit/response-process-business-unit";

/**
 * 진생상황 BudgetProcessGridBusinessUnit 그리드 모델
 */
export class BudgetProcessGridBusinessUnit extends CommonGridModel<ResponseProcessBusinessUnit>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<ResponseProcessBusinessUnit>;
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
        field: "businessUnitName",
        headerName: date,
        headerClass: 'ag-grids-custom-header',
      },
      // // 예산
      // {
      //   field: "Currency",
      //   headerName:`KRW` ,
      //   headerClass: 'ag-grids-custom-header',
      //   cellRendererFramework: CommonGridRendererSkeleton,
      // },
      // 예산
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
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}

