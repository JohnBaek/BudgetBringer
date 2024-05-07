import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseCountryBusinessManager} from "../../../models/responses/budgets/response-country-business-manager";

/**
 * 예산 그리드 모델
 */
export class CountryBusinessManagerGridData extends CommonGridModel{
  /**
   * 표현할 그리드의 RowData 를 받는다.
   */
  items : Array<ResponseCountryBusinessManager>;
  /**
   * 컬럼정보
   */
  columDefined : any [];
  /**
   * 생성자
   */
  constructor() {
    super();
    this.columDefined = [
      {
        field: "sequence",
        headerClass: 'ag-grids-custom-header',
        headerName:"순서" ,
        showDisabledCheckboxes: true,
        filter: 'agTextColumnFilter',
        floatingFilter: true,
        width:100,
      },
      {
        field: "name",
        headerClass: 'ag-grids-custom-header',
        headerName:"Name" ,
        showDisabledCheckboxes: true,
        filter: 'agTextColumnFilter',
        floatingFilter: true,
        width:250,
      },
      {
        field: "regDate",
        headerClass: 'ag-grids-custom-header',
        headerName:"Registration Date"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
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
      {
        field: "regName",
        headerClass: 'ag-grids-custom-header',
        headerName:"Registration Name"  ,
        filter: "agTextColumnFilter",
        filterParams: {
          filterOptions: ["포함하는"],
          maxNumConditions: 1,
        },
        floatingFilter: true,
        width:250,
      },
    ]
    this.items = [];

  }
}
