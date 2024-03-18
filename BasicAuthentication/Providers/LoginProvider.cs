using BasicAuthenticated.Resources;
using BasicAuthentication.Interfaces;
using BasicAuthentication.Models;
using BasicAuthentication.Repositories;
using BasicAuthentication.ViewModels.Enums;
using BasicAuthentication.ViewModels.Requests.Login;
using BasicAuthentication.ViewModels.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Providers;

/// <summary>
/// 로그인 프로바이더
/// </summary>
public class LoginProvider : ILoginService
{
    private readonly IUserRepository _userRepository;

    public LoginProvider(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    /// <summary>
    /// 로그인을 시도한다.
    /// </summary>
    /// <param name="request">로그인 요청정보</param>
    /// <returns>결과</returns>
    public async Task<Response> TryLogin(RequestLogin request)
    {
        var user = await _userRepository.FindByLoginIdAsync(request.LoginId);
        if (user == null)
        {
            return new Response("", "사용자 정보를 찾을 수 없거나 비밀번호가 일치하지 않습니다.");
        }

        var signInResult = await _userRepository.CheckPasswordSignInAsync(user, request.Password);
        if (!signInResult.Succeeded)
        {
            return new Response("", "사용자 정보를 찾을 수 없거나 비밀번호가 일치하지 않습니다.");
        }

        return new Response(EnumResponseResult.Success, "", "");
    }
}