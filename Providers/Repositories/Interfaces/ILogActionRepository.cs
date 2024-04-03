using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Query;
using Models.Responses;

namespace Providers.Repositories.Interfaces;

/// <summary>
/// 액션 로그 모델 인터페이스
/// </summary>
public interface ILogActionRepository
{
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<List<DbModelLogAction>> GetListAsync(RequestQuery requestQuery);

    /// <summary>
    /// 로그를 추가한다.
    /// </summary>
    /// <param name="actionType">데이터베이스 액션 타입</param>
    /// <param name="contents">로그 컨텐츠</param>
    /// <param name="category">카테고리</param>
    /// <param name="user">사용자 정보</param>
    /// <returns></returns>
    Task<Response> AddAsync(EnumDatabaseLogActionType actionType, string contents, string category, DbModelUser user);
}