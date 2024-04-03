using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Budgets;

/// <summary>
/// 섹터 요청 정보 모델
/// </summary>
public class RequestSector : RequestBase
{
    /// <summary>
    /// 섹터 값
    /// </summary>
    [Required(ErrorMessage = "값을 입력해주세요")]
    public int Value { get; init; }
}