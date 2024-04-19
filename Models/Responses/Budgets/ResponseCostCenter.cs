namespace Models.Responses.Budgets;

/// <summary>
/// 코스트 센터 응답 모델
/// </summary>
public class ResponseCostCenter : ResponseCommonWriter
{
    /// <summary>
    /// 아이디 
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// DbModelCostCenter 값 (유니크)
    /// </summary>
    public string Value { get; init; } = "";
}