using Features.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Files;
using Models.Responses;
using Models.Responses.Budgets;
using Models.Responses.Files;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// Implements of File Service
/// </summary>
public class FileService : IFileService
{
    /// <summary>
    /// Web Environment
    /// </summary>
    private readonly IWebHostEnvironment _hostEnvironment;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<FileService> _logger;
    
    /// <summary>
    /// SystemConfig Service
    /// </summary>
    private ISystemConfigService _systemConfigService;
    
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="systemConfigService"></param>
    /// <param name="logger">Logger</param>
    /// <param name="hostEnvironment"></param>
    /// <param name="dbContext"></param>
    /// <param name="userRepository"></param>
    public FileService(
          ISystemConfigService systemConfigService
        , ILogger<FileService> logger
        , IWebHostEnvironment hostEnvironment
        , AnalysisDbContext dbContext, IUserRepository userRepository)
    {
        _systemConfigService = systemConfigService;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Try upload files to temp folder 
    /// </summary>
    /// <param name="request">Form tempUploadedFiles</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseFileUpload>> UploadFileToTempPath(IFormFile request)
    {
        ResponseData<ResponseFileUpload> result;

        try
        {
            // Get Temp tempUploadedFiles upload path
            string tempFileUploadPath;
            
            // In Development
            if (_hostEnvironment.IsDevelopment())
            {
                tempFileUploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Caches", "Budget");
            }
            // Another 
            else
            {
                tempFileUploadPath = await _systemConfigService.GetValueAsync<string>("UPLOAD", "TEMP_PATH") ?? "";
            }
            
            // Does not have Path
            if(tempFileUploadPath.IsEmpty())
                return new ResponseData<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "경로를 찾을수 없습니다.", new ResponseFileUpload());

            // Empty tempUploadedFiles
            if (request.Length == 0)
                return new ResponseData<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "파일 내용을 확인해주세요", new ResponseFileUpload());
            
            // Get tempUploadedFiles extension
            string extension = Path.GetExtension(request.FileName);
            
            // Get Allowed tempUploadedFiles extension lists
            List<string> allowFileExtensions =
                (await _systemConfigService.GetValueAsync<string>("UPLOAD", "ALLOW_EXTENSIONS") ?? "").Split(",").ToList();

            // File is Not allowed
            if(allowFileExtensions.Any(i => i.ToLower() == extension.Replace(".","")) == false)
                return new ResponseData<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", $"[{extension}] 은 지원하지 않는 파일 형식입니다.\n[{string.Join(",",allowFileExtensions)}] 형식만 등록가능합니다.", new ResponseFileUpload());
            
            // Destination tempUploadedFiles Name ( GUID base )
            string tempFileName = $"{Guid.NewGuid()}{extension}";
            
            // Does not have Temp tempUploadedFiles upload path
            if (!Directory.Exists(tempFileUploadPath))
                // Create Directory
                Directory.CreateDirectory(tempFileUploadPath);
            
            // Result Destination
            string resultPath = Path.Combine(tempFileUploadPath, tempFileName);
            
            // Create tempUploadedFiles
            await using Stream stream = new FileStream(resultPath, FileMode.Create);

            try
            {
                // Create File
                await request.CopyToAsync(stream);
            }
            catch (Exception e)
            {
                e.LogError(_logger);
                return new ResponseData<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "파일 업로드중 문제가 발생했습니다.", new ResponseFileUpload());
            }
            
