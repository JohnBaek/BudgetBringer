using Models.Common.Enums;

namespace Models.Requests.Query;

/// <summary>
/// Query Search Meta Informations 
/// </summary>
public class RequestQuerySearchMeta
{
    /// <summary>
    /// Type of Query search
    /// </summary>
    public EnumQuerySearchType SearchType { get; set; } = EnumQuerySearchType.Equals;

    /// <summary>
    /// Field Name
    /// </summary>
    public string Field { get; set; } = "";
    
    /// <summary>
    /// Excel Header Name
    /// </summary>
    public string ExcelHeaderName { get; set; } = "";

    /// <summary>
    /// True: Include Excel Columns
    /// </summary>
    public bool IsIncludeExcelHeader { get; set; } = false;

    /// <summary>
    /// Value of Sum
    /// </summary>
    public double Sum { get; set; } = 0;

    /// <summary>
    /// Is Have to Sum?
    /// </summary>
    public bool isSum { get; set; }

    /// <summary>
    /// If value has EnumType this will be not null
    /// </summary>
    public Type? EnumType { get; set; } = null;
}