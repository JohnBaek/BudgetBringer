import {EnumApprovalStatus} from "../../enums/enum-approval-status";
import {ResponseFileUpload} from "../../responses/files/response-upload-file";

/**
 * 예산정보 승인 정보 요청 클래스
 */
export class RequestBudgetApproved {
  /**
   * 500K 이상 예산 여부
   */
  isAbove500K: boolean = true;
  /**
   * 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
   */
  approvalDate: string = '';
  /**
   * 설명
   */
  description: string | null = '';
  /**
   * 섹터 아이디
   */
  sectorId: string = '';
  /**
   * DbModelBusinessUnit 아이디
   */
  businessUnitId: string = '';

  /**
   * DbModelCostCenter 아이디
   */
  costCenterId: string = '';

  /**
   * DbModelCountryBusinessManager 아이디
   */
  countryBusinessManagerId: string = '';

  /**
   * 인보이스 번호
   */
  poNumber: number = 0;

  /**
   * 승인 상태 : PO 전/후 , Invoice 발행 여부
   */
  approvalStatus: EnumApprovalStatus = EnumApprovalStatus.None;

  /**
   * 승인된 예산
   */
  approvalAmount: number = 0;

  /**
   * Not PO Issue Amount
   */
  notPoIssueAmount : number = 0;

  /**
   * PO Issue Amount
   */
  poIssueAmount : number = 0;

  /**
   * SpendingAndIssue PO Amount
   */
  spendingAndIssuePoAmount : number = 0;

  /**
   * Actual
   */
  actual: number = 0;

  /**
   * OcProjectName
   */
  ocProjectName: string | null = '';

  /**
   * BossLineDescription
   */
  bossLineDescription: string | null = '';
  /**
   * attached Files
   */
  attachedFiles: Array<ResponseFileUpload> = [];
  /**
   * Base Year for Statistics ex ) 2024 .. 2025
   */
  baseYearForStatistics: number = 0;
}
