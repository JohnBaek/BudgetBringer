using Microsoft.AspNetCore.Http;
using Models.DataModels;
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
    Task<ResponseData<ResponseFileUpload>> UploadFileToTempPathAsync(IFormFile request);
    
    /// <summary>
    /// Try persist files from temp path
    /// </summary>
    /// <param name="category"></param>
    /// <param name="tempUploadedFiles"></param>
    /// <returns></returns>
    Task<ResponseList<ResponseFileUpload>> PersistFilesAsync(string category, List<RequestUploadFile> tempUploadedFiles);

    /// <summary>
    /// Try persist files from temp path
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="category"></param>
    /// <param name="tempUploadedFiles"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<Guid?> PersistFilesAsync(AnalysisDbContext dbContext, string category, List<RequestUploadFile> tempUploadedFiles, Guid? groupId );

    /// <summary>
    /// Get Files
    /// </summary>
    /// <param name="fileGroupId"></param>
    /// <returns></returns>
    Task<ResponseList<ResponseFileUpload>> GetFilesAsync(Guid fileGroupId);

    /// <summary>
    /// Remove files
    /// </summary>
    /// <param name="fileIds"></param>
    /// <returns></returns>
    Task<Response> RemoveFilesAsync(List<Guid> fileIds);
}