using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Budgets;

/// <summary>
/// 비지니스 유닛 요청 모델
/// </summary>
public class RequestBusinessUnit : RequestBase
{
    /// <summary>
    /// 유닛명 (유니크)
    /// </summary>
    [Length(1,255 , ErrorMessage = "최소 1글자에서 255사이의 값을 입력해주세요")]
    public required string Name { get; set; }
    
    /// <summary>
    /// Sequence 
    /// </summary>
    public int Sequence { get; set; } = 0;
}