            // Result Trim
            ResponseFileUpload fileUploadInformation = new ResponseFileUpload
            {
                Name = tempFileName,
                OriginalFileName = request.FileName
            };
            result = new ResponseData<ResponseFileUpload>(EnumResponseResult.Success, "", "", fileUploadInformation);
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "처리중 예외가 발생했습니다.", new ResponseFileUpload());
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// Try persist files from temp path
    /// </summary>
    /// <param name="category"></param>
    /// <param name="tempUploadedFiles"></param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseFileUpload>> PersistFiles( string category , List<RequestUploadFile> tempUploadedFiles)
    {
        ResponseList<ResponseFileUpload> result;

        try
        {
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseList<ResponseFileUpload>{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // Get Temp tempUploadedFiles upload path
            string tempFileUploadPath;
            string persistFileUploadPath;
            
            // In Development
            if (_hostEnvironment.IsDevelopment())
            {
                tempFileUploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Caches", "Budget");
                persistFileUploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Caches", "Budget","Persist");
            }
            // Another 
            else
            {
                tempFileUploadPath = await _systemConfigService.GetValueAsync<string>("UPLOAD", "TEMP_PATH") ?? "";
                persistFileUploadPath = await _systemConfigService.GetValueAsync<string>("UPLOAD", "FILE_PATH") ?? "";
            }
            
            // Does not have Path
            if(tempFileUploadPath.IsEmpty())
                return new ResponseList<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "경로를 찾을수 없습니다.", null);

            // Check all files 
            foreach (RequestUploadFile file in tempUploadedFiles)
            {
                // Combine path 
                string path = Path.Combine(tempFileUploadPath, file.Name);
                
                // If Not
                if(!File.Exists(path))
                    return new ResponseList<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "경로를 찾을수 없습니다.", null);
            }

            // Store files
            List<DbModelFileInfo> willStoreDatabase = new List<DbModelFileInfo>();
            
            // Create Group GUID
            Guid fileGroupId = Guid.NewGuid();

            // Process all files
            foreach (RequestUploadFile file in tempUploadedFiles)
            {
                // Get File Info 
                string path = Path.Combine(tempFileUploadPath, file.Name);
                FileInfo fileInfo = new FileInfo(path);
                
                GetSeperatedFileUploadPath(category, persistFileUploadPath, fileInfo.Name, out string diskStaticPath, out string publicPath);
                
                // Get MimeType
                string mimeType = "";
                var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
                if (provider.TryGetContentType(fileInfo.FullName, out var contentType)) {
                    mimeType = contentType;
                } else {
                    mimeType = "application/octet-stream"; // 기본 MIME 타입
                }
                
                // Move file
                fileInfo.MoveTo(diskStaticPath);
                willStoreDatabase.Add(new DbModelFileInfo
                {
                    Id = Guid.NewGuid(),
                    GroupId = fileGroupId,
                    OriginFileName = fileInfo.Name ,
                    DisplayFileName = file.OriginalFileName,         
                    RegId = user.Id ,
                    ModId = user.Id ,
                    RegName = user.DisplayName,
                    ModName = user.DisplayName,
                    RegDate = DateTime.Now,
                    ModDate = DateTime.Now,
                    InternalFilePath = publicPath ,
                    Extension = fileInfo.Extension ,
                    MediaType = mimeType ,
                    PublicFileUri = $"files/{publicPath}" ,
                    Size = fileInfo.Length,
                    Tags = category ,
                    Checksum = fileInfo.CalculateChecksum()
                });
            }
            
            // Begin transaction
            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.FileInfos.AddRangeAsync(willStoreDatabase);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result = new ResponseList<ResponseFileUpload>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
                e.LogError(_logger);
            }

            List<ResponseFileUpload> uploadedItems = new List<ResponseFileUpload>();
            foreach (var fileInfo in willStoreDatabase)
            {
                uploadedItems.Add(new ResponseFileUpload
                {
                    Id = fileInfo.Id ,
                    Name = fileInfo.DisplayFileName ,
                    OriginalFileName = fileInfo.OriginFileName ,
                    Url = fileInfo.PublicFileUri 
                });    
            }

            return new ResponseList<ResponseFileUpload>(EnumResponseResult.Success, "", "", uploadedItems);
        }
        catch (Exception e)
        {
            result = new ResponseList<ResponseFileUpload>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "처리중 예외가 발생했습니다.", null);
            e.LogError(_logger);
        }

        return result;
    }

    
    /// <summary>
    /// Generate File Paths 
    /// </summary>
    /// <param name="category"></param>
    /// <param name="realFileRootPath"></param>
    /// <param name="fileName"></param>
    /// <param name="realPath"></param>
    /// <param name="publicPath"></param>
    private void GetSeperatedFileUploadPath(string category, string realFileRootPath, string fileName, out string realPath,out string publicPath)
    {
        // Get Today
        DateTime today = DateTime.Now;
        string hierarchy = Path.Combine(category, today.Year.ToString("0000"), today.Month.ToString("00"), today.Day.ToString("00"));
        
        // Create Public Path ( URL )
        publicPath = Path.Combine(hierarchy, fileName);
        
        // Create Real Path
        realPath = Path.Combine(realFileRootPath, publicPath);
    }
}

