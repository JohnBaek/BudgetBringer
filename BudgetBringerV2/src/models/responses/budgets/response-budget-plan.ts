import {ResponseCommonWriter} from "../response-common-writer";
import {ResponseFileUpload} from "../files/response-upload-file";

/**
 * 예산정보 응답 클래스
 */
export interface ResponseBudgetPlan extends ResponseCommonWriter {
  /**
   * 예산 모델 아이디
   */
  id: string;
  /**
   * 500K 이상 예산 여부
   */
  isAbove500K: boolean;
  /**
   * Whether to include in statistics
   */
  IsIncludeInStatistics:boolean;
  /**
   * 기안일 ( 날짜가아닌 일반 스트링데이터도 포함 될 수 있다. )
   */
  approvalDate: string;
  /**
   * 기안일 정상 포맷 (yyyy-MM-dd) 이라면 DateOnly 로 파싱된 값
   */
  approveDateValue: Date | null;
  /**
   * 기안일 정상 포맷 (yyyy-MM-dd) 여부
   */
  isApprovalDateValid: boolean;
  /**
   * 설명
   */
  description: string | null;
  /**
   * 섹터 아이디
   */
  sectorId: string;
  /**
   * DbModelBusinessUnit 아이디
   */
  businessUnitId: string;
  /**
   * DbModelCostCenter 아이디
   */
  costCenterId: string;
  /**
   * DbModelCountryBusinessManager 아이디
   */
  countryBusinessManagerId: string;
  /**
   * DbModelSector 명
   */
  sectorName: string;
  /**
   * DbModelCostCenter 명
   */
  budgetTotal: number;
  /**
   * DbModelCountryBusinessManager 명
   */
  ocProjectName: string | null;
  /**
   * DbModelBusinessUnit 명
   */
  bossLineDescription: string | null;
  /**
   * File group Id
   */
  fileGroupId: string | null;
  /**
   * attached Files
   */
  attachedFiles: Array<ResponseFileUpload> | null;
  /**
   * Base Year for Statistics ex ) 2024 .. 2025
   */
  baseYearForStatistics: number;
}

