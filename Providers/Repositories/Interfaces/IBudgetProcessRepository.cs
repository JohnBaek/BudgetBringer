using Models.Responses;
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
    /// <returns></returns>
    Task<ResponseData<ResponseProcessOwnerSummary>> GetOwnerBudgetAsync();
}