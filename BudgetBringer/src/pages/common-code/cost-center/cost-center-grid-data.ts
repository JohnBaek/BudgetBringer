import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";

/**
 * CostCenter Grid Model
 */
export class CostCenterGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.requestQuery.apiUri = "/api/v1/CostCenter";
    this.requestQuery.sortFields = [ 'regDate' ];
    this.requestQuery.sortOrders = [ 'desc' ];
    this.columDefined = [
      {  field: 'value', headerName: '이름', headerClass: 'ag-grids-custom-header', filter: 'agTextColumnFilter', floatingFilter: true, useAsModel: true, inputType : 'text', width: 250 , isRequired: true},
      CommonColumnDefinitions.getRegDateColumn() ,
      CommonColumnDefinitions.getRegNameColumn() ,
    ]
  }
}
