using Models.Responses.Process.ProcessBusinessUnit;

namespace Models.Responses.Process.ProcessApproved;

/// <summary>
/// 상세
/// </summary>
public class ResponseProcessApprovedSummaryDetail
{
    /// <summary>
    /// 시퀀스 정보 , 총 3가지의 종류로 나가기때문
    /// </summary>
    public int Sequence { get; set; } = 0;

    /// <summary>
    /// 타이틀 정보 - 전년도 2023FY - 올해 2024FY - 전년도 2023FY & 올해 2024FY
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// 상세 정보 리스트
    /// </summary>
    public List<ResponseProcessApproved> Items { get; set; } = new List<ResponseProcessApproved>();
}