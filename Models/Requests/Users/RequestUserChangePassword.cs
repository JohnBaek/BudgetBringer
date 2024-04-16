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
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$"
        , ErrorMessage = "비밀번호는 8자 이상이어야 하며 대문자, 소문자, 숫자, 특수문자가 각각 1개 이상 포함되어야 합니다.")]
    public string Password { get; set; } = "";
}