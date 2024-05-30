using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using ClosedXML.Excel;
using Features.Debounce;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Files;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Models.Responses.Files;
using Providers.Repositories.Interfaces;
using Providers.Services.Implements;
using Providers.Services.Interfaces;

namespace Providers.Repositories.Implements;


[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class BudgetPlanRepository : IBudgetPlanRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<BudgetPlanRepository> _logger;
    
    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 쿼리 서비스
    /// </summary>
    private readonly IQueryService _queryService;

    /// <summary>
    /// 액션 로그 기록 서비스
    /// </summary>
    private readonly ILogActionWriteService _logActionWriteService;

    /// <summary>
    /// 로그 카테고리명
    /// </summary>
    private const string LogCategory = "[BudgetPlan]";
    
    /// <summary>
    /// 디스패처 서비스
    /// </summary>
    private readonly IDispatchService _dispatchService;

    /// <summary>
    /// 디바운서
    /// </summary>
    private readonly DebounceManager _debounceManager;

    /// <summary>
    /// File Service
    /// </summary>
    private readonly IFileService _fileService;
    
    /// <summary>
    /// SystemConfig Service
    /// </summary>
    private ISystemConfigService _systemConfigService;

    /// <summary>
    /// Host Environment
    /// </summary>
    private IHostEnvironment _hostEnvironment;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="queryService">쿼리 서비스</param>
    /// <param name="userRepository">유저 리파지토리</param>
    /// <param name="logActionWriteService">액션 로그 기록 서비스</param>
    /// <param name="dispatchService">디스패처 서비스</param>
    /// <param name="debounceManager"></param>
    /// <param name="fileService"></param>
    /// <param name="systemConfigService"></param>
    public BudgetPlanRepository(
        ILogger<BudgetPlanRepository> logger
        , AnalysisDbContext dbContext
        , IQueryService queryService
        , IUserRepository userRepository
        , ILogActionWriteService logActionWriteService
        , IDispatchService dispatchService
        , DebounceManager debounceManager, IFileService fileService, ISystemConfigService systemConfigService, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _dbContext = dbContext;
        _queryService = queryService;
        _userRepository = userRepository;
        _logActionWriteService = logActionWriteService;
        _dispatchService = dispatchService;
        _debounceManager = debounceManager;
        _fileService = fileService;
        _systemConfigService = systemConfigService ?? throw new ArgumentNullException(nameof(systemConfigService));
        _hostEnvironment = hostEnvironment;
    }

    
    /// <summary>
    /// 셀렉터 매핑 정의
    /// </summary>
    private Expression<Func<DbModelBudgetPlan, ResponseBudgetPlan>> MapDataToResponse { get; init; } = item => new ResponseBudgetPlan
        {
            Id = item.Id,
            IsAbove500K = item.IsAbove500K ,
            BaseYearForStatistics = item.BaseYearForStatistics ,
            ApprovalDate = item.ApprovalDate ,
            Description = item.Description ,
            SectorId = item.SectorId ,
            BusinessUnitId = item.BusinessUnitId ,
            CostCenterId = item.CostCenterId ,
            CountryBusinessManagerId = item.CountryBusinessManagerId ,
            SectorName = item.SectorName ,
            CostCenterName = item.CostCenterName ,
            CountryBusinessManagerName = item.CountryBusinessManagerName ,
            BusinessUnitName = item.BusinessUnitName ,
            BudgetTotal = item.BudgetTotal ,
            OcProjectName = item.OcProjectName ,
            BossLineDescription = item.BossLineDescription ,
            IsIncludeInStatistics = item.IsIncludeInStatistics,
            RegName = item.RegName ,
            ModName = item.ModName ,
            RegDate = item.RegDate ,
            ModDate = item.ModDate ,
            FileGroupId = item.FileGroupId ,
        };
    
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseBudgetPlan>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseBudgetPlan> result = new ResponseList<ResponseBudgetPlan>();
        try
        {
            result = await _queryService.ToResponseListAsync(requestQuery, MapDataToResponse);
            
            // Avoid Null
            if (result.Items == null)
                result.Items = new List<ResponseBudgetPlan>();
            
            // Process all attached files
            foreach (ResponseBudgetPlan item in result.Items)
            {
                // Does not have attached files
                if(item.FileGroupId == null)
                    continue;
                
                // Avoid Null
                var attachedFiles = await _fileService.GetFilesAsync(item.FileGroupId.Value);
                if(attachedFiles.Items != null)
                    item.AttachedFiles = attachedFiles.Items;
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseBudgetPlan>> GetAsync(string id)
    {
        ResponseData<ResponseBudgetPlan> result = new ResponseData<ResponseBudgetPlan>();
        try
        {
            // 요청이 유효하지 않은경우
            if (id.IsEmpty())
                return new ResponseData<ResponseBudgetPlan>(EnumResponseResult.Error,"ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요",null);

            // 데이터를 조회한다.
            ResponseBudgetPlan? data = await _dbContext.BudgetPlans
                .Where(i => i.Id == id.ToGuid())
                .Select(MapDataToResponse) // MapDataToResponse 셀렉터를 적용
                .FirstOrDefaultAsync();
            
            // If Has files
            if (data is { FileGroupId: not null })
            {
                Guid fileGroupId = data.FileGroupId.Value; 
                ResponseList<ResponseFileUpload> responseFiles = await _fileService.GetFilesAsync(fileGroupId);
                if (responseFiles.Success)
                    data.AttachedFiles = responseFiles.Items!;
            }
            
            // 조회된 데이터가 없다면
            if(data == null)
                return new ResponseData<ResponseBudgetPlan>(EnumResponseResult.Error,"ERROR_IS_NONE_EXIST", "대상이 존재하지 않습니다.",null);

            // 데이터를 복사한다.
            return new ResponseData<ResponseBudgetPlan> {Result = EnumResponseResult.Success, Data = data};
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 데이터를 업데이트한다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <param name="request">요청정보</param>
    /// <returns></returns>
    public async Task<Response> UpdateAsync(string id , RequestBudgetPlan request)
    {
        Response result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(id.IsEmpty() || request.IsInValid())
                return new Response{ Code = "ERROR_INVALID_PARAMETER", Message = "필수 값을 입력해주세요"};
            
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 동일한 이름을 가진 데이터가 있는지 확인
            DbModelBudgetPlan? update = await        _dbContext.BudgetPlans
                .Where(i => i.Id == id.ToGuid())
                .FirstOrDefaultAsync();
            
            // 대상 데이터가 없는경우
            if(update == null)
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "대상이 존재하지 않습니다."};
            
            // 로그기록을 위한 데이터 스냅샷
            DbModelBudgetPlan snapshot = update.ToClone()!;
            
            // 데이터를 바인딩한다
            Response bidingResult = await SetBudgetPlanDispatchValidatorAsync(update, user , request);


            if (bidingResult.Result != EnumResponseResult.Success)
                return bidingResult;
            
            // Has List of persist files
            if (request.AttachedFiles.Count > 0)
            {
                Guid? fileGroupId = await _fileService.PersistFilesAsync(_dbContext, LogCategory, request.AttachedFiles, update.FileGroupId);
                if (fileGroupId != null)
                    update.FileGroupId = fileGroupId;
            }
            
            // 데이터베이스에 업데이트처리 
            _dbContext.BudgetPlans.Update(update);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response{ Result = EnumResponseResult.Success};
            
            // 로그 기록
            await _logActionWriteService.WriteUpdate(snapshot.FromCopyValue<ResponseBudgetPlan>(), update.FromCopyValue<ResponseBudgetPlan>(), user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }
    
        return result;
    }
   
    /// <summary>
    /// 데이터를 추가한다.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseBudgetPlan>> AddAsync(RequestBudgetPlan request)
    {
        ResponseData<ResponseBudgetPlan> result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<ResponseBudgetPlan>{ Code = "ERROR_INVALID_PARAMETER", Message = request.GetFirstErrorMessage()};

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseBudgetPlan>{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
          
            // 데이터를 생성한다.
            DbModelBudgetPlan add = new DbModelBudgetPlan
            {
                Id = Guid.NewGuid() ,
                CostCenterName = "",
                CountryBusinessManagerName = "",
                BusinessUnitName = "",
                RegName = "",
                ModName = ""
            };

            // 데이터를 바인딩한다
            Response bidingResult = await SetBudgetPlanDispatchValidatorAsync(add, user , request);
            add.RegName = user.DisplayName;
            add.RegDate = DateTime.Now;
            add.RegId = user.Id;

            // 바인딩에 실패한 경우
            if (bidingResult.Result != EnumResponseResult.Success)
                return new ResponseData<ResponseBudgetPlan>{ Code = bidingResult.Code, Message = bidingResult.Message };
            
            // Has List of persist files
            if (request.AttachedFiles.Count > 0)
            {
                Guid? fileGroupId = await _fileService.PersistFilesAsync(_dbContext, LogCategory, request.AttachedFiles, null);
                if (fileGroupId != null)
                    add.FileGroupId = fileGroupId;
            }

            // 데이터베이스에 데이터 추가 
            await _dbContext.BudgetPlans.AddAsync(add);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            
            // 추가된 데이터를 조회
            ResponseData<ResponseBudgetPlan> added = await GetAsync(add.Id.ToString());
            result = new ResponseData<ResponseBudgetPlan>{ Result = EnumResponseResult.Success , Data = added.Data };
            
    
            // 로그 기록
            await _logActionWriteService.WriteAddition(add.FromCopyValue<ResponseBudgetPlan>(), user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new ResponseData<ResponseBudgetPlan>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }
    
        return result;
    }
    

    /// <summary>
    /// Get import Preview
    /// </summary>
    /// <param name="uploadFile"></param>
    /// <returns></returns>
    public async Task<ResponseList<RequestBudgetPlanExcelImport>> GetImportPreview(RequestUploadFile uploadFile)
    {
        ResponseList<RequestBudgetPlanExcelImport> result;
 
        try
        {
            // Get root TempFile Path
            string? tempPath = await _systemConfigService.GetValueAsync<string>("UPLOAD", "TEMP_PATH");
            
            // In Development
            if (_hostEnvironment.IsDevelopment())
            {
                tempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Caches", "Budget");
            }
            
            if (tempPath == null)
                tempPath = "";

            // Combine result Path
            string filePath = Path.Combine(tempPath, uploadFile.Name);

            // Get File object
            FileInfo importedFile = new FileInfo(filePath);
            
            // Does not have file
            if (!importedFile.Exists)
                return new ResponseList<RequestBudgetPlanExcelImport>(EnumResponseResult.Error, "", "업로드된 파일 찾기에 실패했습니다.", null);

            // Get excel instance
            using IXLWorkbook workbook = new XLWorkbook(filePath);

            // Get First 
            IXLWorksheet worksheet = workbook.Worksheets.FirstOrDefault()!;

            List<RequestBudgetPlanExcelImport> items = new List<RequestBudgetPlanExcelImport>();
            
            // Process all
            foreach (IXLRow row in worksheet.Rows().Skip(1))
            {
                try
                {
                    RequestBudgetPlanExcelImport add = new RequestBudgetPlanExcelImport();
                    add.ApprovalDate = row.Cell(1).Value.ToString();
                    add.BaseYearForStatistics = int.Parse(row.Cell(2).Value.ToString());
                    add.IsIncludeInStatistics = row.Cell(3).Value.ToString() == "포함";
                    add.Description = row.Cell(4).Value.ToString();

                    // Get Cost Center Name
                    string costCenterName = row.Cell(5).Value.ToString();
                    add.CostCenterName = costCenterName;
                    string costCenterId = await _dispatchService.GetValueFromAsync<DbModelCostCenter>
                        (nameof(DbModelCostCenter.Value), nameof(DbModelCountryBusinessManager.Id), costCenterName);
                    
                    // Cannot Find
                    if (costCenterId.IsEmpty())
                    {
                        items.Add(new RequestBudgetPlanExcelImport()
                        {
                            Result = EnumResponseResult.Error ,
                            Message = $"코스트센터 : [{costCenterName}] 을 찾을수 없습니다."
                        });
                        continue;
                    }
                    add.CostCenterId = costCenterId.ToGuid();
                    
                    // Get CountryBusiness Manager Name
                    string countryBusinessManagerName = row.Cell(6).Value.ToString();
                    add.CountryBusinessManagerName = countryBusinessManagerName;
                    string countryBusinessManagerId = await _dispatchService.GetValueFromAsync<DbModelCountryBusinessManager>
                        (nameof(DbModelCountryBusinessManager.Name), nameof(DbModelCountryBusinessManager.Id), countryBusinessManagerName);
                    
                    // Cannot Find
                    if (countryBusinessManagerId.IsEmpty())
                    {
                        items.Add(new RequestBudgetPlanExcelImport()
                        {
                            Result = EnumResponseResult.Error ,
                            Message = $"컨트리비지니스 매니저 : [{countryBusinessManagerName}] 을 찾을수 없습니다."
                        });
                        continue;
                    }
                    add.CountryBusinessManagerId = countryBusinessManagerId.ToGuid();
                    
                    // Get Sector
                    string sectorName = row.Cell(7).Value.ToString();
                    add.SectorName = sectorName;
                    string sectorId = await _dispatchService.GetValueFromAsync<DbModelSector>
                        (nameof(DbModelSector.Value), nameof(DbModelSector.Id), sectorName);
                    
                    // Cannot Find
                    if (sectorId.IsEmpty())
                    {
                        items.Add(new RequestBudgetPlanExcelImport()
                        {
                            Result = EnumResponseResult.Error ,
                            Message = $"컨트리비지니스 매니저 : [{sectorName}] 을 찾을수 없습니다."
                        });
                        continue;
                    }
                    add.SectorId = sectorId.ToGuid();
                    
                    // Get Business Unit
                    string businessUnitName = row.Cell(8).Value.ToString();
                    add.BusinessUnitName = businessUnitName;
                    string businessUnitId = await _dispatchService.GetValueFromAsync<DbModelBusinessUnit>
                        (nameof(DbModelBusinessUnit.Name), nameof(DbModelBusinessUnit.Id), businessUnitName);
                    // Cannot Find
                    if (businessUnitId.IsEmpty())
                    {
                        items.Add(new RequestBudgetPlanExcelImport()
                        {
                            Result = EnumResponseResult.Error ,
                            Message = $"컨트리비지니스 매니저 : [{businessUnitName}] 을 찾을수 없습니다."
                        });
                        continue;
                    }
                    add.BusinessUnitId = businessUnitId.ToGuid();
                    add.BudgetTotal = double.Parse(row.Cell(9).Value.ToString());
                    add.OcProjectName = row.Cell(10).Value.ToString();
                    add.BossLineDescription = row.Cell(11).Value.ToString();
                    add.Result = EnumResponseResult.Success;
                    
                    items.Add(add);
                }
                catch (Exception e)
                {
                    items.Add(new RequestBudgetPlanExcelImport()
                    {
                        Result = EnumResponseResult.Error ,
                        Message = $"처리중 예외가 발생했습니다."
                    });
                }
            }
            
            result = new ResponseList<RequestBudgetPlanExcelImport>(EnumResponseResult.Success, "", "", items);
        }
        catch (Exception e)
        {
            result = new ResponseList<RequestBudgetPlanExcelImport>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }
    
        return result;
    }


    /// <summary>
    /// Add Multiple Request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseData<ResponseBudgetPlan>>> AddListAsync(List<RequestBudgetPlan> request)
    {
        ResponseList<ResponseData<ResponseBudgetPlan>> result;
        try
        {
            List<ResponseData<ResponseBudgetPlan>> storedResult = new List<ResponseData<ResponseBudgetPlan>>();
            
            // Proces all 
            foreach (RequestBudgetPlan requestBudgetPlan in request)
            {
                storedResult.Add(await AddAsync(requestBudgetPlan));
            }

            return new ResponseList<ResponseData<ResponseBudgetPlan>>(EnumResponseResult.Success, "", "", storedResult);
        }
        catch (Exception e)
        {
            result = new ResponseList<ResponseData<ResponseBudgetPlan>>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 데이터를 삭제한다.
    /// </summary>
    /// <param name="id">대상 아이디값</param>
    /// <returns></returns>
    public async Task<Response> DeleteAsync(string id)
    {
        Response result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(id.IsEmpty())
                return new Response(EnumResponseResult.Error, "ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요");

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new Response(EnumResponseResult.Error, "ERROR_SESSION_TIMEOUT", "로그인 상태를 확인해주세요");
            
            // 기존데이터를 조회한다.
            DbModelBudgetPlan? remove =
                await _dbContext.BudgetPlans.Where(i => i.Id == id.ToGuid()).FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(remove == null)
                return new Response(EnumResponseResult.Error, "ERROR_IS_NONE_EXIST", "대상이 존재하지 않습니다.");

            // 대상을 삭제한다.
            _dbContext.Remove(remove);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success,"","");
            
            // 로그 기록
            await _logActionWriteService.WriteDeletion(remove.FromCopyValue<ResponseBudgetPlan>(), user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }
    
        return result;
    }
    
     /// <summary>
    /// 인서트모델에 대한 유효성 검증및 데이터 추가
    /// </summary>
    /// <param name="model"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<Response> SetBudgetPlanDispatchValidatorAsync(DbModelBudgetPlan model, DbModelUser user, RequestBudgetPlan request)
    {
        Response result;
        try
        {
            // 기안일이 정상적인 Date 데이터인지 여부 
            bool isApprovalDateValid = DateOnly.TryParse(request.ApprovalDate, out DateOnly approvalDate);
            
            // 코스트센터명 조회
            string costCenterName = await _dispatchService.GetNameByIdAsync<DbModelCostCenter>
                (nameof(DbModelCostCenter.Id), nameof(DbModelCostCenter.Value), request.CostCenterId);
            // 코스트센터명이 조회되지 않는경우 
            if(costCenterName.IsEmpty())
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "코스트센터가 존재하지 않습니다."};
            
            // 컨트리 비지니스 매니저명 조회
            string countryBusinessManagerName = await _dispatchService.GetNameByIdAsync<DbModelCountryBusinessManager>
                (nameof(DbModelCountryBusinessManager.Id), nameof(DbModelCountryBusinessManager.Name), request.CountryBusinessManagerId);
            // 컨트리 비지니스 매니저 가 조회되지 않는경우 
            if(countryBusinessManagerName.IsEmpty())
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "컨트리 비지니스 매니저가 존재하지 않습니다."};

            // 비지니스유닛 명 조회
            string businessUnitName = await _dispatchService.GetNameByIdAsync<DbModelBusinessUnit>
                (nameof(DbModelBusinessUnit.Id), nameof(DbModelBusinessUnit.Name), request.BusinessUnitId);
            // 비지니스유닛이 조회되지 않는경우 
            if(businessUnitName.IsEmpty())
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "비지니스유닛이 존재하지 않습니다."};
            
            // 섹터값 조회
            string sectorName = await _dispatchService.GetNameByIdAsync<DbModelSector>
                (nameof(DbModelSector.Id), nameof(DbModelSector.Value), request.SectorId);
            // 섹터값이 조회되지 않을경우 
            if(sectorName.IsEmpty())
                return new Response{ Code = "ERROR_INVALID_PARAMETER", Message = "섹터정보가 올바르지 않습니다."};
            
            model.IsAbove500K = request.IsAbove500K;
            model.ApprovalDate = request.ApprovalDate;
            model.BaseYearForStatistics = request.BaseYearForStatistics;
            model.Description = request.Description;
            model.SectorId = request.SectorId;
            model.BusinessUnitId = request.BusinessUnitId;
            model.CostCenterId = request.CostCenterId;
            model.CountryBusinessManagerId = request.CountryBusinessManagerId;
            model.BusinessUnitName = businessUnitName;
            model.CostCenterName = costCenterName;
            model.CountryBusinessManagerName = countryBusinessManagerName;
            model.SectorName = sectorName;
            model.BudgetTotal = request.BudgetTotal;
            model.OcProjectName = request.OcProjectName;
            model.BossLineDescription = request.BossLineDescription;
            model.IsIncludeInStatistics = request.IsIncludeInStatistics;
            model.ModName = user.DisplayName;
            model.ModDate = DateTime.Now;
            model.ModId = user.Id;
            return new Response() {Result = EnumResponseResult.Success};
        }
        catch (Exception e)
        {
            result = new Response { Code = "ERROR_DATABASE", Message = "처리중 예외가 발생했습니다.", Result = EnumResponseResult.Error };
            e.LogError(_logger);
        }

        return result;
    }
}
