import {EnumResponseResult} from "../enums/enum-response-result";
import {T} from "unplugin-vue-router/options-yBvUhD_i";
import {Response} from "./response";

/**
 * 응답 클래스
 */
export class ResponseData<T> extends  Response {
  /**
   * 응답 데이터
   */
  data? : T;
}
