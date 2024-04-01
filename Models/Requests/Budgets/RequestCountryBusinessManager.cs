using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Budgets;

/// <summary>
/// CBM 관리 요청 모델 
/// </summary>
public class RequestCountryBusinessManager
{
    /// <summary>
    /// 오너명
    /// </summary>
    [Length(1,255 , ErrorMessage = "최소 1글자에서 255사이의 값을 입력해주세요")]
    public required string Name { get; init; } 
}