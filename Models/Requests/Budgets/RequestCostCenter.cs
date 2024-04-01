using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Budgets;

/// <summary>
/// 코스트 센터 응답 모델
/// </summary>
public class RequestCostCenter
{
    /// <summary>
    /// DbModelCostCenter 값 
    /// </summary>
    [Required(ErrorMessage = "값을 입력해주세요")]
    public int Value { get; init; }
}