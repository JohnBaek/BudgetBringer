namespace Models.Responses.Budgets;

/// <summary>
/// 섹터 정보 응답 모델 
/// </summary>
public class ResponseSector : ResponseCommonWriter
{
    /// <summary>
    /// 섹터 아이디 
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 섹터 값
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