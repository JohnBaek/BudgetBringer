using System.Security.Cryptography;

namespace Features.Extensions;

/// <summary>
/// 문자열 확장
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// SHA 로 해싱한다.
    /// </summary>
    /// <param name="input">대상 문자열</param>
    /// <returns>해싱 결과</returns>
    public static string WithDateTime(this string input)
    {
        return $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] {input}";
    }
    
    /// <summary>
    /// Guid 로 파싱한다.
    /// </summary>
    /// <param name="input">대상 문자열</param>
    /// <returns>해싱 결과</returns>
    public static Guid ToGuid(this string input)
    {
        return Guid.Parse(input);
    }
    
    /// <summary>
    /// 데이터가 Null 또는 비어있는 경우 True
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
}