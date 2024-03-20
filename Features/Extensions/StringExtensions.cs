using System.Security.Cryptography;
using System.Text;

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
    public static string ToSha(this string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        
        return builder.ToString();
    }
}