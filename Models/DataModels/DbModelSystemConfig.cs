using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DataModels;

/// <summary>
/// System Common Config 
/// </summary>
[Table("SystemConfigs")]
public class DbModelSystemConfig
{
    /// <summary>
    /// Id 
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Name Of Config
    /// </summary>
    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = ""; 
    
    /// <summary>
    /// Descriptions
    /// </summary>
    [MaxLength(3000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Details of configs
    /// </summary>
    public virtual ICollection<DbModelSystemConfigDetail>? Details { get; init; }
}