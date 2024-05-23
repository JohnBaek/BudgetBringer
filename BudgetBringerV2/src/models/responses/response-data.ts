import {Response} from "./response";

/**
 * Within <T> Response
 */
export interface ResponseData<T> extends Response {
  // Response Data
  data? : T;
}
