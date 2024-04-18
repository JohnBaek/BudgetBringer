using ClosedXML.Excel;
using Features.Attributes;
using Features.Extensions;
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
/// 예산계획 유닛 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class BudgetPlanController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly IBudgetPlanService _budgetPlanService;

    /// <summary>
    /// Excel Provider
    /// </summary>
    private readonly IExcelService _excelService;


    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="budgetPlanService">비지니스 유닛 서비스 </param>
    /// <param name="excelService">Excel Provider</param>
    public BudgetPlanController(IBudgetPlanService budgetPlanService, IExcelService excelService)
    {
        _budgetPlanService = budgetPlanService;
        _excelService = excelService;
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
        return await _budgetPlanService.GetListAsync(GetDefinedSearchMeta(requestQuery));
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
    [HttpPut("{id}")]
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
        ResponseList<ResponseBudgetPlan> response = await GetListAsync(requestQuery);

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();

        // Find index for 'isabove500k'
        int foundIndex = requestQuery.SearchFields!.FindIndex(i => i.ToLower() == "isabove500k");
        string sheetName = "Above 500K (Budget)";
        
        // Is founded 'isabove500k'
        if (foundIndex > -1)
        {
            // To parse value
            bool isAbove = Convert.ToBoolean(requestQuery.SearchKeywords?[foundIndex]);

            // Set sheet name
            sheetName = (isAbove) ? "Above 500K (Budget)" : "Below 500K (Budget)";
        }

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
    private RequestQuery GetDefinedSearchMeta(RequestQuery requestQuery)
    {
        requestQuery.SearchMetas = [];
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetPlan.IsAbove500K));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.ApprovalDate) , "APPROVAL DATE" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.ApproveDateValue));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.IsApprovalDateValid));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.Description), "DESCRIPTION", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.CostCenterName), "COST CENTER NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.CountryBusinessManagerName), "COUNTRY BUSINESS MANAGER NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.SectorName), "SECTOR NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.BusinessUnitName), "BUSINESS UNIT NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetPlan.BudgetTotal), "BUDGET TOTAL", true , isSum: true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.OcProjectName), "OC-PROJECT NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetPlan.BossLineDescription), "BOSS-LINE DESCRIPTION", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegName) , "REGISTER NAME" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegDate), "REGISTRATION DATE" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModName), "MODIFICATION NAME" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModDate), "MODIFICATION DATE" , true);
        return requestQuery;      
    }
}