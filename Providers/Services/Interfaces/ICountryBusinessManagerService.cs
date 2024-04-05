using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;

namespace Providers.Services.Interfaces;

/// <summary>
/// 컨트리 비지니스 매니저 (CBM) 서비스 인터페이스
/// </summary>
public interface ICountryBusinessManagerService
{
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<ResponseList<ResponseCountryBusinessManager>> GetListAsync(RequestQuery requestQuery);

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    Task<ResponseData<ResponseCountryBusinessManager>> GetAsync(string id);

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Response> UpdateAsync(string id,  RequestCountryBusinessManager request);
    
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ResponseData<ResponseCountryBusinessManager>> AddAsync(RequestCountryBusinessManager request);
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    Task<Response> DeleteAsync(string id);
    
    /// <summary>
    /// 매니저에 비지니스 유닛을 추가한다.
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    Task<ResponseData<ResponseCountryBusinessManager>> AddUnitAsync(string managerId , string unitId);
    
    
    /// <summary>
    /// 매니저에 비지니스 유닛을 제거한다..
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    Task<Response> DeleteUnitAsync(string managerId , string unitId);
}
