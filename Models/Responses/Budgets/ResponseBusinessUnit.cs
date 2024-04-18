namespace Models.Responses.Budgets;

/// <summary>
/// 비지니스 유닛 응답 모델
/// </summary>
public class ResponseBusinessUnit : ResponseCommonWriter
{
    /// <summary>
    /// 생성자
    /// </summary>
    public ResponseBusinessUnit()
    {
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public ResponseBusinessUnit(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// 아이디 
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// 유닛명 (유니크)
    /// </summary>
    public string Name { get; init; }
}