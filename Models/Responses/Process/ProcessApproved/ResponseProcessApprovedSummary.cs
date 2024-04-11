namespace Models.Responses.Process.ProcessApproved;

/// <summary>
/// 결과중 개별 승인 별 통계 데이터 모음
/// </summary>
public class ResponseProcessApprovedSummary
{
    /// <summary>
    /// 오너정보
    /// </summary>
    public List<ResponseProcessApprovedSummaryDetail> Items { get; set; } = new List<ResponseProcessApprovedSummaryDetail>();
}