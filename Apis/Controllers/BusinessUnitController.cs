using Features.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Services.Interfaces;

namespace Apis.Controllers;

/// <summary>
/// 비지니스 유닛 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,User")]
public class BusinessUnitController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly IBusinessUnitService _businessUnitService;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="businessUnitService">비지니스 유닛 서비스 </param>
    public BusinessUnitController(IBusinessUnitService businessUnitService)
    {
        _businessUnitService = businessUnitService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("")]
    [ClaimRequirement("Permission","budget-plan,budget-plan-view")]
    public async Task<ResponseList<ResponseBusinessUnit>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _businessUnitService.GetListAsync(requestQuery);
    }
}