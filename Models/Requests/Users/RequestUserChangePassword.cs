using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Users;

/// <summary>
/// 사용자 패스워드 변경
/// </summary>
public class RequestUserChangePassword : RequestBase
{
    /// <summary>
    /// 아이디 값 
    /// </summary>
    [Required]
    public Guid Id { get; init;}
    
    /// <summary>
    /// 변경할 패스워드 
    /// </summary>
    [Required]
    public string Password { get; set; } = "";
}