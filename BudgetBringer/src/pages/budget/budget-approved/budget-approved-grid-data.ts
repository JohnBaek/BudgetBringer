import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {ResponseBudgetApproved} from "../../../models/responses/budgets/response-budget-approved";
import {CommonColumnDefinitions} from "../../../services/utils/common-grid-column-definitions";

/**
 * Budget Approved Grid Model
 */
export class BudgetApprovedGridData extends CommonGridModel{
  /**
   * GridRaw Data
   */
  items : Array<ResponseBudgetApproved> = [];
  /**
   * Constructor
   */
  constructor() {
    super();
    this.columDefined = [
      CommonColumnDefinitions.getBaseYearColumn() ,
      CommonColumnDefinitions.CreateColumnDefinitionForTextFilter(145, "approvalDate", "Approval Date"),
      CommonColumnDefinitions.getAttachedFiles() ,
      CommonColumnDefinitions.getApprovalStatus() ,
      CommonColumnDefinitions.getDescription() ,
      CommonColumnDefinitions.getSector() ,
      CommonColumnDefinitions.getBusinessUnit() ,
      CommonColumnDefinitions.getCountryBusinessManager() ,
      CommonColumnDefinitions.getCostCenter() ,
      CommonColumnDefinitions.CreateColumnDefinitionForNumberFilter(130, "actual", "Actual", this.numberValueFormatter),
      CommonColumnDefinitions.CreateColumnDefinitionForNumberFilter(150, "approvalAmount", "ApprovalAmount", this.numberValueFormatter),
      CommonColumnDefinitions.CreateColumnDefinitionForTextFilter(130, "poNumber", "PoNumber"),
      CommonColumnDefinitions.CreateColumnDefinitionForTextFilter(190, "bossLineDescription", "BossLineDescription"),
    ]
  }
}
