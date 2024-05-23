import type { ResponseCommonWriter } from '@/models/responses/response-common-writer'

/**
 * 섹터 정보 응답 모델
 */
export interface ResponseSector extends ResponseCommonWriter {
  /**
   * 아이디
   */
  id: string;

  /**
   * 섹터 값
   */
  value: string;
}
