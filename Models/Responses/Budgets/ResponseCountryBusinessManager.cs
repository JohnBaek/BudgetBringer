namespace Models.Responses.Budgets;

/// <summary>
/// CBM 관리 응답 모델 
/// </summary>
public class ResponseCountryBusinessManager : ResponseCommonWriter
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
    /// 비지니스 유닛
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    public List<ResponseBusinessUnit> BusinessUnits { get; set; } = new List<ResponseBusinessUnit>();
}