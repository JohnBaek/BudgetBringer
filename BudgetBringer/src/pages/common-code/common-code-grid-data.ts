import {CommonGridModel} from "../../shared/grids/common-grid-model";

/**
 * 예산 그리드 모델
 */
export class CommonCodeGridData extends CommonGridModel<CommonCodeGridDataModel>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<CommonCodeGridDataModel>;
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
        field: "Code",
        headerClass: 'ag-grids-custom-header',
        headerName:"코드" ,
        showDisabledCheckboxes: true,
      },
      // 섹터
      {
        field: "Description",
        headerClass: 'ag-grids-custom-header',
        headerName:"설명",
      },
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}

/**
 * 데이터 모델
 */
export class CommonCodeGridDataModel {
  /**
   * 키값
   */
  id: string;
  /**
   * 상위코드
   */
  code: string;
  /**
   * 설명
   */
  description:string;
}

