import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../services/utils/common-grid-column-definitions";

/**
 * Budget Approved Grid Model
 */
export class BudgetApprovedGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.columDefined = [
      CommonColumnDefinitions.getBaseYearColumn() ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(145, "approvalDate", "Approval Date"),
      CommonColumnDefinitions.getAttachedFiles() ,
      CommonColumnDefinitions.getApprovalStatus() ,
      CommonColumnDefinitions.getDescription() ,
      CommonColumnDefinitions.getSector() ,
      CommonColumnDefinitions.getBusinessUnit() ,
      CommonColumnDefinitions.getCountryBusinessManager() ,
      CommonColumnDefinitions.getCostCenter() ,
      CommonColumnDefinitions.createColumnDefinitionForNumberFilter(130, "actual", "Actual", this.numberValueFormatter),
      CommonColumnDefinitions.createColumnDefinitionForNumberFilter(150, "approvalAmount", "ApprovalAmount", this.numberValueFormatter),
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(130, "poNumber", "PoNumber"),
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(190, "bossLineDescription", "BossLineDescription"),
    ]
  }
}
