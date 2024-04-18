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
/// 비지니스 유닛 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
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
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseList<ResponseBusinessUnit>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _businessUnitService.GetListAsync(requestQuery);
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseBusinessUnit>> GetAsync([FromRoute] string id)
    {
        return await _businessUnitService.GetAsync(id);
    }


    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> UpdateAsync([FromRoute] string id , RequestBusinessUnit request)
    {
        return await _businessUnitService.UpdateAsync(id , request);
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseBusinessUnit>> AddAsync(RequestBusinessUnit request)
    {
        return await _businessUnitService.AddAsync(request);
    }
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> DeleteAsync(string id){
        return await _businessUnitService.DeleteAsync(id);
    }
}