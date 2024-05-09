using ClosedXML.Excel;
using Features.Attributes;
using Features.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Enums;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Process;
using Models.Responses.Process.ProcessApproved;
using Models.Responses.Process.ProcessBusinessUnit;
using Models.Responses.Process.ProcessOwner;
using Providers.Services.Interfaces;

namespace Apis.Controllers;


/// <summary>
/// 예산 결과 서비스 컨트롤러
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,User")]
public class BudgetProcessController : Controller
{
    /// <summary>
    /// 비지니스 유닛 서비스 
    /// </summary>
    private readonly IBudgetProcessService _budgetProcessService;
    
    /// <summary>
    /// Excel Provider
    /// </summary>
    private readonly IExcelService _excelService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="businessUnitService"></param>
    /// <param name="excelService"></param>
    public BudgetProcessController(IBudgetProcessService businessUnitService, IExcelService excelService)
    {
        _budgetProcessService = businessUnitService;
        _excelService = excelService;
    }

    /// <summary>
    /// ProcessOwner 별 Budget 프로세스 정보를 가져온다.
    /// </summary>
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    [HttpGet("ProcessOwner")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<ResponseData<ResponseProcessOwnerSummary>> GetOwnerBudgetAsync(string year = "")
    {
        return await _budgetProcessService.GetOwnerBudgetSummaryAsync(year);
    }
    
    /// <summary>
    /// BusinessUnit 별 Budget 프로세스 정보를 가져온다.
    /// </summary>
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    [HttpGet("BusinessUnit")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<ResponseData<ResponseProcessBusinessUnitSummary>> GetBusinessUnitBudgetAsync(string year = "")
    {
        if (year.IsEmpty())
            year = DateTime.Now.ToString("yyyy");
        
        return await _budgetProcessService.GetBusinessUnitBudgetSummaryAsync(year);
    }
    
    
    /// <summary>
    /// Get Approved Analysis for Below 
    /// </summary>
    /// <returns></returns>
    [HttpGet("Approved/Below")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetApprovedBelowAmountSummaryAsync()
    {
        return await _budgetProcessService.GetApprovedBelowAmountSummaryAsync();
    }
    
    /// <summary>
    /// Get Approved Analysis for Above 
    /// </summary>
    /// <returns></returns>
    [HttpGet("Approved/Above")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetApprovedAboveAmountSummaryAsync()
    {
        return await _budgetProcessService.GetApprovedAboveAmountSummaryAsync();
    }
    
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("ProcessOwner/Export/Excel/{year}")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<IActionResult> ProcessOwnerExportExcel([FromQuery] RequestQuery requestQuery, [FromRoute] string year)
    {
        string thisYearDate = DateTime.Now.ToString("yyyy-MM-dd");
        
        // Define request 
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessOwner.CountryBusinessManagerName) ,thisYearDate,true );
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.BudgetYear) ,$"{year}FY BUDGET AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.ApprovedYear) ,$"{year}FY APPROVED AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.RemainingYear) ,$"{year}FY REMAINING YEAR",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.Ratio) ,$"{year}FY RATIO(%)",true ,true);

        // Get data
        ResponseData<ResponseProcessOwnerSummary> response = await GetOwnerBudgetAsync(year);

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        
        if (response.Data == null)
            response.Data = new ResponseProcessOwnerSummary();

        // Process all 
        foreach (ResponseProcessSummaryDetail<ResponseProcessOwner> item in response.Data.Items)
        {
            // Make data For worksheet 
            workbook = _excelService.AddDataToWorkbook(
                  sheetName: item.Title
                , workbook: workbook
                , requestQuery: requestQuery
                , items: item.Items
            );

            var lastRow = workbook.Worksheets.Last().LastRowUsed().RowNumber();
            for (int rowIndex = 1; rowIndex <= lastRow; rowIndex++)
            {
                // 숫자 형식 적용
                workbook.Worksheets.Last().Cell(rowIndex, 5).Style.NumberFormat.Format = "0.00";

                if (rowIndex == lastRow) 
                {
                    double budgetSum = double.Parse(workbook.Worksheets.Last().Cell(rowIndex, 2).Value.ToString());
                    double remainingSum = double.Parse(workbook.Worksheets.Last().Cell(rowIndex, 4).Value.ToString());
                    double ratio = remainingSum == 0 ? 0 : (remainingSum / budgetSum) * 100; 
                    workbook.Worksheets.Last().Cell(rowIndex, 5).Value = ratio;
                }
            }
        }

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("BusinessUnit/Export/Excel/{year}")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<IActionResult> BusinessUnitExportExcel([FromQuery] RequestQuery requestQuery, [FromRoute] string year)
    {
        string thisYearDate = DateTime.Now.ToString("yyyy-MM-dd");
        
        // Define request 
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.BusinessUnitName) ,thisYearDate,true );
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.BudgetYear) ,$"{year}FY BUDGET AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.ApprovedYear) ,$"{year}FY APPROVED AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.RemainingYear) ,$"{year}FY REMAINING YEAR",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.Ratio) ,$"{year}FY Ratio(%)",true ,true);
        // requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.BudgetApprovedYearSum) ,$"{thisYear}&{beforeYear}FY\nAPPROVED AMOUNT",true ,true);
        // requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessBusinessUnit.BudgetRemainingYear) ,"REMAINING YEAR",true ,true);

        // Get data
        ResponseData<ResponseProcessBusinessUnitSummary> response = await GetBusinessUnitBudgetAsync(year);
        
        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        
        if (response.Data == null)
            response.Data = new ResponseProcessBusinessUnitSummary();

        // Process all 
        foreach (ResponseProcessSummaryDetail<ResponseProcessBusinessUnit> item in response.Data.Items)
        {
            // Make data For worksheet 
            workbook = _excelService.AddDataToWorkbook(
                  sheetName: item.Title
                , workbook: workbook
                , requestQuery: requestQuery
                , items: item.Items
            );
            
            var lastRow = workbook.Worksheets.Last().LastRowUsed().RowNumber();
            for (int rowIndex = 1; rowIndex <= lastRow; rowIndex++)
            {
                // 숫자 형식 적용
                workbook.Worksheets.Last().Cell(rowIndex, 5).Style.NumberFormat.Format = "0.00";

                if (rowIndex == lastRow) 
                {
                    double budgetSum = double.Parse(workbook.Worksheets.Last().Cell(rowIndex, 2).Value.ToString());
                    double remainingSum = double.Parse(workbook.Worksheets.Last().Cell(rowIndex, 4).Value.ToString());
                    double ratio = remainingSum == 0 ? 0 : (remainingSum / budgetSum) * 100; 
                    workbook.Worksheets.Last().Cell(rowIndex, 5).Value = ratio;
                }
            }
        }
        
        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
    
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("Approved/Below/Export/Excel")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<IActionResult> ApprovedBelowAmountSummaryExportAsync([FromQuery] RequestQuery requestQuery)
    {
        DateTime today = DateTime.Now;
        string thisYearDate = today.ToString("yyyy-MM-dd");
        
        // Define request 
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.CountryBusinessManagerName) ,thisYearDate,true );
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.PoIssueAmountSpending) ,$"SPENDING & PO ISSUE AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.PoIssueAmount) ,$"PO ISSUE AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.NotPoIssueAmount) ,$"NOT PO ISSUE AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.ApprovedAmount) ,$"APPROVED AMOUNT",true ,true);

        // Get data
        ResponseData<ResponseProcessApprovedSummary> response = await GetApprovedBelowAmountSummaryAsync();

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        
        if (response.Data == null)
            response.Data = new ResponseProcessApprovedSummary();

        // Process all 
        foreach (ResponseProcessSummaryDetail<ResponseProcessApproved> item in response.Data.Items.OrderBy(i => i.Sequence))
        {
            // Make data For worksheet 
            workbook = _excelService.AddApprovedDataToWorkbook(
                  sheetName: item.Title
                , workbook: workbook
                , requestQuery: requestQuery
                , items: item.Items
            );    
        }

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
    
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("Approved/Above/Export/Excel")]
    [ClaimRequirement("Permission","process-result,process-result-view")]
    public async Task<IActionResult> ApprovedAboveAmountSummaryExportAsync([FromQuery] RequestQuery requestQuery)
    {
        DateTime today = DateTime.Now;
        string thisYearDate = today.ToString("yyyy-MM-dd");
        
        // Define request 
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.CountryBusinessManagerName) ,thisYearDate,true );
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.PoIssueAmountSpending) ,$"SPENDING & PO ISSUE AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.PoIssueAmount) ,$"PO ISSUE AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.NotPoIssueAmount) ,$"NOT PO ISSUE AMOUNT",true ,true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseProcessApproved.ApprovedAmount) ,$"APPROVED AMOUNT",true ,true);

        // Get data
        ResponseData<ResponseProcessApprovedSummary> response = await GetApprovedAboveAmountSummaryAsync();

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        
        if (response.Data == null)
            response.Data = new ResponseProcessApprovedSummary();

        // Process all 
        foreach (ResponseProcessSummaryDetail<ResponseProcessApproved> item in response.Data.Items.OrderBy(i => i.Sequence))
        {
            // Make data For worksheet 
            workbook = _excelService.AddApprovedDataToWorkbook(
                  sheetName: item.Title
                , workbook: workbook
                , requestQuery: requestQuery
                , items: item.Items
            );    
        }

        // Create Stream for generate file
        MemoryStream stream = new MemoryStream();
        
        // Save workbook to memory stream
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
    }
}