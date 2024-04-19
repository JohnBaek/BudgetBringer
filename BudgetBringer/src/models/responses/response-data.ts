import {Response} from "./response";

/**
 * 응답 클래스
 */
export class ResponseData<T> extends Response {
  /**
   * 응답 데이터
   */
  data? : T;
}
