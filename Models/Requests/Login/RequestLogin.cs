using System.ComponentModel.DataAnnotations;

namespace Models.Requests.Login;

/// <summary>
/// 로그인 요청 정보
/// </summary>
public class RequestLogin : RequestBase
{
    /// <summary>
    /// 로그인 아이디
    /// </summary>
    [Required(ErrorMessage = "아이디를 입력해주세요")]
    [MinLength(1 , ErrorMessage = "아이디를 입력해주세요")]
    public string LoginId { get; set; } = "";

    /// <summary>
    /// 로그인 아이디
    /// </summary>
    [Required(ErrorMessage = "패스워드를 입력해주세요")]
    [MinLength(1 , ErrorMessage = "패스워드를 입력해주세요")]
    public string Password { get; set; } = "";

}