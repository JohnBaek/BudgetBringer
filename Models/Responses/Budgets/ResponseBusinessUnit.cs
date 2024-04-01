namespace Models.Responses.Budgets;

/// <summary>
/// 비지니스 유닛 응답 모델
/// </summary>
public class ResponseBusinessUnit
{
    /// <summary>
    /// 아이디 
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// 유닛명 (유니크)
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