import {Response} from "./response";

/**
 * 응답 클래스
 */
export class ResponseList<T> extends  Response {
  /**
   * 응답 데이터
   */
  items : T[];
  /**
   * 스킵
   */
  Skip: number;
  /**
   * 페이지 카운트
   */
  PageCount: number;
  /**
   * 전체 수
   */
  TotalCount: number;
}
