namespace Models.Responses.Process.ProcessApproved;

/// <summary>
/// 결과중 개별 승인 별 통계 데이터 
/// </summary>
public class ResponseProcessApproved
{
    /// <summary>
    /// 컨트리 비지니스매니저 아이디 
    /// </summary>
    public Guid CountryBusinessManagerId { get; set; }

    /// <summary>
    /// 컨트리 비지니스매니저 명
    /// </summary>
    public string CountryBusinessManagerName { get; set; }  = "";
    
    /// <summary>
    /// 비지니스 유닛 아이디 
    /// </summary>
    public Guid BusinessUnitId { get; set; }

    /// <summary>
    /// 비지니스유닛 명
    /// </summary>
    public string BusinessUnitName { get; set; }  = "";

    /// <summary>
    /// 승인된 금액 중 PO 발행건 합산금액
    /// 당해년도 전체 예산
    /// </summary>
    public double PoIssueAmountSpending { get; set; }
    
    /// <summary>
    /// PO 발행건 합산금액 
    /// 승인된 전 년도 전체 예산
    /// </summary>
    public double PoIssueAmount { get; set; }
    
    /// <summary>
    /// PO 미 발행건 합산금액 
    /// 승인된 이번년도 전체 예산
    /// </summary>
    public double NotPoIssueAmount { get; set; }
    
    /// <summary>
    /// 승인된 금액 전체 [승인된 금액 중 PO 발행건 합산금액] + [PO 미 발행건 합산금액 ]
    /// </summary>
    public double ApprovedAmount { get; set; }

    /// <summary>
    /// 비지니스 유닛별 
    /// </summary>
    public List<ResponseProcessApproved> BusinessUnits { get; set; } = new List<ResponseProcessApproved>();
}