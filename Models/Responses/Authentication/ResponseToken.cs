namespace Models.Responses.Authentication;

/// <summary>
/// Represent of JWT token model
/// </summary>
public class ResponseToken
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = "";

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = "";
}