using System.Linq.Expressions;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
    
    /// <summary>222222
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
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseBudgetApproved>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseBudgetApproved> result = new ResponseList<ResponseBudgetApproved>();
        try
        {
            // 검색 메타정보 추가
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.Description));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.PoNumber));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.ApprovalStatus));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.ApprovalAmount));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseBudgetApproved.Actual));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.CostCenterName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.CountryBusinessManagerName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.BusinessUnitName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.OcProjectName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseBudgetApproved.BossLineDescription));
            
            // 셀렉팅 정의
            Expression<Func<DbModelBudgetApproved, ResponseBudgetApproved>> mapDataToResponse = item => new ResponseBudgetApproved
            {
                Id = item.Id,
                ApprovalDate = item.ApprovalDate,
                IsApproved = item.IsApproved,
                IsAbove500K = item.IsAbove500K,
                Description = item.Description,
                SectorId = item.SectorId,
                BusinessUnitId = item.BusinessUnitId,
                CostCenterId = item.CostCenterId,
                CountryBusinessManagerId = item.CountryBusinessManagerId,
                CostCenterName = item.CostCenterName,
                CountryBusinessManagerName = item.CountryBusinessManagerName,
                BusinessUnitName = item.BusinessUnitName,
                PoNumber = item.PoNumber,
                ApprovalStatus = item.ApprovalStatus,
                ApprovalAmount = item.ApprovalAmount,
                Actual = item.Actual,
                OcProjectName = item.OcProjectName,
                BossLineDescription = item.BossLineDescription,
            };
            
            // 결과를 반환한다.
            return await _queryService.ToResponseListAsync(requestQuery, mapDataToResponse);
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
    public async Task<ResponseData<ResponseBudgetApproved>> GetAsync(string id)
    {
        ResponseData<ResponseBudgetApproved> result = new ResponseData<ResponseBudgetApproved>();
        try
        {
            // 요청이 유효하지 않은경우
            if (id.IsEmpty())
                return new ResponseData<ResponseBudgetApproved>("ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요");

            // 기존데이터를 조회한다.
            DbModelBudgetApproved? before =
                await _dbContext.BudgetApproved.Where(i => i.Id == id.ToGuid()).FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(before == null)
                return new ResponseData<ResponseBudgetApproved>("ERROR_IS_NONE_EXIST", "대상이 존재하지 않습니다.");

            // 데이터를 복사한다.
            ResponseBudgetApproved data = before.FromCopyValue<ResponseBudgetApproved>()!;
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
                return new Response{ Code = "ERROR_INVALID_PARAMETER", Message = "필수 값을 입력해주세요"};
            
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 동일한 이름을 가진 데이터가 있는지 확인
            DbModelBudgetApproved? update = await _dbContext.BudgetApproved
                .Where(i => i.Id == id.ToGuid())
                .FirstOrDefaultAsync();
            
            // 대상 데이터가 없는경우
            if(update == null)
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "대상이 존재하지 않습니다."};
            
            // 로그기록을 위한 데이터 스냅샷
            DbModelBudgetApproved snapshot = update.FromClone()!;

            // 기안일이 정상적인 Date 데이터인지 여부 
            bool isApprovalDateValid = DateOnly.TryParse(request.ApprovalDate, out DateOnly approvalDate);
            
            // 정상적인 데이터인경우 
            if (isApprovalDateValid)
            {
                update.IsApprovalDateValid = true;
                update.ApproveDateValue = approvalDate;
                update.Year = approvalDate.Year.ToString();
                update.Month = approvalDate.Month.ToString("00");
                update.Day = approvalDate.Day.ToString("00");
            }
            
            // 데이터를 수정한다.
            update.IsAbove500K = request.IsAbove500K;
            update.ApprovalDate = request.ApprovalDate;
            update.Description = request.Description;
            update.PoNumber = request.PoNumber;
            update.ApprovalStatus = request.ApprovalStatus;
            update.ApprovalAmount = request.ApprovalAmount;
            update.Actual = request.Actual;
            update.OcProjectName = request.OcProjectName;
            update.BossLineDescription = request.BossLineDescription;
            update.ModName = user.DisplayName; 
            update.ModDate = DateTime.Now; 
            update.ModId = user.Id; 
            
            // 데이터베이스에 업데이트처리 
            _dbContext.BudgetApproved.Update(update);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success);
            
            // 로그 기록
            await _logActionWriteService.WriteUpdate(snapshot, update, user , "",LogCategory);
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
                SectorId = request.SectorId,
                
                CostCenterId = request.CostCenterId,
                BusinessUnitId = request.BusinessUnitId,
                CountryBusinessManagerId = request.CountryBusinessManagerId,
                //
                // CostCenterName = costCenterName ,
                // BusinessUnitName = businessUnitName,
                // CountryBusinessManagerName = countryBusinessManagerName,
                
                IsAbove500K = request.IsAbove500K,
                ApprovalDate = request.ApprovalDate,
                Description = request.Description,
                PoNumber = request.PoNumber,
                ApprovalStatus = request.ApprovalStatus,
                ApprovalAmount = request.ApprovalAmount,
                Actual = request.Actual,
                OcProjectName = request.OcProjectName,
                BossLineDescription = request.BossLineDescription,
                IsApproved = false,
                RegName = user.DisplayName,
                ModName = user.DisplayName,
                RegDate = DateTime.Now,
                ModDate = DateTime.Now,
                RegId = user.Id,
                ModId = user.Id,
            };
            
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
            result = new ResponseData<ResponseBudgetApproved>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
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
            DbModelBusinessUnit? remove =
                await _dbContext.BusinessUnits.Where(i => i.Id == id.ToGuid()).FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(remove == null)
                return new Response{ Code = "ERROR_IS_NONE_EXIST", Message = "대상이 존재하지 않습니다."};

            // 대상을 삭제한다.
            _dbContext.Remove(remove);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success);
            
            // 로그 기록
            await _logActionWriteService.WriteDeletion(remove, user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }
    
        return result;
    }
}