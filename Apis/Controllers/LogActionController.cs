using ClosedXML.Excel;
using Features.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Enums;
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
    /// ExcelService
    /// </summary>
    private readonly IExcelService _excelService;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="budgetPlanService">서비스</param>
    /// <param name="excelService"></param>
    public LogActionController(ILogActionService budgetPlanService, IExcelService excelService)
    {
        _service = budgetPlanService;
        _excelService = excelService;
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
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("Export/Excel")]
    [ClaimRequirement("Permission","log-action,log-action-view")]
    public async Task<IActionResult> ExportExcel([FromQuery] RequestQuery requestQuery)
    {
        // Define request 
        requestQuery = GetDefinedSearchMeta(requestQuery);

        // Get data
        ResponseList<ResponseLogAction> response = await GetListAsync(requestQuery);

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        string sheetName = "Action Logs";
        
        // Make data For worksheet 
        workbook = _excelService.AddDataToWorkbook(sheetName: sheetName, workbook: workbook, requestQuery: requestQuery, items: response.Items!);

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
    
    /// <summary>
    /// Return RequestQuery object to set Search Meta 
    /// </summary>
    /// <param name="requestQuery">request</param>
    /// <returns></returns>
    private static RequestQuery GetDefinedSearchMeta(RequestQuery requestQuery)
    {
        requestQuery.ResetMetas();
        
        // 기본 Sort가 없을 경우 
        if (requestQuery.SortOrders is { Count: 0 })
        {
            requestQuery.SortOrders.Add("Asc");
            requestQuery.SortFields?.Add(nameof(ResponseCostCenter.Value));
        }
            
        // 검색 메타정보 추가
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseLogAction.Category), "카테고리", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseLogAction.ActionType), "액션종류", true, false, typeof(EnumDatabaseLogActionType));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseLogAction.Contents), "내용", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseLogAction.RegName), "등록자", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseLogAction.RegDate), "등록일", true);

        return requestQuery;      
    }
}