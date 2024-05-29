namespace Providers.Services.Interfaces;

/// <summary>
/// SHA 256 Service
/// </summary>
public interface IHashService
{
    /// <summary>
    /// Generates a SHA256 hash for the given input with an optional salt.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <param name="salt">An optional salt value to enhance security.</param>
    /// <returns>The SHA256 hash of the input with salt.</returns>
    string ComputeHash(string input, string salt);

    /// <summary>
    /// Generates a SHA256 hash for the given input with an optional salt.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <returns>The SHA256 hash of the input with salt.</returns>
    string ComputeHash(string input);
}