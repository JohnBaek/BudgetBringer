import {ResponseProcessApproved} from "./response-process-approved";

/**
 * 상세
 */
export interface ResponseProcessApprovedSummaryDetail {
  /**
   * 시퀀스 정보 , 총 3가지의 종류로 나가기때문
   */
  sequence: number;
  /**
   * 타이틀 정보 - 전년도 2023FY - 올해 2024FY - 전년도 2023FY & 올해 2024FY
   */
  title: string;
  /**
   * 상세 정보 리스트
   */
  items: ResponseProcessApproved[];
}
