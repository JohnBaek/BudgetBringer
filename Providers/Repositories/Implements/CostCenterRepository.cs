using System.Diagnostics.CodeAnalysis;
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
/// 코스트 센터 리파지토리
/// </summary>
[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class CostCenterRepository : ICostCenterRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<CostCenterRepository> _logger;
    
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
    private const string LogCategory = "[CostCenter]";

    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="queryService">쿼리 서비스</param>
    /// <param name="userRepository">유저 리파지토리</param>
    /// <param name="logActionWriteService">액션 로그 기록 서비스</param>
    public CostCenterRepository(
        ILogger<CostCenterRepository> logger
        , AnalysisDbContext dbContext
        , IQueryService queryService
        , IUserRepository userRepository
        , ILogActionWriteService logActionWriteService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _queryService = queryService;
        _userRepository = userRepository;
        _logActionWriteService = logActionWriteService;
    }
    
    
   /// <summary>
    /// 셀렉터 매핑 정의
    /// </summary>
    private Expression<Func<DbModelCostCenter, ResponseCostCenter>> MapDataToResponse { get; init; } = item => new ResponseCostCenter
    {
        Id = item.Id,
        Value = item.Value,
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
    public async Task<ResponseList<ResponseCostCenter>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseCostCenter> result = new ResponseList<ResponseCostCenter>();
        try
        {
            // 결과를 반환한다.
            return await _queryService.ToResponseListAsync(requestQuery, MapDataToResponse);
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
    public async Task<ResponseData<ResponseCostCenter>> GetAsync(string id)
    {
        ResponseData<ResponseCostCenter> result = new ResponseData<ResponseCostCenter>();
        try
        {
            // 요청이 유효하지 않은경우
            if (id.IsEmpty())
                return new ResponseData<ResponseCostCenter>(EnumResponseResult.Error,"ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요",null);

            // 기존데이터를 조회한다.
            ResponseCostCenter? before =
                await _dbContext.CostCenters.Where(i => i.Id == id.ToGuid())
                    .Select(MapDataToResponse)
                    .FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(before == null)
                return new ResponseData<ResponseCostCenter>(EnumResponseResult.Error,"ERROR_IS_NONE_EXIST", "대상이 존재하지 않습니다.",null);

            // 데이터를 복사한다.
            return new ResponseData<ResponseCostCenter>(EnumResponseResult.Success, "", "", before);
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
    public async Task<Response> UpdateAsync(string id , RequestCostCenter request)
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
            DbModelCostCenter? update = await _dbContext.CostCenters
                .Where(i => i.Id == id.ToGuid())
                .FirstOrDefaultAsync();
            
            // 대상 데이터가 없는경우
            if(update == null)
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "대상이 존재하지 않습니다."};
            
            // 로그기록을 위한 데이터 스냅샷
            DbModelCostCenter snapshot = update.ToClone()!;
          
            // 데이터를 수정한다.
            update.Value = request.Value;
            update.RegName = user.DisplayName; 
            update.ModName = user.DisplayName; 
            update.RegDate = DateTime.Now; 
            update.ModDate = DateTime.Now; 
            update.RegId = user.Id; 
            update.ModId = user.Id; 
            
            // 데이터베이스에 업데이트처리 
            _dbContext.CostCenters.Update(update);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success,"","");
            
            // 로그 기록
            await _logActionWriteService.WriteUpdate(snapshot.FromCopyValue<ResponseCostCenter>(), update.FromCopyValue<ResponseCostCenter>(), user , "",LogCategory);
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
    public async Task<ResponseData<ResponseCostCenter>> AddAsync(RequestCostCenter request)
    {
        ResponseData<ResponseCostCenter> result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<ResponseCostCenter>{ Code = "ERROR_INVALID_PARAMETER", Message = request.GetFirstErrorMessage()};

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseCostCenter>{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 동일한 이름을 가진 데이터가 있는지 확인
            bool isDuplicated = await _dbContext.CostCenters.AnyAsync(i => i.Value.ToLower() == request.Value.ToString());
            
            // 동일한 데이터가 있다면 
            if(isDuplicated)
                return new ResponseData<ResponseCostCenter>{ Code = "ERROR_IS_DUPLICATED", Message = "이미 존재하는 데이터입니다."};
          
            // 데이터를 생성한다.
            DbModelCostCenter add = new DbModelCostCenter
            {
                Id = Guid.NewGuid() ,
                Value = request.Value.ToString() ,
                RegName = user.DisplayName ,
                ModName = user.DisplayName ,
                RegDate = DateTime.Now ,
                ModDate = DateTime.Now ,
                RegId = user.Id ,
                ModId = user.Id ,
            };
            
            // 데이터베이스에 데이터 추가 
            await _dbContext.CostCenters.AddAsync(add);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            
            // 추가된 데이터를 조회
            ResponseData<ResponseCostCenter> added = await GetAsync(add.Id.ToString());
            
            result = new ResponseData<ResponseCostCenter>{ Result = EnumResponseResult.Success , Data = added.Data };
            
            // 로그 기록
            await _logActionWriteService.WriteAddition(add.FromCopyValue<ResponseCostCenter>(), user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new ResponseData<ResponseCostCenter>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
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
            if (id.IsEmpty())
                return new Response {Code = "ERROR_INVALID_PARAMETER", Message = "필수 값을 입력해주세요"};

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if (user == null)
                return new Response {Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};

            // 기존데이터를 조회한다.
            DbModelCostCenter? remove =
                await _dbContext.CostCenters.Where(i => i.Id == id.ToGuid()).FirstOrDefaultAsync();

            // 조회된 데이터가 없다면
            if (remove == null)
                return new Response {Code = "ERROR_IS_NONE_EXIST", Message = "대상이 존재하지 않습니다."};

            // 대상을 삭제한다.
            _dbContext.Remove(remove);
            await _dbContext.SaveChangesAsync();

            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success, "", "");

            // 로그 기록
            await _logActionWriteService.WriteDeletion(remove.FromCopyValue<ResponseCostCenter>(), user, "", LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new Response(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return result;
    }
}