import {ResponseCommonWriter} from "../response-common-writer";

/**
 * 액션 로그 응답 클래스
 */
export class ResponseLogAction extends ResponseCommonWriter {
  /**
   * 아이디
   */
  regName: string;

  /**
   * 내용
   */
  contents: string;
}
