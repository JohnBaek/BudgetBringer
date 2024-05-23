import type { ResponseProcessOwner } from '@/models/responses/process/process-owner/response-process-owner'

/**
 * 결과중 개별 ProcessOwner 별 통계 데이터 모음
 */
export interface ResponseProcessOwnerSummary {
  /**
   * 오너정보
   */
  items: Array<ResponseProcessOwner>;
}
