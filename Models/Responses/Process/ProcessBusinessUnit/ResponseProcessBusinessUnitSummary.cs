namespace Models.Responses.Process.ProcessBusinessUnit;

/// <summary>
/// 결과중 개별 ProcessOwner 별 통계 데이터 모음
/// </summary>
public class ResponseProcessBusinessUnitSummary
{
    /// <summary>
    /// 오너정보
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    public List<ResponseProcessSummaryDetail<ResponseProcessBusinessUnit>> Items { get; set; } = [];
}