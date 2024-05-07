using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Budgets;

/// <summary>
/// 코스트 센터 요청 모델
/// </summary>
public class RequestCostCenter : RequestBase
{
    /// <summary>
    /// 아이디 
    /// </summary>
    public string? Id { get; init; }

    /// <summary>
    /// DbModelCostCenter 값 
    /// </summary>
    [Required(ErrorMessage = "값을 입력해주세요")]
    [MinLength(1)]
    [MaxLength(255)]
    public string Value { get; set; } = "";
}