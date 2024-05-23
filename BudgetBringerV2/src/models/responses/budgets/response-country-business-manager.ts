import {ResponseCommonWriter} from "../response-common-writer";
import {ResponseBusinessUnit} from "./response-business-unit";

/**
 * CBM 관리 응답 모델
 */
export class ResponseCountryBusinessManager extends ResponseCommonWriter {
  /**
   * 아이디
   */
  id: string;
  /**
   * 오너명
   */
  name: string;
  /**
   * CBM 이 소유한 비지니스 유닛
   */
  businessUnits: Array<ResponseBusinessUnit> = [];
  /**
   * Sequence
   */
  sequence: number;
}
