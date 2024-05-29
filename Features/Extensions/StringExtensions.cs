using System.Text.RegularExpressions;

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

    /// <summary>
    /// String to Int
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int ToInt(this string input)
    {
        return int.Parse(input);
    }

    /// <summary>
    /// Checks if the current object, which is a regex pattern, matches the provided input string.
    /// </summary>
    /// <param name="pattern">The regex pattern to match against the input string.</param>
    /// <param name="input">The input string to be checked against the regex pattern.</param>
    /// <returns>True if the regex pattern matches the input string; otherwise, false.</returns>
    public static bool IsMatch(this string pattern, string input)
    {
        Regex regex = new Regex(pattern: pattern);
        return regex.Match(input).Success;
    }
}