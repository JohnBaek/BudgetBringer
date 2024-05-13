import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";
import {RequestQuery} from "../../../models/requests/query/request-query";

/**
 * Budget Approved Grid Model
 */
export class BudgetApprovedGridData extends CommonGridModel{
  /**
   * Constructor
   */
  constructor() {
    super();
    this.requestQuery.apiUri = '/api/v1/BudgetApproved';
    this.requestQuery.sortFields = [ 'regDate' ];
    this.requestQuery.sortOrders = [ 'desc' ];
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
