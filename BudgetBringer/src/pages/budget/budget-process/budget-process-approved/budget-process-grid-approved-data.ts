import {CommonGridModel} from "../../../../shared/grids/common-grid-model";
import {RequestQuery} from "../../../../models/requests/query/request-query";
import {CommonColumnDefinitions} from "../../../../shared/grids/common-grid-column-definitions";

/**
 * StatusOfPurchase Grid Model
 */
export class BudgetProcessGridProcessApproved extends CommonGridModel{
  /**
   * Date
   */
  public date: string;
  /**
   * Year of Date
   */
  public year : number;
  /**
   * Constructor
   * @param date 전체 날짜 정보
   * @param year
   * @param requestQuery
   */
  constructor( date: string , year : number ,  requestQuery: RequestQuery  ) {
    super();
    this.date = date;
    this.year = year;
    this.requestQuery = requestQuery;
    this.columDefined = [
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "countryBusinessManagerName", date , null,false, false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "poIssueAmountSpending", "Spending & PO issue Amount" , this.numberValueFormatter , false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "poIssueAmount", "PO issue Amount" , this.numberValueFormatter , false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "notPoIssueAmount", "Not PO issue Amount" , this.numberValueFormatter , false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "approvedAmount", "ApprovedAmount" , this.numberValueFormatter , false) ,
    ]
    this.setSkeleton();
    this.setChart("bar","countryBusinessManagerName");
  }
}

