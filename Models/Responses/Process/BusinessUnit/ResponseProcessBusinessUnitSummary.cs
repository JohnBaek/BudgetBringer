namespace Models.Responses.Process.BusinessUnit;

/// <summary>
/// 결과중 개별 Owner 별 통계 데이터 모음
/// </summary>
public class ResponseProcessBusinessUnitSummary
{
    /// <summary>
    /// 오너정보
    /// </summary>
    public List<ResponseProcessBusinessUnitSummaryDetail> Items { get; set; } = new List<ResponseProcessBusinessUnitSummaryDetail>();
}