using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;
using Models.Responses.Files;
using Providers.Services.Interfaces;

namespace Apis.Controllers;

/// <summary>
/// File Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class FileController : Controller
{
    /// <summary>
    /// 파일 서비스 
    /// </summary>
    private readonly IFileService _fileService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="fileService">File Service</param>
    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Try Upload file
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    [HttpPost("Upload")]
    public async Task<ResponseData<ResponseFileUpload>> UploadFile(IFormFile formFile)
    {
         return await _fileService.UploadFileToTempPath(formFile);
    }
}