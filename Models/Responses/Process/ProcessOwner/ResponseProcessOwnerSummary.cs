namespace Models.Responses.Process.ProcessOwner;

/// <summary>
/// 결과중 개별 ProcessOwner 별 통계 데이터 모음
/// </summary>
public class ResponseProcessOwnerSummary
{
    /// <summary>
    /// 오너정보
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    public List<ResponseProcessSummaryDetail<ResponseProcessOwner>> Items { get; set; } = [];
}