using System.Reflection;
using ClosedXML.Excel;
using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Requests.Query;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/**
 * Excel Service Implements
 */
public class ExcelService : IExcelService
{
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<ExcelService> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public ExcelService(ILogger<ExcelService> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Predefined Colors for enum type
    /// </summary>
    private readonly List<XLColor> _colors =
    [
        XLColor.AshGrey,
        XLColor.YellowProcess,
        XLColor.GreenPigment,
        XLColor.DeepSkyBlue,
        XLColor.ElectricLime,
        XLColor.FluorescentYellow,
        XLColor.GreenYellow,
        XLColor.HotPink,
        XLColor.CornflowerBlue,
        XLColor.Jade
    ];
    

    /// <summary>
    /// Generate work book for 
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="requestQuery"></param>
    /// <param name="items"></param>
    /// <param name="sheetName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public XLWorkbook AddDataToWorkbook<T>(XLWorkbook workbook, RequestQuery requestQuery, List<T> items , string sheetName)
    {
        try
        {
            // Create work sheet
            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
            
            // Generate all headers
            int columnIndex = 1;
            foreach (RequestQuerySearchMeta item in requestQuery.SearchMetas.Where(i => i.IsIncludeExcelHeader))
            {
                // Set Address A1 .. B1 ... C1
                string cellAddress = $"{GetColumnName(columnIndex++)}1";
            
                // Get Cell From Worksheet
                IXLCell cell = worksheet.Cell(cellAddress);
            
                // Set Cell Value
                cell.Value = item.ExcelHeaderName;
            }

            // Store rowHeaderRanges
            string rowCellRange = $"A1:{GetColumnName(columnIndex - 1)}1";
            
            // Set Header Style
            IXLRange headerRange = worksheet.Range(rowCellRange);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            headerRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            headerRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            headerRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
            headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            headerRange.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            headerRange.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            worksheet.Row(1).Height = 40;
            worksheet.SheetView.FreezeRows(1);
            
            int rowIndex = 2;
            List<RequestQuerySearchMeta> targetHeaders = requestQuery.SearchMetas.Where(i => i.IsIncludeExcelHeader).ToList();
            
            // Process All Data
            foreach (T data in items)
            {
                columnIndex = 1;
                foreach (RequestQuerySearchMeta meta in targetHeaders)
                {
                    // Find Property By Field Name
                    PropertyInfo? propertyInfo = data?.GetType().GetProperty(meta.Field);
                
                    // Not Exist
                    if(propertyInfo == null)
                        continue;
                    
                    string dataCell = $"{GetColumnName(columnIndex)}{rowIndex}";
                    var value = propertyInfo.GetValue(data); // Reflection을 이용해 값을 가져옴
                    
                    // Is Enum Type
                    if (meta.EnumType != null)
                    {
                        // Parse the enum, cast it, and get the description
                        Enum enumValue = (Enum)Enum.Parse(meta.EnumType, value?.ToString() ?? "");
                        worksheet.Cell(dataCell).Value = enumValue.GetDescription();
                        
                        // Get index of enum
                        int index = Convert.ToInt32(value);
                        XLColor color = GetColorFromIndex(index);
                        
                        // Set Color
                        worksheet.Cell(dataCell).Style.Fill.BackgroundColor = color;
                    }
                    // Others
                    else
                    {
                        worksheet.Cell(dataCell).Value = value?.ToString();
                    }
                        
                    // Need to Sum
                    if (meta.isSum && double.TryParse(value?.ToString(), out double doubleValue))
                    {
                        meta.Sum += doubleValue;
                        worksheet.Cell(dataCell).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        worksheet.Cell(dataCell).Value = doubleValue;
                    }
                    columnIndex++;
                }
                rowIndex++;
            }
            
            // Default Row Defined Range
            string cellStart = "A2";
            string cellEnd = $"{GetColumnName(columnIndex - 1)}{rowIndex}";
            
            // Set Default Rows to Style
            IXLRange defaultRows = worksheet.Range($"{cellStart}:{cellEnd}");
            defaultRows.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            // Add Row for Sum
            columnIndex = 1;
            foreach (RequestQuerySearchMeta meta in requestQuery.SearchMetas.Where(i => i.IsIncludeExcelHeader))
            {
                string dataCell = $"{GetColumnName(columnIndex)}{rowIndex}";

                // Set default value & Style for cell
                worksheet.Cell(dataCell).Value = "-";
                worksheet.Cell(dataCell).Style.Font.Bold = true;
                worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(dataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                
                // Store rowHeaderRanges
                rowCellRange = $"A{rowIndex}:{GetColumnName(columnIndex)}{rowIndex}";
            
                // Set Header Style
                IXLRange cellRange = worksheet.Range(rowCellRange);
                
                // Set default Style for row
                cellRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Row(rowIndex).Height = 40;
                
                // Is need to SUM COLUMN?
                if (meta.isSum)
                {
                    worksheet.Cell(dataCell).FormulaA1 = $"SUM({GetColumnName(columnIndex)}2:{GetColumnName(columnIndex)}{rowIndex - 1})";
                    worksheet.Cell(dataCell).Style.NumberFormat.Format = "#,##0";
                    worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                }

                columnIndex++;
            }
        
            // Set auto-resize for Column Width
            worksheet.Columns().AdjustToContents();
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return workbook;
    }
    
    
    /// <summary>
    /// Convert column names to numeric positions (e.g. 1 -> 'A', 2 -> 'B', ...)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string GetColumnName(int index)
    {
        int dividend = index;
        string columnName = "";

        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }
    
    
    /// <summary>
    /// Gets Colors from colors table in circularity
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private XLColor GetColorFromIndex(int index)
    {
        return _colors[index % _colors.Count]; 
    }
}