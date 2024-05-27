import {Response} from "./response";

/**
 * Within <T> ResponseList
 */
export class ResponseList<T> extends  Response {
  // List of Response data
  items : T[] = [];

  // Skips
  skip: number = 0;

  // Counter of Pages
  pageCount: number = 0;

  // total Count
  totalCount: number = 0;
}
