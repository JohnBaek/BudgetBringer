using System.Text;
using System.Text.Json;
using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Newtonsoft.Json;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 액션로그 기록 서비스 
/// </summary>
public class LogActionWriteService : ILogActionWriteService
{
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<LogActionWriteService> _logger;
    
    /// <summary>
    /// 로그액션 리파지토리
    /// </summary>
    private readonly ILogActionRepository _logActionRepository;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="logActionRepository">리파지토리</param>
    public LogActionWriteService(ILogger<LogActionWriteService> logger, ILogActionRepository logActionRepository)
    {
        _logger = logger;
        _logActionRepository = logActionRepository;
    }


    /// <summary>
    /// 업데이트 로그를 기록한다.
    /// </summary>
    /// <param name="before">반영 전</param>
    /// <param name="after">반영 후</param>
    /// <param name="user">사용자 정보</param>
    /// <param name="contents">로그 컨텐츠 정보</param>
    /// <param name="category">카테고</param>
    /// <typeparam name="T">모델 T</typeparam>
    /// <returns></returns>
    public async Task<Response> WriteUpdate<T>(T before, T after, DbModelUser user, string contents , string category) where T : class 
    {
        Response result;
        StringBuilder stringBuilder = new StringBuilder();
        
        try
        {
            // 반영 전 데이터를 시리얼라이즈 한다.
            string beforeJson = JsonConvert.SerializeObject(before);
            
            // 반영 후 데이터를 시리얼라이즈 한다.
            string afterJson = JsonConvert.SerializeObject(after);

            // 로그를 작성한다.
            stringBuilder.AppendLine(contents);
            stringBuilder.AppendLine($"사용자 \"[{user.DisplayName} ({user.Id})]\" 가 데이터를 업데이트 했습니다.");
            stringBuilder.AppendLine($"변경전: {beforeJson}");
            stringBuilder.AppendLine($"변경후: {afterJson}");

            await _logActionRepository.AddAsync(EnumDatabaseLogActionType.Update, stringBuilder.ToString(), category, user);
            result = new Response(EnumResponseResult.Success,"","");
        }
        catch (Exception e)
        {
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 추가 로그를 기록한다.
    /// </summary>
    /// <param name="before">반영 전</param>
    /// <param name="user">사용자 정보</param>
    /// <param name="contents">로그 컨텐츠 정보</param>
    /// <param name="category">카테고리</param>
    /// <typeparam name="T">모델 T</typeparam>
    /// <returns></returns>
    public async Task<Response> WriteAddition<T>(T before, DbModelUser user, string contents , string category) where T : class
    {
        Response result;
        StringBuilder stringBuilder = new StringBuilder();
        
        try
        {
            // 반영 전 데이터를 시리얼라이즈 한다.
            string beforeJson = JsonConvert.SerializeObject(before);

            // 로그를 작성한다.
            stringBuilder.AppendLine(contents);
            stringBuilder.AppendLine($"사용자 \"[{user.DisplayName} ({user.Id})]\" 가 데이터를 추가 했습니다.");
            stringBuilder.AppendLine($"데이터: {beforeJson}");

            await _logActionRepository.AddAsync(EnumDatabaseLogActionType.Add, stringBuilder.ToString(), category, user);
            result = new Response(EnumResponseResult.Success,"","");
        }
        catch (Exception e)
        {
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 삭제 로그를 기록한다.
    /// </summary>
    /// <param name="before">반영 전</param>
    /// <param name="user">사용자 정보</param>
    /// <param name="contents">로그 컨텐츠 정보</param>
    /// <param name="category">카테고리</param>
    /// <typeparam name="T">모델 T</typeparam>
    /// <returns></returns>
    public async Task<Response> WriteDeletion<T>(T before, DbModelUser user, string contents , string category) where T : class
    {
        Response result;
        StringBuilder stringBuilder = new StringBuilder();
        
        try
        {
            // 반영 전 데이터를 시리얼라이즈 한다.
            string beforeJson = JsonConvert.SerializeObject(before);

            // 로그를 작성한다.
            stringBuilder.AppendLine(contents);
            stringBuilder.AppendLine($"사용자 \"[{user.DisplayName} ({user.Id})]\" 가 데이터를 삭제 했습니다.");
            stringBuilder.AppendLine($"데이터: {beforeJson}");

            await _logActionRepository.AddAsync(EnumDatabaseLogActionType.Delete, stringBuilder.ToString(), category, user);
            result = new Response(EnumResponseResult.Success ,"","");
        }
        catch (Exception e)
        {
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return result;
    }
}