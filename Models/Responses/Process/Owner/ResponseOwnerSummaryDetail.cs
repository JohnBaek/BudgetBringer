namespace Models.Responses.Process.Owner;

/// <summary>
/// 상세
/// </summary>
public class ResponseOwnerSummaryDetail
{
    /// <summary>
    /// 시퀀스 정보 , 총 3가지의 종류로 나가기때문
    /// </summary>
    public int Sequence { get; set; } = 0;

    /// <summary>
    /// 타이틀 정보
    /// CAPEX below CHF500K
    /// CAPEX above CHF500K
    /// Total CAPEX
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// 상세 정보 리스트
    /// </summary>
    public List<ResponseOwner> Items { get; set; } = new List<ResponseOwner>();
}