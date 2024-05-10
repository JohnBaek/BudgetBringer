using Models.Responses;
using Models.Responses.Process.ProcessApproved;
using Models.Responses.Process.ProcessBusinessUnit;
using Models.Responses.Process.ProcessOwner;

namespace Providers.Repositories.Interfaces;

/// <summary>
/// Budget 관련 Repository
/// </summary>
public interface IBudgetProcessRepository
{
    /// <summary>
    /// 오너별 예산 편성 진행상황을 가져온다.
    /// 단, process-result-view 의 Claim 만 소유한 경우 로그인한 사용자의 CountryManagerId 가 일치하는
    /// 정보만 나와야한다. 
    /// </summary>
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    Task<ResponseData<ResponseProcessOwnerSummary>> GetOwnerBudgetSummaryAsync(string year);
    
    /// <summary>
    /// 비지니스유닛별 예산 편성 진행상황을 가져온다.
    /// 단, process-result-view 의 Claim 만 소유한 경우 로그인한 사용자의 부서가 일치하는
    /// 정보만 나와야한다. 
    /// </summary>
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    Task<ResponseData<ResponseProcessBusinessUnitSummary>> GetBusinessUnitBudgetSummaryAsync(string year);
    
    /// <summary>
    /// Get Approved Analysis for Below Amount
    /// ! If an authenticated user has only 'process-result-view' permissions, they can only view results they own.
    /// </summary>
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    Task<ResponseData<ResponseProcessApprovedSummary>> GetComputeStateOfPurchaseBelowAsync(string year);
    
    /// <summary>
    /// Get Approved Analysis for Above Amount
    /// ! If an authenticated user has only 'process-result-view' permissions, they can only view results they own.
    /// </summary>
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    Task<ResponseData<ResponseProcessApprovedSummary>> GetComputeStateOfPurchaseAboveAsync(string year);
}