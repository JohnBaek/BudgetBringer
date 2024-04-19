using System.Linq.Expressions;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Budgets;
using Models.Requests.Query;
using Models.Responses;
using Models.Responses.Budgets;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Repositories.Implements;

/// <summary>
/// 예산 승인 리파지토리
/// </summary>
public class BudgetApprovedRepository : IBudgetApprovedRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<BudgetApprovedRepository> _logger;
    
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
    private const string LogCategory = "[BudgetApproved]";

    /// <summary>
    /// 디스패처 서비스
    /// </summary>
    private readonly IDispatchService _dispatchService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="queryService">쿼리 서비스</param>
    /// <param name="userRepository">유저 리파지토리</param>
    /// <param name="logActionWriteService">액션 로그 기록 서비스</param>
    /// <param name="dispatchService">디스패처 서비스</param>
    public BudgetApprovedRepository(
        ILogger<BudgetApprovedRepository> logger
        , AnalysisDbContext dbContext
        , IQueryService queryService
        , IUserRepository userRepository
        , ILogActionWriteService logActionWriteService
        , IDispatchService dispatchService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _queryService = queryService;
        _userRepository = userRepository;
        _logActionWriteService = logActionWriteService;
        _dispatchService = dispatchService;
    }

    /// <summary>
    /// 셀렉터 매핑 정의
    /// </summary>
    private Expression<Func<DbModelBudgetApproved, ResponseBudgetApproved>> MapDataToResponse { get; init; } = item => new ResponseBudgetApproved
    {
        Id = item.Id,
        IsAbove500K = item.IsAbove500K,
        ApprovalDate = item.ApprovalDate ,
        Description = item.Description,
        SectorId = item.SectorId,
        BusinessUnitId = item.BusinessUnitId,
        CostCenterId = item.CostCenterId,
        CountryBusinessManagerId = item.CountryBusinessManagerId,
        SectorName = item.SectorName,
        CostCenterName = item.CostCenterName,
        CountryBusinessManagerName = item.CountryBusinessManagerName,
        BusinessUnitName = item.BusinessUnitName,
        PoNumber = item.PoNumber,
        ApprovalStatus = item.ApprovalStatus,
        ApprovalAmount = item.ApprovalAmount,
        Actual = item.Actual,
        OcProjectName = item.OcProjectName,
        BossLineDescription = item.BossLineDescription,
        IsApproved = item.IsApproved,
        RegName = item.RegName ,
        ModName = item.ModName ,
        RegDate = item.RegDate ,
        ModDate = item.ModDate ,
    };
   
    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseBudgetApproved>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseBudgetApproved> result;
        try
        {
            // 결과를 반환한다.
            return await _queryService.ToResponseListAsync(requestQuery, MapDataToResponse);
        }
        catch (Exception e)
        {
            result = new ResponseList<ResponseBudgetApproved>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }
        
        return result;
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseBudgetApproved>> GetAsync(string id)
    {
        ResponseData<ResponseBudgetApproved> result = new ResponseData<ResponseBudgetApproved>();
        try
        {
            // 요청이 유효하지 않은경우
            if (id.IsEmpty())
                return new ResponseData<ResponseBudgetApproved>(EnumResponseResult.Error,"ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요",null);

            // 기존데이터를 조회한다.
            ResponseBudgetApproved? data =
                await _dbContext.BudgetApproved.Where(i => i.Id == id.ToGuid())
                    .Select(MapDataToResponse).FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(data == null)
                return new ResponseData<ResponseBudgetApproved>(EnumResponseResult.Error,"ERROR_IS_NONE_EXIST", "대상이 존재하지 않습니다.",null);

            return new ResponseData<ResponseBudgetApproved> {Result = EnumResponseResult.Success, Data = data};
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
    public async Task<Response> UpdateAsync(string id , RequestBudgetApproved request)
    {
        Response result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(id.IsEmpty() || request.IsInValid())
                return new Response(EnumResponseResult.Success,"ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요");
            
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new Response(EnumResponseResult.Success,"ERROR_SESSION_TIMEOUT", "로그인 상태를 확인해주세요");
            
            // 동일한 이름을 가진 데이터가 있는지 확인
            DbModelBudgetApproved? update = await _dbContext.BudgetApproved
                .Where(i => i.Id == id.ToGuid())
                .FirstOrDefaultAsync();
            
            // 대상 데이터가 없는경우
            if(update == null)
                return new Response(EnumResponseResult.Success,"ERROR_TARGET_DOES_NOT_FOUND", "대상이 존재하지 않습니다.");
            
            // 로그기록을 위한 데이터 스냅샷
            DbModelBudgetApproved snapshot = update.ToClone()!;

            // 데이터를 바인딩한다
            Response bidingResult = await SetBudgetPlanDispatchValidatorAsync(update, user , request);
            
            if (bidingResult.Result != EnumResponseResult.Success)
                return bidingResult;
            
            // 데이터베이스에 업데이트처리 
            _dbContext.BudgetApproved.Update(update);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success,"","");
            
            // 로그 기록
            await _logActionWriteService.WriteUpdate(snapshot.FromCopyValue<ResponseBudgetApproved>(), update.FromCopyValue<ResponseBudgetApproved>(), user , "",LogCategory);
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
    public async Task<ResponseData<ResponseBudgetApproved>> AddAsync(RequestBudgetApproved request)
    {
        ResponseData<ResponseBudgetApproved> result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<ResponseBudgetApproved>{ Code = "ERROR_INVALID_PARAMETER", Message = request.GetFirstErrorMessage()};

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseBudgetApproved>{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};

            // 데이터를 생성한다.
            DbModelBudgetApproved add = new DbModelBudgetApproved
            {
                Id = Guid.NewGuid(),
                RegName = "",
                ModName = "",
            };
            
            // 데이터를 바인딩한다
            Response bidingResult = await SetBudgetPlanDispatchValidatorAsync(add, user , request);
            
            // 바인딩에 실패한 경우
            if (bidingResult.Result != EnumResponseResult.Success)
                return new ResponseData<ResponseBudgetApproved>{ Code = bidingResult.Code, Message = bidingResult.Message };
            
            // 데이터베이스에 데이터 추가 
            await _dbContext.BudgetApproved.AddAsync(add);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            
            // 추가된 데이터를 조회
            ResponseData<ResponseBudgetApproved> added = await GetAsync(add.Id.ToString());
            
            result = new ResponseData<ResponseBudgetApproved>{ Result = EnumResponseResult.Success , Data = added.Data };
            
            // 로그 기록
            await _logActionWriteService.WriteAddition(add, user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new ResponseData<ResponseBudgetApproved>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
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
                return new Response{ Code = "ERROR_INVALID_PARAMETER", Message = "필수 값을 입력해주세요"};

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 기존데이터를 조회한다.
            DbModelBudgetApproved? remove =
                await _dbContext.BudgetApproved.Where(i => i.Id == id.ToGuid()).FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(remove == null)
                return new Response{ Code = "ERROR_IS_NONE_EXIST", Message = "대상이 존재하지 않습니다."};

            // 대상을 삭제한다.
            _dbContext.Remove(remove);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success,"","");
            
            // 로그 기록
            await _logActionWriteService.WriteDeletion(remove.FromCopyValue<ResponseBudgetApproved>(), user , "",LogCategory);
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
    private async Task<Response> SetBudgetPlanDispatchValidatorAsync(DbModelBudgetApproved model, DbModelUser user, RequestBudgetApproved request)
    {
        Response result;
        try
        {
            model.IsApprovalDateValid = false;
            model.ApproveDateValue = null;
            model.Year = "";
            model.Month = "";
            model.Day = "";
            model.IsApproved = false;
            model.ApprovalDate = request.ApprovalDate;
            
            // 기안일이 정상적인 Date 데이터인지 여부 
            bool isApprovalDateValid = DateOnly.TryParse(request.ApprovalDate, out DateOnly approvalDate);

            // 정상적인 데이터인경우 
            if (isApprovalDateValid)
            {
                model.IsApprovalDateValid = true;
                model.ApproveDateValue = approvalDate;
                model.Year = approvalDate.Year.ToString();
                model.Month = approvalDate.Month.ToString("00");
                model.Day = approvalDate.Day.ToString("00");
                
                // 승인일이 정상인경우 승인으로 인정
                model.IsApproved = true;
                model.ApprovalDate = approvalDate.ToString("yyyy-MM-dd");
            }
  
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
            model.Description = request.Description;
            model.SectorId = request.SectorId;
            model.BusinessUnitId = request.BusinessUnitId;
            model.CostCenterId = request.CostCenterId;
            model.CountryBusinessManagerId = request.CountryBusinessManagerId;
            model.SectorName = sectorName;
            model.CostCenterName = costCenterName;
            model.CountryBusinessManagerName = countryBusinessManagerName;
            model.BusinessUnitName = businessUnitName;
            model.PoNumber = request.PoNumber;
            model.ApprovalStatus = request.ApprovalStatus;
            model.ApprovalAmount = request.ApprovalAmount;
            model.Actual = request.Actual;
            model.OcProjectName = request.OcProjectName;
            model.BossLineDescription = request.BossLineDescription;
            model.RegName = user.DisplayName; 
            model.ModName = user.DisplayName; 
            model.RegDate = DateTime.Now; 
            model.ModDate = DateTime.Now; 
            model.RegId = user.Id; 
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