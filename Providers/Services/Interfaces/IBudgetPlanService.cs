using Models.Requests.Budgets;
using Models.Requests.Files;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;

namespace Providers.Services.Interfaces;

/// <summary>
/// 예산 계획 서비스 인터페이스
/// </summary>
public interface IBudgetPlanService
{
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<ResponseList<ResponseBudgetPlan>> GetListAsync(RequestQuery requestQuery);
    
    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    Task<ResponseData<ResponseBudgetPlan>> GetAsync(string id);

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Response> UpdateAsync(string id, RequestBudgetPlan request);
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ResponseData<ResponseBudgetPlan>> AddAsync(RequestBudgetPlan request);
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    Task<Response> DeleteAsync(string id);

    /// <summary>
    /// Get import Preview
    /// </summary>
    /// <param name="uploadFile"></param>
    /// <returns></returns>
    Task<ResponseList<RequestBudgetPlanExcelImport>> GetImportPreview(RequestUploadFile uploadFile);

    /// <summary>
    /// Add Multiple Request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ResponseList<ResponseData<ResponseBudgetPlan>>> AddListAsync(List<RequestBudgetPlan> request);
}