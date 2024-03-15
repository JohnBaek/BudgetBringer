using System.ComponentModel.DataAnnotations;

namespace BasicAuthentication.ViewModels.Requests.Login;

/// <summary>
/// 로그인 요청 클래스
/// </summary>
public class RequestLogin
{
    /// <summary>
    /// 로그인 아이디
    /// </summary>
    [Required(ErrorMessage = "로그인 아이디를 입력해주세요")]
    public string LoginId { get; set; }

    /// <summary>
    /// 패스워드
    /// </summary>
    [Required(ErrorMessage = "패스워드를 입력해주세요")]
    public string Password { get; set; }
}