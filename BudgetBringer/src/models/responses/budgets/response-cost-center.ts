import {ResponseCommonWriter} from "../response-common-writer";

/**
 * 코스트 센터 응답 모델
 */
export class ResponseCostCenter extends ResponseCommonWriter {
  /**
   * 아이디
   */
  id: string;

  /**
   * DbModelCostCenter 값 (유니크)
   */
  value: string;
}
