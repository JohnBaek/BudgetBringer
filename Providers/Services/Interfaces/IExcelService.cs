using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Models.Requests.Query;
using Models.Responses.Process.ProcessApproved;

namespace Providers.Services.Interfaces;

/**
 * Excel Service Interface
 */
public interface IExcelService
{
    /// <summary>
    /// GenerateAsync work book for 
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="requestQuery"></param>
    /// <param name="items"></param>
    /// <param name="sheetName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    XLWorkbook AddDataToWorkbook<T>(XLWorkbook workbook, RequestQuery requestQuery, List<T> items , string sheetName);
    
    /// <summary>
    /// GenerateAsync work book for 
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="requestQuery"></param>
    /// <param name="items"></param>
    /// <param name="sheetName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    XLWorkbook AddApprovedDataToWorkbook(XLWorkbook workbook, RequestQuery requestQuery, List<ResponseProcessApproved> items , string sheetName);

    /// <summary>
    /// To Add Drop down list
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="cellIndex"></param>
    /// <param name="dropDownList"></param>
    void AddDropDownList(IXLWorksheet worksheet, int cellIndex, string dropDownList);
}