using Features.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Models.Responses.Logs;
using Providers.Services.Interfaces;

namespace Apis.Controllers;

/// <summary>
/// 액션로그 유닛 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class LogActionController : Controller
{
    /// <summary>
    /// 서비스 
    /// </summary>
    private readonly ILogActionService _service;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="budgetPlanService">서비스</param>
    public LogActionController(ILogActionService budgetPlanService)
    {
        _service = budgetPlanService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("")]
    [ClaimRequirement("Permission","log-action,log-action-view")]
    public async Task<ResponseList<ResponseLogAction>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _service.GetListAsync(requestQuery);
    }

}