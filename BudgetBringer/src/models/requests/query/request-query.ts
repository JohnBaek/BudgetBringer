/**
 *  Query 요청
 */
export class RequestQuery {
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
  pageCount: number = 30;

  /**
   *  (사용자로부터 입력 받음) 검색 키워드
   */
  searchKeywords: string[] | null;

  /**
   * (사용자로부터 입력 받음)  검색 필드
   */
  searchFields: string[] | null;

  /**
   *  (사용자로부터 입력 받음) Sort 종류
   */
  sortOrders: string[] | null;

  /**
   * (사용자로부터 입력 받음)  Sort 필드
   */
  sortFields: string[] | null;
}
