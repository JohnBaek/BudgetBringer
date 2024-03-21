import {EnumResponseResult} from "../Enums/EnumResponseResult";

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
}
