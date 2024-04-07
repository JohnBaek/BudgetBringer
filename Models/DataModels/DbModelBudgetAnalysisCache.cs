using System.ComponentModel.DataAnnotations;
using System.Runtime;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Common.Enums;

namespace Models.DataModels;

/// <summary>
/// Budget 통계 정보 캐싱 테이블
/// </summary>
public class DbModelBudgetAnalysisCache : DbModelDefault
{
    /// <summary>
    /// 아이디 
    /// </summary>
    [Key]
    public Guid Id { get; init; }
    
    /// <summary>
    /// Json 으로 시리얼라이즈된 정보 
    /// </summary>
    public string? JsonRaw { get; set; }

    /// <summary>
    /// 캐싱 타입 
    /// </summary>
    public EnumBudgetAnalysisCacheType CacheType { get; set; } = EnumBudgetAnalysisCacheType.ProcessOwner;
}