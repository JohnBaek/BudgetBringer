export enum EnumApprovalStatus {
  /**
   * 상태 없음
   */
  None,

  /**
   * 세금계산서 발행 전
   */
  NotYetIssuePo,

  /**
   * 세금계산서 발행
   */
  IssuePo,

  /**
   * 인보이스 발행
   */
  SpendingAndIssuePo
}

/**
 * key : value
 */
export const ApprovalStatusDescriptions: Record<EnumApprovalStatus, string> = {
  [EnumApprovalStatus.None]: "None",
  [EnumApprovalStatus.NotYetIssuePo]: "Not Yet Issue PO",
  [EnumApprovalStatus.IssuePo]: "Issue PO",
  [EnumApprovalStatus.SpendingAndIssuePo]: "Spending & Issue PO"
};


export const GetApprovalStatusList = () => {
  return Object.entries(ApprovalStatusDescriptions).map(([status, description]) => ({
    id: parseInt(status),
    title: description
  }))
}

