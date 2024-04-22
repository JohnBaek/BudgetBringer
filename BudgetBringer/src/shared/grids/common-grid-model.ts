import {RequestQuery} from "../../models/requests/query/request-query";

/**
 * CommonGrid Model
 */
export abstract class CommonGridModel {
  /**
   * ag-grid Column Definitions
   */
  columDefined : any [];
  /**
   * Request Query Model
   */
  requestQuery : RequestQuery;
  /**
   * Number Formatter.
   * Uses in ag-grid Column Renderer
   * @param params
   */
  numberValueFormatter = (params) => {
    // 넘버형식인 경우
    if( typeof params.value === 'number')
      return new Intl.NumberFormat('en-US', { style: 'decimal', maximumFractionDigits: 0 }).format(params.value);

    // 그 외
    return 0;
  }
}
