namespace Models.Responses.Budgets;

/// <summary>
/// CBM 관리 응답 모델 
/// </summary>
public class ResponseCountryBusinessManager
{
    /// <summary>
    /// 아이디 
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// 오너명
    /// </summary>
    public required string Name { get; init; } 

    /// <summary>
    /// 등록일 (필수)
    /// </summary>
    public DateTime RegDate { get; init; }
    
    /// <summary>
    /// 수정일 (필수)
    /// </summary>
    public DateTime ModDate { get; init; }
}