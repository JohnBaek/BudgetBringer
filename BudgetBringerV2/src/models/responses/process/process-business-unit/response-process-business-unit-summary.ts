import type {
  ResponseProcessBusinessUnitSummaryDetail
} from '@/models/responses/process/process-business-unit/response-process-business-unit-summary-detail'

/**
 * 결과중 개별 ProcessOwner 별 통계 데이터 모음
 */
export interface ResponseProcessBusinessUnitSummary {
  /**
   * 오너정보
   */
  items: ResponseProcessBusinessUnitSummaryDetail[];
}
