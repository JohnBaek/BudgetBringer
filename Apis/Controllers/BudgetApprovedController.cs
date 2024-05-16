using ClosedXML.Excel;
using Features.Attributes;
using Features.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Enums;
using Models.Requests.Budgets;
using Models.Requests.Files;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Services.Interfaces;

namespace Apis.Controllers;

/// <summary>
/// 예산승인 유닛 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class BudgetApprovedController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly IBudgetApprovedService _budgetApprovedService;
    
    /// <summary>
    /// Excel Provider
    /// </summary>
    private readonly IExcelService _excelService;
    
    
    /// <summary>
    /// Business Unit
    /// </summary>
    private readonly IBusinessUnitService _businessUnitService;
    
    /// <summary>
    /// CostCenter
    /// </summary>
    private readonly ICostCenterService _costCenterService;
    
    /// <summary>
    /// Sector
    /// </summary>
    private readonly ISectorService _sectorService;
    
    /// <summary>
    /// Country Business Managers
    /// </summary>
    private readonly ICountryBusinessManagerService _countryBusinessManagerService;

    /// <summary>
    /// 생성자 
    /// </summary>
    /// <param name="budgetPlanService">비지니스 유닛 서비스 </param>
    /// <param name="excelService"></param>
    /// <param name="businessUnitService"></param>
    /// <param name="costCenterService"></param>
    /// <param name="sectorService"></param>
    /// <param name="countryBusinessManagerService"></param>
    public BudgetApprovedController(IBudgetApprovedService budgetPlanService, IExcelService excelService, IBusinessUnitService businessUnitService, ICostCenterService costCenterService, ISectorService sectorService, ICountryBusinessManagerService countryBusinessManagerService)
    {
        _budgetApprovedService = budgetPlanService;
        _excelService = excelService;
        _businessUnitService = businessUnitService;
        _costCenterService = costCenterService;
        _sectorService = sectorService;
        _countryBusinessManagerService = countryBusinessManagerService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseList<ResponseBudgetApproved>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _budgetApprovedService.GetListAsync(GetDefinedSearchMeta(requestQuery));
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseBudgetApproved>> GetAsync([FromRoute] string id)
    {
        return await _budgetApprovedService.GetAsync(id);
    }


    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> UpdateAsync([FromRoute] string id , RequestBudgetApproved request)
    {
        return await _budgetApprovedService.UpdateAsync(id , request);
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<ResponseData<ResponseBudgetApproved>> AddAsync(RequestBudgetApproved request)
    {
        return await _budgetApprovedService.AddAsync(request);
    }
    
    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ClaimRequirement("Permission","common-code")]
    public async Task<Response> DeleteAsync(string id){
        return await _budgetApprovedService.DeleteAsync(id);
    }
    
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Import/List")]
    [ClaimRequirement("Permission","budget-plan")]
    public async Task<ResponseList<ResponseData<ResponseBudgetApproved>>> AddListAsync(List<RequestBudgetApproved> request)
    {
        return await _budgetApprovedService.AddListAsync(request);
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
        ResponseList<ResponseBudgetApproved> response = await GetListAsync(requestQuery);

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
    /// Import Budget Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("Import/Excel/File")]
    public async Task<IActionResult> GetImportExcelFile()
    {
        // Define request 
        RequestQuery requestQuery = GetDefinedSearchMeta(new RequestQuery());
        
        // Except import Properties
        requestQuery.SearchMetas = requestQuery.SearchMetas.Where(i => !new[]
        {
            nameof(ResponseCommonWriter.RegName), nameof(ResponseCommonWriter.RegDate),
            nameof(ResponseCommonWriter.ModName), nameof(ResponseCommonWriter.ModDate)
        }.Contains(i.Field) ).ToList();

        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
       
        // Sum 제거 
        foreach (RequestQuerySearchMeta searchMeta in requestQuery.SearchMetas)
            searchMeta.IsSum = false;

        // Make data For worksheet 
        workbook = _excelService.AddDataToWorkbook(sheetName: "BudgetApprovedImport", workbook: workbook, requestQuery: requestQuery, items: new List<ResponseBudgetApproved>());

        // Get Work sheet
        IXLWorksheet worksheet = workbook.Worksheets.FirstOrDefault()!;

        
        string sectors = (await _sectorService.GetListAsync(new RequestQuery(0, 100))).Items!.Select(i => i.Value).WithJoinString(",");
        _excelService.AddDropDownList(worksheet, 4,  sectors);
        
        string costCenters = (await _costCenterService.GetListAsync(new RequestQuery(0, 100))).Items!.Select(i => i.Value).WithJoinString(",");
        _excelService.AddDropDownList(worksheet, 5,  costCenters);

        string countryBusinessManagers =
            (await _countryBusinessManagerService.GetListAsync(new RequestQuery(0, 100))).Items!.Select(i => i.Name)
            .WithJoinString(",");
        _excelService.AddDropDownList(worksheet, 6,  countryBusinessManagers);

        string businessUnits = (await _businessUnitService.GetListAsync(new RequestQuery(0, 100))).Items!.Select(i => i.Name).WithJoinString(",");
        _excelService.AddDropDownList(worksheet, 7,  businessUnits);
        
        string approvalStatus = Enum.GetNames(typeof(EnumApprovalStatus)).ToList().WithJoinString(",");
        _excelService.AddDropDownList(worksheet, 9 , approvalStatus);
        

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "import.xlsx");
    }
    
    
    /// <summary>
    /// Import Budget Excel
    /// </summary>
    /// <returns></returns>
    [HttpPost("Import/Excel")]
    public async Task<ResponseList<RequestBudgetApprovedExcelImport>> GetImportPreview(RequestUploadFile request)
    {
        return await _budgetApprovedService.GetImportPreview(request);
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
            requestQuery.SortOrders.Add("Desc");
            requestQuery.SortFields?.Add(nameof(ResponseBudgetApproved.BaseYearForStatistics));
        }
        
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.IsAbove500K));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.ApprovalDate) , "APPROVAL DATE" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.BaseYearForStatistics) , "통계 기준년도" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.Description), "DESCRIPTION", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.SectorName), "SECTOR NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.CostCenterName), "COST CENTER NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.CountryBusinessManagerName), "COUNTRY BUSINESS MANAGER NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.BusinessUnitName), "BUSINESS UNIT NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.PoNumber), "PO NUMBER", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.ApprovalStatus), "APPROVAL STATUS", true, false, typeof(EnumApprovalStatus));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.ApprovalAmount) , "APPROVAL AMOUNT", true, true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.Actual),"ACTUAL", true, true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.OcProjectName));
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.OcProjectName), "OC-PROJECT NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.BossLineDescription), "BOSS-LINE DESCRIPTION", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegName) , "REGISTER NAME" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegDate), "REGISTRATION DATE" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModName), "MODIFICATION NAME" , true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModDate), "MODIFICATION DATE" , true);
        return requestQuery;      
    }
}