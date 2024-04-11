using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Requests.Logs;

/// <summary>
/// 사용자 액션 로그 요청 정보
/// </summary>
public class RequestLogAction
{
    /// <summary>
    /// 아이디 값 
    /// </summary>
    public Guid Id { get; init;}

    /// <summary>
    /// 등록일
    /// </summary>
    [Required]
    public DateTime RegDate { get; init;}

    /// <summary>
    /// 등록자 아이디 
    /// </summary>
    [Required]
    [ForeignKey("DbModelUser")]
    public Guid RegId { get; init;}

    /// <summary>
    /// 등록자명 
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string RegName { get; init;}

    /// <summary>
    /// 내용 
    /// </summary>
    [Required]
    [MaxLength(3000)]
    public required string Contents { get; init;}
}