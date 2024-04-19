using ClosedXML.Excel;
using Features.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Enums;
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
public class CostCenterController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly ICostCenterService _costCenterService;
    
    /// <summary>
    /// Excel Provider
    /// </summary>
    private readonly IExcelService _excelService;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="costCenterService">코스트센터 서비스</param>
    /// <param name="excelService"></param>
    public CostCenterController(ICostCenterService costCenterService, IExcelService excelService)
    {
        _costCenterService = costCenterService;
        _excelService = excelService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseList<ResponseCostCenter>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _costCenterService.GetListAsync(GetDefinedSearchMeta(requestQuery));
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseCostCenter>> GetAsync([FromRoute] string id)
    {
        return await _costCenterService.GetAsync(id);
    }


    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> UpdateAsync([FromRoute] string id , RequestCostCenter request)
    {
        return await _costCenterService.UpdateAsync(id , request);
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseCostCenter>> AddAsync(RequestCostCenter request)
    {
        return await _costCenterService.AddAsync(request);
    }
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> DeleteAsync(string id){
        return await _costCenterService.DeleteAsync(id);
    }
    
    
    /// <summary>
    /// Return RequestQuery object to set Search Meta 
    /// </summary>
    /// <param name="requestQuery">request</param>
    /// <returns></returns>
    private RequestQuery GetDefinedSearchMeta(RequestQuery requestQuery)
    {
        requestQuery.SearchMetas = [];
        // 기본 Sort가 없을 경우 
        if (requestQuery.SortOrders is { Count: 0 })
        {
            requestQuery.SortOrders.Add("Asc");
            requestQuery.SortFields?.Add(nameof(ResponseCostCenter.Value));
        }
            
        // 검색 메타정보 추가
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCostCenter.Value), "VALUE", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegDate), "REGISTRATION DATE" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegName) , "REGISTER NAME" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModDate), "MODIFICATION DATE" );
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModName), "MODIFICATION NAME" );
        return requestQuery;      
    }
    
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("Export/Excel")]
    public async Task<IActionResult> ExportExcel([FromQuery] RequestQuery requestQuery)
    {
        // Define request 
        requestQuery = GetDefinedSearchMeta(requestQuery);

        // Get data
        ResponseList<ResponseCostCenter> response = await GetListAsync(requestQuery);

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        string sheetName = "Cost Centers";
        
        // Make data For worksheet 
        workbook = _excelService.AddDataToWorkbook(sheetName: sheetName, workbook: workbook, requestQuery: requestQuery, items: response.Items!);

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
}