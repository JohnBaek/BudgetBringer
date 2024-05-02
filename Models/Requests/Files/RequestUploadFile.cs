namespace Models.Requests.Files;

/// <summary>
/// File Upload Model
/// </summary>
public class RequestUploadFile
{
    /// <summary>
    /// Name of file
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Name of origin file Name
    /// </summary>
    public string OriginalFileName { get; set; } = "";
}