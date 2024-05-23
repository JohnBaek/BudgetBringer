import {ResponseCommonWriter} from "../response-common-writer";

/**
 * 비지니스 유닛 응답 모델
 */
export interface ResponseBusinessUnit extends ResponseCommonWriter {
  /**
   * 아이디
   */
  id: string;
  /**
   * 유닛명 (유니크)
   */
  name: string;
  /**
   * Sequence
   */
  sequence: number;
}
