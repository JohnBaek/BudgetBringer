import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";

/**
 * Budget Plan Grid Model
 */
export class BudgetPlanGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.requestQuery.apiUri = '/api/v1/BudgetPlan';
    this.requestQuery.sortFields = [ 'regDate' ];
    this.requestQuery.sortOrders = [ 'desc' ];
    this.columDefined = [
      CommonColumnDefinitions.getBaseYearColumn() ,
      CommonColumnDefinitions.getApprovalDate() ,
      CommonColumnDefinitions.getIsIncludeInStatistics() ,
      CommonColumnDefinitions.getAttachedFiles() ,
      CommonColumnDefinitions.getSector() ,
      CommonColumnDefinitions.getCountryBusinessManager() ,
      CommonColumnDefinitions.getBusinessUnit() ,
      CommonColumnDefinitions.getCostCenter() ,
      CommonColumnDefinitions.getDescription() ,
      CommonColumnDefinitions.getOcProjectName(),
      CommonColumnDefinitions.getBossLineDescription(),
      CommonColumnDefinitions.getBudgetTotal(),
    ]
  }
}

