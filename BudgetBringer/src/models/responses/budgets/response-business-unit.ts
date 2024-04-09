import {ResponseCommonWriter} from "../response-common-writer";

/**
 * 비지니스 유닛 응답 모델
 */
export class ResponseBusinessUnit extends ResponseCommonWriter {
  /**
   * 아이디
   */
  id: string;

  /**
   * 유닛명 (유니크)
   */
  name: string;
}
