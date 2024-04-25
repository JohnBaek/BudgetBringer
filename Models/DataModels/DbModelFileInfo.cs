using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// Detail file info 
/// </summary>
[Table("FileInfo")]
public class DbModelFileInfo : DbModelDefault
{
    /// <summary>
    /// Id 
    /// </summary>
    [Key]
    public Guid Id { get; init; }
    
    /// <summary>
    /// Id of Group It could be null 
    /// </summary>
    public Guid? GroupId { get; init; }

    /// <summary>
    /// Display file name ex) 0cc5d32e-01fc-11ef-82ae-0242ac1c001e.extention
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string DisplayFileName { get; set; } = "";
    
    /// <summary>
    /// Origin of file Name ex) Report.xlsx 
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string OriginFileName { get; set; } = "";

    /// <summary>
    /// Extension of file ex) xlsx , txt ..
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Extension { get; set; } = "";

    /// <summary>
    /// /path/path/[OriginFileName]
    /// </summary>
    [MaxLength(255)]
    public string PublicFileUri { get; set; } = "";
    
    /// <summary>
    /// Real File Path
    /// </summary>
    [MaxLength(255)]
    public string InternalFilePath { get; set; } = "";

    /// <summary>
    /// Media Types
    /// </summary>
    [MaxLength(255)]
    public String MediaType { get; set; } = "";
    
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Checksum for file integrity verification
    /// </summary>
    [MaxLength(255)]
    public string Checksum { get; set; } = "";

    /// <summary>
    /// Tags for easier categorization and search
    /// </summary>
    [MaxLength(255)]
    public string Tags { get; set; } = "";
}