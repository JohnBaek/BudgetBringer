import {EnumResponseResult} from "../enums/enum-response-result";
import {T} from "unplugin-vue-router/options-yBvUhD_i";

/**
 * 응답 클래스
 */
export class ResponseData<T> {
  /**
   * 응답 결과
   */
  result : EnumResponseResult = EnumResponseResult.error;

  /**
   * 응답 데이터
   */
  data? : T;

  /**
   * 응답 메세지
   */
  message: string;

  /**
   * 생성자
   * @param result 응답 결과
   * @param message 응답 데이터
   * @param data 응답 메세지
   */
  constructor(result: EnumResponseResult, message: string, data: T) {
    this.result = result;
    this.message = message;
    this.data = data;
  }
}
