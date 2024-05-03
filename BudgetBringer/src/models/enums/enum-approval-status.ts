export enum EnumApprovalStatus {
  /**
   * 상태 없음
   */
  None,

  /**
   * 세금계산서 발행 전
   */
  PoNotYetPublished,

  /**
   * 세금계산서 발행
   */
  PoPublished,

  /**
   * 인보이스 발행
   */
  InVoicePublished
}

/**
 * key : value
 */
export const ApprovalStatusDescriptions: Record<EnumApprovalStatus, string> = {
  [EnumApprovalStatus.None]: "None",
  [EnumApprovalStatus.PoNotYetPublished]: "Po Not Yet Published",
  [EnumApprovalStatus.PoPublished]: "Po Published",
  [EnumApprovalStatus.InVoicePublished]: "Invoice Published"
};
