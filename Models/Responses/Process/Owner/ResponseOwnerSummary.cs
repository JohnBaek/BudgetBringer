namespace Models.Responses.Process.Owner;

/// <summary>
/// 결과중 개별 Owner 별 통계 데이터 모음
/// </summary>
public class ResponseOwnerSummary
{
    /// <summary>
    /// 오너정보
    /// </summary>
    public List<ResponseOwnerSummaryDetail> Items { get; set; } = new List<ResponseOwnerSummaryDetail>();
}