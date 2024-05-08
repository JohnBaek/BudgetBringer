import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../services/utils/common-grid-column-definitions";

/**
 * Sector Grid Model
 */
export class SectorGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.columDefined = [
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250, "value", "이름"),
      CommonColumnDefinitions.getRegDateColumn() ,
      CommonColumnDefinitions.getRegNameColumn() ,
    ]
  }
}
