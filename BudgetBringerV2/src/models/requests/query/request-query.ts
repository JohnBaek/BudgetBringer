/**
 *  Represent for Request Query To Server
 */
export class RequestQuery {
  constructor(url:string, sortOrders: string[], sortFields: string[]);
  constructor(url:string);
  constructor(url: string, sortOrders?: string[], sortFields?: string[]) {
    this.url = url;
    if (sortOrders && sortFields) {
      this.sortOrders = sortOrders;
      this.sortFields = sortFields;
    }
  }
  // URL
  private readonly url: string = '';
  // skip
  public skip: number = 0;
  // pageCount
  public pageCount: number = 100;
  // searchKeywords
  public searchKeywords: string[] = [];
  // searchFields
  public searchFields: string[] = [];
  // sortOrders
  public sortOrders: string[] = ['desc'];
  // sortFields
  public sortFields: string[] = ['regDate'];
}
