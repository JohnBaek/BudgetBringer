import {EnumResponseResult} from "../enums/enum-response-result";

/**
 * 응답 기본 데이터
 */
export class Response {
  /**
   * 응답 결과
   */
  public result : EnumResponseResult = EnumResponseResult.error;
  /**
   * 응답 메세지
   */
  public message: string;
  /**
   * 코드
   */
  public code : string;
  /**
   * 권한 여부
   */
  public isAuthenticated = false;
  /**
   * is Result Error
   */
  public error  = () => {
    return this.result === EnumResponseResult.error;
  }
  /**
   * is Result Success
   */
  public success  = () => {
    return this.result === EnumResponseResult.success;
  }
  /**
   * is Result Warning
   */
  public warning  = () => {
    return this.result === EnumResponseResult.waring;
  }
}
