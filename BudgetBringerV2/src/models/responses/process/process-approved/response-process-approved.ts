/**
 * 결과중 개별 승인 별 통계 데이터
 */
export interface ResponseProcessApproved {
  /**
   * 컨트리 비지니스매니저 아이디
   */
  countryBusinessManagerId: string;
  /**
   * 컨트리 비지니스매니저 명
   */
  countryBusinessManagerName: string;
  /**
   * 비지니스 유닛 아이디
   */
  businessUnitId: string;
  /**
   * 비지니스유닛 명
   */
  businessUnitName: string;
  /**
   * 승인된 금액 중 PO 발행건 합산금액
   */
  poIssueAmountSpending: number;
  /**
   * PO 발행건 합산금액
   */
  poIssueAmount: number;
  /**
   * PO 미 발행건 합산금액
   */
  notPoIssueAmount: number;
  /**
   * 승인된 금액 전체 [승인된 금액 중 PO 발행건 합산금액] + [PO 미 발행건 합산금액 ]
   */
  approvedAmount: number;
  /**
   * 비지니스 유닛별
   */
  businessUnits: ResponseProcessApproved[];
}
