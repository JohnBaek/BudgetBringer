namespace Models.Responses.Budgets;

/// <summary>
/// 코스트 센터 응답 모델
/// </summary>
public class ResponseCostCenter
{
    /// <summary>
    /// 아이디 
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// CostCenter 값 (유니크)
    /// </summary>
    public int Value { get; init; }
    
    /// <summary>
    /// 등록일 (필수)
    /// </summary>
    public DateTime RegDate { get; init; }
    
    /// <summary>
    /// 수정일 (필수)
    /// </summary>
    public DateTime ModDate { get; init; }
}