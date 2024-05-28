import {CommonGridModel} from "../../../../shared/grids/common-grid-model";
import {RequestQuery} from "../../../../models/requests/query/request-query";
import {CommonColumnDefinitions} from "../../../../shared/grids/common-grid-column-definitions";

/**
 * Business Unit Grid Model
 */
export class BudgetProcessGridBusinessUnit extends CommonGridModel{
  /**
   * Date
   */
  public date: string;
  /**
   * Year of Date
   */
  public year : number;
  /**
   * 생성자
   * @param date 전체 날짜 정보
   * @param year 년도 정보
   * @param requestQuery 요청정보
   */
  constructor( date: string , year : number ,  requestQuery: RequestQuery  ) {
    super();
    this.date = date;
    this.year = year;
    this.requestQuery = requestQuery;
    this.columDefined = [
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "businessUnitName", date , null,false, false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "budgetYear", `Budget Amount`, this.numberValueFormatter,false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "approvedYear", `Approved Amount`, this.numberValueFormatter,false) ,
      CommonColumnDefinitions.createColumnDefinitionForTextFilter(250 , "remainingYear", `Remaining Amount`, this.numberValueFormatter,false, false) ,
      CommonColumnDefinitions.createColumnDefinitionForNumberFilter(250 , "ratio", `Ratio(%)`, null ,false, function(params) {
        return params.value.toFixed(2);
      }) ,
    ];
    this.setSkeleton();
    this.setChart('bar', 'businessUnitName');
  }
}

