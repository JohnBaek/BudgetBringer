/**
 *  Represent for Request Query To Server
 */
export interface RequestQuery {
  // skip
  skip: number;

  // pageCount
  pageCount: number;

  // searchKeywords
  searchKeywords: string[];

  // searchFields
  searchFields: string[];

  // sortOrders
  sortOrders: string[];

  // sortFields
  sortFields: string[];
}
