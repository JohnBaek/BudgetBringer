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
/// 컨트리비지니스 매니저  컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class CountryBusinessManagerController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly ICountryBusinessManagerService _countryBusinessManagerService;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="costCenterService">컨트리비지니스매니저 서비스</param>
    public CountryBusinessManagerController(ICountryBusinessManagerService costCenterService)
    {
        _countryBusinessManagerService = costCenterService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseList<ResponseCountryBusinessManager>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _countryBusinessManagerService.GetListAsync(requestQuery);
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseCountryBusinessManager>> GetAsync([FromRoute] string id)
    {
        return await _countryBusinessManagerService.GetAsync(id);
    }


    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> UpdateAsync([FromRoute] string id , RequestCountryBusinessManager request)
    {
        return await _countryBusinessManagerService.UpdateAsync(id , request);
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseCountryBusinessManager>> AddAsync(RequestCountryBusinessManager request)
    {
        return await _countryBusinessManagerService.AddAsync(request);
    }
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> DeleteAsync(string id){
        return await _countryBusinessManagerService.DeleteAsync(id);
    }
    
    /// <summary>
    /// 매니저에 비지니스 유닛을 추가한다.
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    [HttpPost("{managerId}/BusinessUnit/{unitId}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseCountryBusinessManager>> AddUnitAsync(
        [FromRoute] string managerId , 
        [FromRoute] string unitId
        )
    {
        return await _countryBusinessManagerService.AddUnitAsync(managerId,unitId);
    }
    
    /// <summary>
    /// 매니저에 비지니스 유닛을 삭제한다.
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    [HttpDelete("{managerId}/BusinessUnit/{unitId}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> DeleteAsync(       
        [FromRoute] string managerId , 
        [FromRoute] string unitId 
        )
    {
        return await _countryBusinessManagerService.DeleteUnitAsync(managerId,unitId);
    }
}