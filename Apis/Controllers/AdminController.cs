using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Common.Enums;
using Models.Requests.Query;
using Models.Requests.Users;
using Models.Responses;
using Models.Responses.Budgets;
using Models.Responses.Users;
using Providers.Services.Interfaces;

namespace Apis.Controllers;


/// <summary>
/// 관리자 컨트롤러 
/// </summary>
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/[controller]")]
public class AdminController : Controller
{
    /// <summary>
    /// 유저 리파지토리
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// Excel Provider
    /// </summary>
    private readonly IExcelService _excelService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="excelService"></param>
    public AdminController(IUserService userService, IExcelService excelService)
    {
        _userService = userService;
        _excelService = excelService;
    }
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">요청 정보</param>
    /// <returns></returns>
    [HttpGet("Users")]
    public async Task<ResponseList<ResponseUser>> GetListAsync([FromQuery] RequestQuery requestQuery)
    {
        return await _userService.GetListAsync(requestQuery);
    }
    
    /// <summary>
    /// 사용자의 패스워드를 변경한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Users")]
    public async Task<Response> UpdateAsync(RequestUserChangePassword request)
    {
        return await _userService.UpdatePasswordAsync(request);
    }
    
    
    /// <summary>
    /// Export Excel
    /// </summary>
    /// <returns></returns>
    [HttpGet("Users/Export/Excel")]
    public async Task<IActionResult> ExportExcel([FromQuery] RequestQuery requestQuery)
    {
        // Define request 
        requestQuery = GetDefinedSearchMeta(requestQuery);

        // Get data
        ResponseList<ResponseUser> response = await GetListAsync(requestQuery);

        // Response is failed
        if (response.Error)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response(EnumResponseResult.Error,"","처리중 예외가 발생했습니다."));
        
        // Create Instance
        XLWorkbook workbook = new XLWorkbook();
        string sheetName = "User Managements";
        
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
            requestQuery.SortFields?.Add(nameof(ResponseSector.Value));
        }
            
        // 검색 메타정보 추가
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseUser.DisplayName) , "NAME", true);
        requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseUser.LoginId), "LOGIN ID" , true);
        return requestQuery;
    }
}