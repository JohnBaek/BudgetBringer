/**
 * 예산정보 요청 클래스
 */
export class RequestBudgetPlan {
  /**
   * 500K 이상 예산 여부
   */
  isAbove500K: boolean = false;

  /**
   * 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
   */
  approvalDate: string = "";

  /**
   * 설명
   */
  description: string | null = "";
  /**
   * Whether to include in statistics
   */
  isIncludeInStatistics:boolean = false;
  /**
   * 섹터 아이디
   */
  sectorId: string = "";

  /**
   * DbModelBusinessUnit 아이디
   */
  businessUnitId: string = "";

  /**
   * DbModelCostCenter 아이디
   */
  costCenterId: string = "";

  /**
   * DbModelCountryBusinessManager 아이디
   */
  countryBusinessManagerId: string = "";

  /**
   * 총예산
   */
  budgetTotal: number = 0;

  /**
   * OcProjectName
   */
  ocProjectName: string | null = "";

  /**
   * BossLineDescription
   */
  bossLineDescription: string | null = "";
}
