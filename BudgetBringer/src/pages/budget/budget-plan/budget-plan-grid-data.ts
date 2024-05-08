import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../services/utils/common-grid-column-definitions";

/**
 * Budget Plan Grid Model
 */
export class BudgetPlanGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.columDefined = [
      CommonColumnDefinitions.getBaseYearColumn() ,
      CommonColumnDefinitions.getApprovalDate() ,
      CommonColumnDefinitions.getIsIncludeInStatistics() ,
      CommonColumnDefinitions.getAttachedFiles() ,
      CommonColumnDefinitions.getDescription() ,
      CommonColumnDefinitions.getSector() ,
      CommonColumnDefinitions.getBusinessUnit() ,
      CommonColumnDefinitions.getCountryBusinessManager() ,
      CommonColumnDefinitions.getCostCenter() ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250, "ocProjectName", "Oc Project Name"),
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250,"bossLineDescription", "BossLineDescription"),
      CommonColumnDefinitions.createColumnDefinitionForNumberFilter(150, "budgetTotal", "FvBudget", this.numberValueFormatter),
    ]
  }
}

