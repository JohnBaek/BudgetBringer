using System.ComponentModel;
using System.Reflection;
using Features.Attributes;

namespace Features.Extensions;

/// <summary>
/// Extensions of Enum
/// </summary>
public static class EnumExtensions
{
   /// <summary>
   /// To Returns Description Attribute value of Enum
   /// </summary>
   /// <param name="value"></param>
   /// <returns></returns>
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        
        // Is field null
        if (field == null)
            return "";
        
        // Get Description Attributes
        DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }

    /// <summary>
    /// To Returns Colors Attribute value of Enum
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetColor(this Enum enumValue)
    {
        FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = (EnumColorAttribute)fieldInfo?.GetCustomAttributes(typeof(EnumColorAttribute), false).FirstOrDefault()!;
        return attribute.ColorCode; // Default to white if no color is defined
    }
}