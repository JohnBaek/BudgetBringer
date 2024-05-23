/**
 *  Represent for Request Query To Server
 */
export class RequestQuery {
  /**
   * Constructor
   * @param apiUri
   * @param skip
   * @param pageCount
   */
  constructor(apiUri: string, skip: number, pageCount: number) {
    this.apiUri = apiUri;
    this.skip = skip;
    this.pageCount = pageCount;
  }
  /**
   * api URI
   */
  apiUri: string = '';
  /**
   *  스킵
   */
  skip: number = 0;
  /**
   *  페이지 카운트
   */
  pageCount: number = 100;
  /**
   *  (사용자로부터 입력 받음) 검색 키워드
   */
  searchKeywords: string[] = [];
  /**
   * (사용자로부터 입력 받음)  검색 필드
   */
  searchFields: string[] = [];
  /**
   *  (사용자로부터 입력 받음) Sort 종류
   */
  sortOrders: string[] = [];
  /**
   * (사용자로부터 입력 받음)  Sort 필드
   */
  sortFields: string[] = [];
}
