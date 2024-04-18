using ClosedXML.Excel;
using Models.Requests.Query;

namespace Providers.Services.Interfaces;

/**
 * Excel Service Interface
 */
public interface IExcelService
{
    /// <summary>
    /// Generate work book for 
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="requestQuery"></param>
    /// <param name="items"></param>
    /// <param name="sheetName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    XLWorkbook AddDataToWorkbook<T>(XLWorkbook workbook, RequestQuery requestQuery, List<T> items , string sheetName);
}