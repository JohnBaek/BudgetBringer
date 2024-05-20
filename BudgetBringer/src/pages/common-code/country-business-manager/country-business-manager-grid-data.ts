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
      CommonColumnDefinitions.getSequence() ,
      CommonColumnDefinitions.getName() ,
      CommonColumnDefinitions.getRegDateColumn() ,
      CommonColumnDefinitions.getRegNameColumn() ,
    ]
  }
}
