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
    public string Value { get; init; } = "";
}