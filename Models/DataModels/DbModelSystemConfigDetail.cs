using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// Detail of Configs
/// </summary>
[Table("SystemConfigDetails")]
public class DbModelSystemConfigDetail
{
    /// <summary>
    /// Parent Config Id
    /// </summary>
    public Guid ConfigId { get; init; }
    
    /// <summary>
    /// Id 
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Name of detail config
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = "";

    /// <summary>
    /// Value of detail config
    /// </summary>
    [MaxLength(1024)]
    [Required]
    public string Value { get; set; } = "";
    
    /// <summary>
    /// Descriptions
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Parent Table 
    /// </summary>
    [ForeignKey("ConfigId")]
    public virtual DbModelSystemConfig? SystemConfig { get; set; }
}