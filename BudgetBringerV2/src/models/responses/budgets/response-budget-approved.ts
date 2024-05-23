import {EnumApprovalStatus} from "../../enums/enum-approval-status";

/**
 * 예산정보 승인 정보 응답 클래스
 */
export interface ResponseBudgetApproved  {
  /**
   * 예산정보 승인 정보 응답 클래스
   */
  id: string;

  /**
   * 예산 승인 모델 아이디
   */
  isAbove500K: boolean;

  /**
   * 승인일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
   */
  description: string | null;

  /**
   * 승인일이 확인된경우 ( OC 승인 예정 등의 텍스트가 아니라 날짜 형태로 들어간 경우 )
   */
  sectorId: string;

  /**
   * 500K 이상 예산 여부
   */
  businessUnitId: string;

  /**
   * 설명
   */
  costCenterId: string;

  /**
   * 섹터 아이디
   */
  countryBusinessManagerId: string;

  /**
   * DbModelBusinessUnit 아이디
   */
  sectorName: string;

  /**
   * DbModelCostCenter 아이디
   */
  poNumber: number;

  /**
   * DbModelCountryBusinessManager 아이디
   */
  approvalStatus: EnumApprovalStatus;

  /**
   * DbModelSector 명
   */
  approvalAmount: number;

  /**
   * Not PO Issue Amount
   */
  notPoIssueAmount : number;

  /**
   * PO Issue Amount
   */
  poIssueAmount : number;

  /**
   * SpendingAndIssue PO Amount
   */
  spendingAndIssuePoAmount : number;

  /**
   * DbModelCostCenter 명
   */
  actual: number;

  /**
   * DbModelCountryBusinessManager 명
   */
  ocProjectName: string | null;

  /**
   * DbModelBusinessUnit 명
   */
  bossLineDescription: string | null;
  /**
   * Base Year for Statistics ex ) 2024 .. 2025
   */
  baseYearForStatistics: number;
}
