import {ResponseCommonWriter} from "../response-common-writer";

/**
 * 섹터 정보 응답 모델
 */
export class ResponseSector extends ResponseCommonWriter {
  /**
   * 아이디
   */
  id: string;

  /**
   * 섹터 값
   */
  value: string;
}
