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
/// 컨트리비지니스 매니저  컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class CountryBusinessManagerController : Controller
{
    /// <summary>
    /// CountryBusinessManagerService
    /// </summary>
    private readonly ICountryBusinessManagerService _countryBusinessManagerService;
    
    /// <summary>
    /// ExcelService
    /// </summary>
    private readonly IExcelService _excelService;

    /// <summary>
    /// Constructor 
    /// </summary>
    /// <param name="costCenterService">CountryBusinessManagerService</param>
    /// <param name="excelService">ExcelService</param>
    public CountryBusinessManagerController(ICountryBusinessManagerService costCenterService, IExcelService excelService)
    {
        _countryBusinessManagerService = costCenterService;
        _excelService = excelService;
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
        return await _countryBusinessManagerService.GetListAsync(GetDefinedSearchMeta(requestQuery));
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
    
    
    /// <summary>
    /// Return RequestQuery object to set Search Meta 
    /// </summary>
    /// <param name="requestQuery">request</param>
    /// <returns></returns>
    private static RequestQuery GetDefinedSearchMeta(RequestQuery requestQuery)
    {
        // Clear previous 
        requestQuery.ResetMetas();
        requestQuery.ExtraHeaders.Add("BUSINESS UNITS");
        
        // Is Default Sort not defined 
        if (requestQuery.SortOrders is { Count: 0 })
        {
            requestQuery.SortOrders.Add("Asc");
            requestQuery.SortFields?.Add(nameof(ResponseCountryBusinessManager.Name));
        }
            
        // Add meta information
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCountryBusinessManager.Name), nameof(ResponseCountryBusinessManager.Name), true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegName),nameof(ResponseCommonWriter.RegName), true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegDate),nameof(ResponseCommonWriter.RegDate), true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.RegDate),nameof(ResponseCommonWriter.RegDate), true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModDate),nameof(ResponseCommonWriter.ModDate), true);
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
        ResponseList<ResponseCountryBusinessManager> response = await GetListAsync(requestQuery);

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        string sheetName = "Country Business Manager";
        
        // Make data For worksheet 
        workbook = _excelService.AddDataToWorkbook(sheetName: sheetName, workbook: workbook, requestQuery: requestQuery, items: response.Items!);

        // Get sheet
        IXLWorksheet? sheet = workbook.Worksheets.FirstOrDefault();
        
        // Exist sheet
        if (sheet != null)
        {
            // Avoid Null Exceptions
            response.Items ??= [];
            
            // Iterate All Business Units
            for (int i = 0; i < response.Items.Count; i++)
            {
                // Get First Value Row ( Start with 2 )
                IXLRow row = sheet.Row(i + 2);

                // Try find header Cell index
                int targetCellIndex = row.CellsUsed().Count() + 1;

                // Get Business Unit in Country Business Manager
                List<ResponseBusinessUnit> businessUnits = response.Items[i].BusinessUnits;

                row.Cell(targetCellIndex).Value =
                    businessUnits.Count == 0 ? "-" : string.Join("\n", businessUnits.Select(i => $"- {i.Name}"));
            }
        }

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
}