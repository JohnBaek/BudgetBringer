using System.Reflection;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Requests.Query;
using Models.Responses.Process.ProcessApproved;
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
    /// Add Data to XLWorkbook for export
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
            
            // Create extra headers
            foreach (string extraHeader in requestQuery.ExtraHeaders)
            {
                // Set Address A1 .. B1 ... C1
                string cellAddress = $"{GetColumnName(columnIndex)}1";
            
                // Get Cell From Worksheet
                IXLCell cell = worksheet.Cell(cellAddress);
            
                // Set Cell Value
                cell.Value = extraHeader;
            }

            // Store rowHeaderRanges
            string rowCellRange = $"A1:{GetColumnName(columnIndex + requestQuery.ExtraHeaders.Count - 1)}1";
            
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
            headerRange.SetAutoFilter();
            worksheet.Row(1).Height = 40;
            worksheet.SheetView.FreezeRows(1);
            
            int rowIndex = 2;
            List<RequestQuerySearchMeta> targetHeaders = requestQuery.SearchMetas.Where(i => i.IsIncludeExcelHeader).ToList();
            
            // Process All Data
            foreach (T data in items)
            {
                columnIndex = 1;
                // For Iterate values
                foreach (RequestQuerySearchMeta meta in targetHeaders)
                {
                    // Find Property By Field Name
                    PropertyInfo? propertyInfo = data?.GetType().GetProperty(meta.Field);
                
                    // Not Exist
                    if(propertyInfo == null)
                        continue;
                    
                    string dataCell = $"{GetColumnName(columnIndex)}{rowIndex}";
                    var value = propertyInfo.GetValue(data); 
                    worksheet.Cell(dataCell).Value = value?.ToString();

                    // Is Enum Type
                    if (meta.EnumType != null)
                    {
                        // Parse the enum, cast it, and get the description
                        Enum enumValue = (Enum)Enum.Parse(meta.EnumType, value?.ToString() ?? "");
                        worksheet.Cell(dataCell).Value = enumValue.GetDescription();

                        // Try get color 
                        string color = enumValue.GetColor();
                        
                        // Does not have defined Color
                        if (color.IsEmpty())
                        {
                            // Get index of enum
                            int index = Convert.ToInt32(value);
                            XLColor xlColor = GetColorFromIndex(index);
                            worksheet.Cell(dataCell).Style.Fill.BackgroundColor = xlColor;
                        }
                        // Does have defined Color
                        else
                        {
                            worksheet.Cell(dataCell).Style.Fill.BackgroundColor = XLColor.FromHtml(color);
                        }
                    }
                        
                    // Need to Sum
                    if (meta.IsSum && double.TryParse(value?.ToString(), out double doubleValue))
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


            // Has any IsSum is true? should add Bottom
            bool needToSumBottom = requestQuery.SearchMetas.Any(i => i.IsSum);
            int rowEndIndex = rowIndex;
            
            // Not need to Sum bottom
            if (needToSumBottom == false)
                rowEndIndex--;
            
            // Default Row Defined Range
            string cellStart = "A2";
            string cellEnd = $"{GetColumnName(columnIndex - 1 + requestQuery.ExtraHeaders.Count())}{rowEndIndex}";
            
            // Set Default Rows to Style
            IXLRange defaultRows = worksheet.Range($"{cellStart}:{cellEnd}");
            defaultRows.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            
            // Not need to Sum bottom
            if (needToSumBottom == false)
            {
                // Set auto-resize for Column Width
                worksheet.Columns().AdjustToContents();
                return workbook;
            }
            
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
                rowCellRange = $"A{rowIndex}:{GetColumnName(columnIndex + requestQuery.ExtraHeaders.Count())}{rowIndex}";
            
                // Set Header Style
                IXLRange cellRange = worksheet.Range(rowCellRange);
                
                // Set default Style for row
                cellRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Row(rowIndex).Height = 40;
                
                // Is need to SUM COLUMN?
                if (meta.IsSum)
                {
                    worksheet.Cell(dataCell).FormulaA1 = $"SUM({GetColumnName(columnIndex)}2:{GetColumnName(columnIndex)}{rowIndex - 1})";
                    worksheet.Cell(dataCell).Style.NumberFormat.Format = "#,##0";
                    worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                }

                columnIndex++;
            }

            // Iterate All Extra Headers
            foreach (string extraHeader in requestQuery.ExtraHeaders)
            {
                string dataCell = $"{GetColumnName(columnIndex)}{rowIndex}";

                // Set default value & Style for cell
                worksheet.Cell(dataCell).Value = "-";
                worksheet.Cell(dataCell).Style.Font.Bold = true;
                worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(dataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
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
    /// Add Data to XLWorkbook for export
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="requestQuery"></param>
    /// <param name="items"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    public XLWorkbook AddApprovedDataToWorkbook(XLWorkbook workbook, RequestQuery requestQuery, List<ResponseProcessApproved> items , string sheetName)
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
            
            // Create extra headers
            foreach (string extraHeader in requestQuery.ExtraHeaders)
            {
                // Set Address A1 .. B1 ... C1
                string cellAddress = $"{GetColumnName(columnIndex)}1";
            
                // Get Cell From Worksheet
                IXLCell cell = worksheet.Cell(cellAddress);
            
                // Set Cell Value
                cell.Value = extraHeader;
            }

            // Store rowHeaderRanges
            string rowCellRange = $"A1:{GetColumnName(columnIndex + requestQuery.ExtraHeaders.Count - 1)}1";
            
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
            headerRange.SetAutoFilter();
            worksheet.Row(1).Height = 40;
            worksheet.SheetView.FreezeRows(1);
            Dictionary<string, List<string>> sumCols = new Dictionary<string, List<string>>();
            
            int rowIndex = 2;
            List<RequestQuerySearchMeta> targetHeaders = requestQuery.SearchMetas.Where(i => i.IsIncludeExcelHeader).ToList();
            
            // Process All Data
            foreach (ResponseProcessApproved data in items)
            {
                columnIndex = 1;
                
                // Is Business Unit
                bool isBusinessUnit = data.BusinessUnitId != "00000000-0000-0000-0000-000000000000".ToGuid(); 
                
                // For Iterate values
                foreach (RequestQuerySearchMeta meta in targetHeaders)
                {
                    // Find Property By Field Name
                    PropertyInfo? propertyInfo = data?.GetType().GetProperty(meta.Field);
                
                    // Not Exist
                    if(propertyInfo == null)
                        continue;
                    
                    string dataCell = $"{GetColumnName(columnIndex)}{rowIndex}";
                    var value = propertyInfo.GetValue(data);
                    worksheet.Cell(dataCell).Value = value?.ToString();

                    if (isBusinessUnit)
                    {
                        if (meta.Field == nameof(ResponseProcessApproved.CountryBusinessManagerName))
                        {
                            worksheet.Cell(dataCell).Value = $" \u2192 {worksheet.Cell(dataCell).Value}";
                        }

                        worksheet.Cell(dataCell).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }
                    else
                    {
                        if (meta.IsSum)
                        {
                            if(sumCols.ContainsKey(meta.Field) == false)
                                sumCols.Add(meta.Field , new List<string>());
                            
                            sumCols[meta.Field].Add(dataCell);
                        }
                    }
                    
                    // Is Enum Type
                    if (meta.EnumType != null)
                    {
                        // Parse the enum, cast it, and get the description
                        Enum enumValue = (Enum)Enum.Parse(meta.EnumType, value?.ToString() ?? "");
                        worksheet.Cell(dataCell).Value = enumValue.GetDescription();

                        // Try get color 
                        string color = enumValue.GetColor();
                        
                        // Does not have defined Color
                        if (color.IsEmpty())
                        {
                            // Get index of enum
                            int index = Convert.ToInt32(value);
                            XLColor xlColor = GetColorFromIndex(index);
                            worksheet.Cell(dataCell).Style.Fill.BackgroundColor = xlColor;
                        }
                        // Does have defined Color
                        else
                        {
                            worksheet.Cell(dataCell).Style.Fill.BackgroundColor = XLColor.FromHtml(color);
                        }
                    }
                        
                    // Need to Sum
                    if (meta.IsSum && double.TryParse(value?.ToString(), out double doubleValue))
                    {
                        worksheet.Cell(dataCell).Value = doubleValue;
                        worksheet.Cell(dataCell).Style.NumberFormat.Format = "#,##0";
                        worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    }
                    columnIndex++;
                }
                rowIndex++;
            }

            // Has any IsSum is true? should add Bottom
            bool needToSumBottom = requestQuery.SearchMetas.Any(i => i.IsSum);
            int rowEndIndex = rowIndex;
            
            // Not need to Sum bottom
            if (needToSumBottom == false)
                rowEndIndex--;
            
            // Default Row Defined Range
            string cellStart = "A2";
            string cellEnd = $"{GetColumnName(columnIndex - 1 + requestQuery.ExtraHeaders.Count())}{rowEndIndex}";
            
            // Set Default Rows to Style
            IXLRange defaultRows = worksheet.Range($"{cellStart}:{cellEnd}");
            defaultRows.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            defaultRows.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            
            // Not need to Sum bottom
            if (needToSumBottom == false)
            {
                // Set auto-resize for Column Width
                worksheet.Columns().AdjustToContents();
                return workbook;
            }
            
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
                rowCellRange = $"A{rowIndex}:{GetColumnName(columnIndex + requestQuery.ExtraHeaders.Count())}{rowIndex}";
            
                // Set Header Style
                IXLRange cellRange = worksheet.Range(rowCellRange);
                
                // Set default Style for row
                cellRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                cellRange.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Row(rowIndex).Height = 40;
                
                // Is need to SUM COLUMN?
                if (meta.IsSum)
                {
                    // Is Have to sum
                    if (sumCols.TryGetValue(meta.Field, out var col))
                    {
                        string addressList = string.Join(",", col);
                        worksheet.Cell(dataCell).FormulaA1 = $"SUM({addressList})";
                    }
                    worksheet.Cell(dataCell).Style.NumberFormat.Format = "#,##0";
                    worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                }

                columnIndex++;
            }

            // Iterate All Extra Headers
            foreach (string extraHeader in requestQuery.ExtraHeaders)
            {
                string dataCell = $"{GetColumnName(columnIndex)}{rowIndex}";

                // Set default value & Style for cell
                worksheet.Cell(dataCell).FormulaA1 = "-";
                worksheet.Cell(dataCell).Style.Font.Bold = true;
                worksheet.Cell(dataCell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell(dataCell).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
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