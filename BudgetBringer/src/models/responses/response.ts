import {EnumResponseResult} from "../enums/enum-response-result";

/**
 * 응답 기본 데이터
 */
export class Response {
  /**
   * 응답 결과
   */
  result : EnumResponseResult = EnumResponseResult.error;

  /**
   * 응답 메세지
   */
  message: string;

  /**
   * 코드
   */
  code : string;

  /**
   * 권한 여부
   */
  isAuthenticated = false;
}
