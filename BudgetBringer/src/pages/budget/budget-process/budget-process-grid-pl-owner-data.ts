import {CommonGridModel} from "../../../shared/grids/common-grid-model";
import {RequestQuery} from "../../../models/requests/query/request-query";

/**
 * 진생상황 P&L Owner 그리드 모델
 */
export class BudgetProcessGridPLOwner extends CommonGridModel {
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
   * @param date
   * @param year
   * @param requestQuery
   */
  constructor( date: string , year : number , requestQuery: RequestQuery ) {
    super();
    this.date = date;
    this.year = year;
    this.requestQuery = requestQuery;
    // // Compute calculated Totals
    // this.calculateTotals = ({ items }: { items: any  }) => {
    //   return items.map(data => ({
    //     countryBusinessManagerName : "합계",
    //     budgetYear: data.items.reduce((sum, item) => sum + item['budgetYear'], 0),
    //     budgetApprovedYearSum: data.items.reduce((sum, item) => sum + item['budgetApprovedYearSum'], 0),
    //     budgetRemainingYear: data.items.reduce((sum, item) => sum + item['budgetRemainingYear'], 0),
    //   }));
    // }

    this.columDefined = [
      {
        field: "countryBusinessManagerName",
        headerName: date,
        headerClass: 'ag-grids-custom-header',
      },
      {
        headerName:`${year.toString()}FY` ,
        headerClass: 'ag-grids-custom-header',
        children: [
          {
            headerName:'BudgetYear',
            field: "budgetYear",
            headerClass: 'ag-grids-custom-header',
            valueFormatter: this.numberValueFormatter,
          }
        ]
      },
      {
        headerName:`${year.toString()}&${(year-1).toString()} FY` ,
        headerClass: 'ag-grids-custom-header',
        children: [
          {
            field: "budgetApprovedYearSum",
            headerClass: 'ag-grids-custom-header',
            headerName:'ApprovedAmount',
            valueFormatter: this.numberValueFormatter,
          }
        ]
      },
      {
        headerName:`${year.toString()}FY` ,
        headerClass: 'ag-grids-custom-header',
        children: [
          {
            field: "budgetRemainingYear",
            headerClass: 'ag-grids-custom-header',
            headerName:'RemainingYear',
            valueFormatter: this.numberValueFormatter,
          }
        ]
      },
    ]
  }
}


