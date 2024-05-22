import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {CommonColumnDefinitions} from "../../../shared/grids/common-grid-column-definitions";

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
      CommonColumnDefinitions.getApprovalDate(),
      // CommonColumnDefinitions.getApprovalStatus() ,
      CommonColumnDefinitions.getAttachedFiles() ,
      CommonColumnDefinitions.getSector() ,
      CommonColumnDefinitions.getCountryBusinessManager() ,
      CommonColumnDefinitions.getBusinessUnit() ,
      CommonColumnDefinitions.getCostCenter() ,
      CommonColumnDefinitions.getApprovalAmount(),
      CommonColumnDefinitions.getNotPoIssueAmount(),
      CommonColumnDefinitions.getPoIssueAmount(),
      CommonColumnDefinitions.getSpendingAndIssuePoAmount(),
      CommonColumnDefinitions.getPoNumber(),
      CommonColumnDefinitions.getActual() ,
      CommonColumnDefinitions.getDescription() ,
      CommonColumnDefinitions.getBossLineDescription(),
    ];
  }
}
