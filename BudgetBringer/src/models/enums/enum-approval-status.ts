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
  [EnumApprovalStatus.None]: "상태 없음",
  [EnumApprovalStatus.PoNotYetPublished]: "세금계산서 발행 전",
  [EnumApprovalStatus.PoPublished]: "세금계산서 발행",
  [EnumApprovalStatus.InVoicePublished]: "인보이스 발행"
};
