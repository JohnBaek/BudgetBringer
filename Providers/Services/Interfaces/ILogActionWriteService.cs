using Models.DataModels;
using Models.Responses;

namespace Providers.Services.Interfaces;

/// <summary>
/// 액션로그 기록 서비스 
/// </summary>
public interface ILogActionWriteService
{
    /// <summary>
    /// 업데이트 로그를 기록한다.
    /// </summary>
    /// <param name="before">반영 전</param>
    /// <param name="after">반영 후</param>
    /// <param name="user">사용자 정보</param>
    /// <param name="contents">로그 컨텐츠 정보</param>
    /// <param name="category">카테고리</param>
    /// <typeparam name="T">모델 T</typeparam>
    /// <returns></returns>
    Task<Response> WriteUpdate<T>(T before, T after, DbModelUser user, string contents, string category)
        where T : class;


    /// <summary>
    /// 추가 로그를 기록한다.
    /// </summary>
    /// <param name="before">반영 전</param>
    /// <param name="user">사용자 정보</param>
    /// <param name="contents">로그 컨텐츠 정보</param>
    /// <param name="category">카테고리</param>
    /// <typeparam name="T">모델 T</typeparam>
    /// <returns></returns>
    Task<Response> WriteAddition<T>(T? before, DbModelUser user, string contents, string category) 
        where T : class;


    /// <summary>
    /// 삭제 로그를 기록한다.
    /// </summary>
    /// <param name="before">반영 전</param>
    /// <param name="user">사용자 정보</param>
    /// <param name="contents">로그 컨텐츠 정보</param>
    /// <param name="category">카테고리</param>
    /// <typeparam name="T">모델 T</typeparam>
    /// <returns></returns>
    Task<Response> WriteDeletion<T>(T before, DbModelUser user, string contents, string category) 
        where T : class;
}