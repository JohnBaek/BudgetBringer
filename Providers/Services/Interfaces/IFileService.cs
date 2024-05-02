using Microsoft.AspNetCore.Http;
using Models.Requests.Files;
using Models.Responses;
using Models.Responses.Files;

namespace Providers.Services.Interfaces;

/// <summary>
/// Common tempUploadedFiles interface
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Try upload files to temp folder 
    /// </summary>
    /// <param name="request">Form tempUploadedFiles</param>
    /// <returns></returns>
    Task<ResponseData<ResponseFileUpload>> UploadFileToTempPath(IFormFile request);
    
    /// <summary>
    /// Try persist files from temp path
    /// </summary>
    /// <param name="category"></param>
    /// <param name="tempUploadedFiles"></param>
    /// <returns></returns>
    Task<ResponseList<ResponseFileUpload>> PersistFiles(string category, List<RequestUploadFile> tempUploadedFiles);
}