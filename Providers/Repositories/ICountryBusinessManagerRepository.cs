using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;

namespace Providers.Repositories;

/// <summary>
/// 컨트리 비지니스 매니저 리파지토리
/// </summary>
public interface ICountryBusinessManagerRepository
{
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<List<DbModelCountryBusinessManager>> GetListAsync(RequestQuery requestQuery);

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Response> UpdateAsync(RequestBusinessUnit request);
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Response> AddAsync(RequestBusinessUnit request);
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    Task<Response> DeleteAsync(string id);
}