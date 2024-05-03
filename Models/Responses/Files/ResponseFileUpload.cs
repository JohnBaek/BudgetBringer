namespace Models.Responses.Files;

/// <summary>
/// Response File Uploaded
/// </summary>
public class ResponseFileUpload
{
    /// <summary>
    /// Id of file 
    /// </summary>
    public Guid? Id { get; init; }
    
    /// <summary>
    /// Name of file
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Name of origin file Name
    /// </summary>
    public string OriginalFileName { get; set; } = "";

    /// <summary>
    /// Url
    /// </summary>
    public string Url { get; set; } = "";
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long Size { get; set; }
}