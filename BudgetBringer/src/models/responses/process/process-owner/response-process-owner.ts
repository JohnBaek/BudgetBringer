/**
 * 결과중 개별 ProcessOwner 별 통계 데이터
 */
export class ResponseProcessOwner {
  /**
   * 컨트리 비지니스매니저 아이디
   */
  countryBusinessManagerId: string;
  /**
   * 컨트리 비지니스매니저 명
   */
  countryBusinessManagerName: string;
  /**
   * 올년도 Budget ( ex: 2024FY )
   */
  budgetAmount: number;
  /**
   * 작년 Budget 확정된 것 ( ex: 2023FY ) 승인된 전 년도 전체 예산
   */
  budgetApprovedYearBefore: number;
  /**
   * 올해 Budget 확정된 것 ( ex: 2024FY ) 승인된 이번년도 전체 예산
   */
  budgetApprovedYear: number;
  /**
   * 올해 & 작년 Budget 확정된 것 ( ex: 2023FY&2024FY ) 승인된 작년 + 이번년도 전체 예산
   */
  budgetApprovedYearSum: number;
  /**
   * 올해 남은 예산 ( BudgetYear - BudgetApprovedYearSum ) 2024 년 남은 Budget [올해 Budget] - [승인된 작년 + 이번년도 전체 예산]
   */
  budgetRemainingYear: number;
}
