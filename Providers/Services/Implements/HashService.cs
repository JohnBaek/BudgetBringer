using System.Security.Cryptography;
using System.Text;
using Features.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// SHA 256 Service
/// </summary>
public class HashService : IHashService
{
    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<HashService> _logger;

    /// <summary>
    /// Configurations
    /// </summary>
    private readonly IConfiguration _configuration;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public HashService(ILogger<HashService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a SHA256 hash for the given input with an optional salt.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <param name="salt">An optional salt value to enhance security.</param>
    /// <returns>The SHA256 hash of the input with salt.</returns>
    public string ComputeHash(string input, string salt)
    {
        StringBuilder result = new StringBuilder();

        try
        {
            // Combine input with salt
            string saltedInput = input + salt;

            // Convert the salted input to a byte array
            byte[] bytes = Encoding.UTF8.GetBytes(saltedInput);

            // Compute the SHA-256 hash
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Convert the byte array to a hexadecimal string
            foreach (byte b in hashBytes)
            {
                result.Append(b.ToString("x2"));
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
            throw;
        }
        return result.ToString();
    }

    /// <summary>
    /// Generates a SHA256 hash for the given input with an optional salt.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <returns>The SHA256 hash of the input with salt.</returns>
    public string ComputeHash(string input)
    {
        try
        {
            return ComputeHash(input: input, salt: _configuration["HashSettings:Salt"] ?? "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}