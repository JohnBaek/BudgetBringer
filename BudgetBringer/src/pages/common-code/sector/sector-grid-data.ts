import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";

/**
 * Sector Grid Model
 */
export class SectorGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.requestQuery.apiUri = "/api/v1/Sector";
    this.requestQuery.sortFields = [ 'regDate' ];
    this.requestQuery.sortOrders = [ 'desc' ];
    this.columDefined = [
      CommonColumnDefinitions.getSectorName() ,
      CommonColumnDefinitions.getRegDateColumn() ,
      CommonColumnDefinitions.getRegNameColumn() ,
    ]
  }
}
