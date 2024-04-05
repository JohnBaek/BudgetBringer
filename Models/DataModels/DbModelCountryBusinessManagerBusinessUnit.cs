using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// 컨트리 비지니스매니저 1:N 비지니스 유닛 관계 테이블
/// </summary>
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
[Table("CountryBusinessManagerBusinessUnit")]
public class DbModelCountryBusinessManagerBusinessUnit
{
    /// <summary>
    /// 컨트리 비지니스매니저 아이디 
    /// </summary>
    [Key]
    public Guid CountryBusinessManagerId { get; set; }
    
    /// <summary>
    /// 비지니스 유닛 아이디
    /// </summary>
    [Required]
    public Guid BusinessUnitId { get; set; }
    
    /// <summary>
    /// 컨트리 비지니스 매니저
    /// </summary>
    public virtual DbModelCountryBusinessManager? CountryBusinessManager { get; init; }
    
    /// <summary>
    /// 비지니스 유닛
    /// </summary>
    public virtual DbModelBusinessUnit? BusinessUnit { get; init; }
}