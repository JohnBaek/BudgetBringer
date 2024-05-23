import type { ResponseBusinessUnit } from '@/models/responses/budgets/response-business-unit'
import type { ResponseCommonWriter } from '@/models/responses/response-common-writer'

/**
 * CBM 관리 응답 모델
 */
export interface ResponseCountryBusinessManager extends ResponseCommonWriter {
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
  businessUnits: Array<ResponseBusinessUnit>;
  /**
   * Sequence
   */
  sequence: number;
}
