import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseUser} from "../../../models/responses/users/response-user";

/**
 * 예산 그리드 모델
 */
export class UserManagementGridData extends CommonGridModel<ResponseUser>{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : ResponseUser[];
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
      {
        field: "displayName",
        headerClass: 'ag-grids-custom-header',
        headerName:"Name" ,
        showDisabledCheckboxes: true,
        filter: 'agTextColumnFilter',
        floatingFilter: true,
      },
      {
        field: "loginId",
        headerClass: 'ag-grids-custom-header',
        headerName:"Login ID" ,
        showDisabledCheckboxes: true,
        filter: 'agTextColumnFilter',
        floatingFilter: true,
      },
    ]
    this.isUseInsert = false;
    this.items = [];
  }
}

