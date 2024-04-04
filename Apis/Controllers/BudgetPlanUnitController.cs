using Features.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Services.Interfaces;

namespace Apis.Controllers;

/// <summary>
/// 예산계획 유닛 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,User")]
public class BudgetPlanController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly IBudgetPlanService _budgetPlanService;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="budgetPlanService">비지니스 유닛 서비스 </param>
    public BudgetPlanController(IBudgetPlanService budgetPlanService)
    {
        _budgetPlanService = budgetPlanService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<ResponseList<ResponseBudgetPlan>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _budgetPlanService.GetListAsync(requestQuery);
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<ResponseData<ResponseBudgetPlan>> GetAsync([FromRoute] string id)
    {
        return await _budgetPlanService.GetAsync(id);
    }


    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{id}")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<Response> UpdateAsync([FromRoute] string id , RequestBudgetPlan request)
    {
        return await _budgetPlanService.UpdateAsync(id , request);
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<ResponseData<ResponseBudgetPlan>> AddAsync(RequestBudgetPlan request)
    {
        return await _budgetPlanService.AddAsync(request);
    }
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<Response> DeleteAsync(string id){
        return await _budgetPlanService.DeleteAsync(id);
    }
    
    /// <summary>
    /// 데이터를 마이그레이션한다.
    /// </summary>
    /// <returns></returns>
    [HttpOptions("Migration")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<Response> Migration()
    {
        return await _budgetPlanService.MigrationAsync();
    }
}