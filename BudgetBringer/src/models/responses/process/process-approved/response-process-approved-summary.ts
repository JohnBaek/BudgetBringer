import {ResponseProcessApprovedSummaryDetail} from "./response-process-approved-summary-detail";

/**
 * 결과중 개별 승인 별 통계 데이터 모음
 */
export class ResponseProcessApprovedSummary {
  /**
   * 오너정보
   */
  items: ResponseProcessApprovedSummaryDetail[];
}
