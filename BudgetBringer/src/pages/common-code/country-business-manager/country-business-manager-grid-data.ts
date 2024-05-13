import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";

/**
 * CountryBusinessManager Grid Model
 */
export class CountryBusinessManagerGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.requestQuery.apiUri = "/api/v1/CountryBusinessManager";
    this.requestQuery.sortFields = [ 'sequence' ];
    this.requestQuery.sortOrders = [ 'asc' ];
    this.columDefined = [
      { field: 'sequence', headerName: '순서', headerClass: 'ag-grids-custom-header', filter: 'agTextColumnFilter', floatingFilter: true, useAsModel: true, inputType : 'number', width: 100 , isRequired: true},
      { field: 'name', headerName: '이름', headerClass: 'ag-grids-custom-header', filter: 'agTextColumnFilter', floatingFilter: true, useAsModel: true, inputType : 'text', width: 250 , isRequired: true} ,
      CommonColumnDefinitions.getRegDateColumn() ,
      CommonColumnDefinitions.getRegNameColumn() ,
    ]
  }
}
