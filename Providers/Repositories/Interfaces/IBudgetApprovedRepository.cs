using Models.Requests.Budgets;
using Models.Requests.Files;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;

namespace Providers.Repositories.Interfaces;

/// <summary>
/// 예산 승인 리파지토리
/// </summary>
public interface IBudgetApprovedRepository
{
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    Task<ResponseList<ResponseBudgetApproved>> GetListAsync(RequestQuery requestQuery);
    
    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    Task<ResponseData<ResponseBudgetApproved>> GetAsync(string id);


    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Response> UpdateAsync(string id, RequestBudgetApproved request);
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ResponseData<ResponseBudgetApproved>> AddAsync(RequestBudgetApproved request);
    
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
    Task<ResponseList<RequestBudgetApprovedExcelImport>> GetImportPreview(RequestUploadFile uploadFile);
        
    /// <summary>
    /// Add Multiple Request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ResponseList<ResponseData<ResponseBudgetApproved>>> AddListAsync(List<RequestBudgetApproved> request);
}