import {Response} from "./response";

/**
 * Within <T> ResponseList
 */
export interface ResponseList<T> extends  Response {
  // List of Response data
  items : T[];

  // Skips
  skip: number;

  // Counter of Pages
  pageCount: number;

  // total Count
  totalCount: number;
}
