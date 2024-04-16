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
/// 컨트리 비지니스 매니저 리파지토리
/// </summary>
[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class CountryBusinessManagerRepository : ICountryBusinessManagerRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<CountryBusinessManagerRepository> _logger;
    
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
    private const string LogCategory = "[CountryBusinessManager]";

    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">디비컨텍스트</param>
    /// <param name="queryService">쿼리 서비스</param>
    /// <param name="userRepository">유저 리파지토리</param>
    /// <param name="logActionWriteService">액션 로그 기록 서비스</param>
    public CountryBusinessManagerRepository(
        ILogger<CountryBusinessManagerRepository> logger
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
    private Expression<Func<DbModelCountryBusinessManager, ResponseCountryBusinessManager>> MapDataToResponse { get; init; } = item => new ResponseCountryBusinessManager
    {
        Id = item.Id,
        Name = item.Name,
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
    public async Task<ResponseList<ResponseCountryBusinessManager>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseCountryBusinessManager> result = new ResponseList<ResponseCountryBusinessManager>();
        try
        {
            // 기본 Sort가 없을 경우 
            if (requestQuery.SortOrders is { Count: 0 })
            {
                requestQuery.SortOrders.Add("Asc");
                requestQuery.SortFields?.Add(nameof(ResponseCountryBusinessManager.Name));
            }
            
            // 검색 메타정보 추가
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCountryBusinessManager.Name));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(ResponseCommonWriter.RegDate));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.RegDate));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Equals , nameof(ResponseCommonWriter.ModDate));
            
            // 결과를 반환한다.
            result = await _queryService.ToResponseListAsync(requestQuery, MapDataToResponse);
            
            // 조회에 성공했을 경우 
            if (result is { Result: EnumResponseResult.Success, Items.Count: > 0 })
                // 각 비지니스 컨트리 매니저에 비지니스 유닛을 바인딩한다.
                await SetAddBusinessUnitsAsync(result.Items);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 각 비지니스 컨트리 매니저에 비지니스 유닛을 바인딩한다.
    /// </summary>
    /// <param name="managers"></param>
    private async Task SetAddBusinessUnitsAsync(List<ResponseCountryBusinessManager> managers)
    {
        try
        {
            // 관계 키를 전체 조회한다.
            List<DbModelCountryBusinessManagerBusinessUnit> relations = 
                await _dbContext.CountryBusinessManagerBusinessUnits
                    .Include(i => i.BusinessUnit)
                    .AsNoTracking()
                    .ToListAsync();
            
            // 조회된 모든 매니저에대해 처리한다.
            foreach (ResponseCountryBusinessManager manager in managers)
            {
                // 관계 정보를 찾는다.
                List<DbModelCountryBusinessManagerBusinessUnit> relation =
                    relations.Where(i => i.CountryBusinessManagerId == manager.Id)
                        .ToList();
                
                // 비지니스 유닛을 추가한다.
                foreach (DbModelCountryBusinessManagerBusinessUnit item in relation)
                {
                    manager.BusinessUnits.Add(new ResponseBusinessUnit(item.BusinessUnit.Id, item.BusinessUnit.Name));
                }
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
    }

    /// <summary>
    /// 데이터를 가져온다.
    /// </summary>
    /// <param name="id">아이디</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCountryBusinessManager>> GetAsync(string id)
    {
        ResponseData<ResponseCountryBusinessManager> result = new ResponseData<ResponseCountryBusinessManager>();
        try
        {
            // 요청이 유효하지 않은경우
            if (id.IsEmpty())
                return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"ERROR_INVALID_PARAMETER", "필수 값을 입력해주세요",null);

            // 기존데이터를 조회한다.
            ResponseCountryBusinessManager? before =
                await _dbContext.CountryBusinessManagers.Where(i => i.Id == id.ToGuid())
                    .Select(MapDataToResponse)
                    .FirstOrDefaultAsync();
            
            // 조회된 데이터가 없다면
            if(before == null)
                return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"ERROR_IS_NONE_EXIST", "대상이 존재하지 않습니다.",null);

            // 관계 키를 전체 조회한다.
            List<DbModelCountryBusinessManagerBusinessUnit> relations = 
                await _dbContext.CountryBusinessManagerBusinessUnits
                    .Include(i => i.BusinessUnit)
                    .AsNoTracking()
                    .Where(i => i.CountryBusinessManagerId == before.Id)
                    .ToListAsync();
            
            // 비지니스 유닛을 추가한다.
            foreach (DbModelCountryBusinessManagerBusinessUnit item in relations)
            {
                before.BusinessUnits.Add(new ResponseBusinessUnit(item.BusinessUnit.Id, item.BusinessUnit.Name));
            }
            
            // 데이터를 복사한다.
            return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Success, "", "", before);
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
    public async Task<Response> UpdateAsync(string id , RequestCountryBusinessManager request)
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
            DbModelCountryBusinessManager? update = await _dbContext.CountryBusinessManagers
                .Where(i => i.Id == id.ToGuid())
                .FirstOrDefaultAsync();
            
            // 대상 데이터가 없는경우
            if(update == null)
                return new Response{ Code = "ERROR_TARGET_DOES_NOT_FOUND", Message = "대상이 존재하지 않습니다."};
            
            // 로그기록을 위한 데이터 스냅샷
            DbModelCountryBusinessManager snapshot = update.ToClone()!;
          
            // 데이터를 수정한다.
            update.Name = request.Name;
            update.RegName = user.DisplayName; 
            update.ModName = user.DisplayName; 
            update.RegDate = DateTime.Now; 
            update.ModDate = DateTime.Now; 
            update.RegId = user.Id; 
            update.ModId = user.Id; 
            
            // 데이터베이스에 업데이트처리 
            _dbContext.CountryBusinessManagers.Update(update);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success,"","");
            
            // 로그 기록
            await _logActionWriteService.WriteUpdate(snapshot.FromCopyValue<ResponseCountryBusinessManager>(), update.FromCopyValue<ResponseCountryBusinessManager>(), user , "",LogCategory);
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
    public async Task<ResponseData<ResponseCountryBusinessManager>> AddAsync(RequestCountryBusinessManager request)
    {
        ResponseData<ResponseCountryBusinessManager> result;
        
        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new ResponseData<ResponseCountryBusinessManager>{ Code = "ERROR_INVALID_PARAMETER", Message = request.GetFirstErrorMessage()};

            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseCountryBusinessManager>{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 동일한 이름을 가진 데이터가 있는지 확인
            bool isDuplicated = await _dbContext.CountryBusinessManagers.AnyAsync(i => i.Name.ToLower() == request.Name.ToLower());
            
            // 동일한 데이터가 있다면 
            if(isDuplicated)
                return new ResponseData<ResponseCountryBusinessManager>{ Code = "ERROR_IS_DUPLICATED", Message = "이미 존재하는 데이터입니다."};
          
            // 데이터를 생성한다.
            DbModelCountryBusinessManager add = new DbModelCountryBusinessManager
            {
                Id = Guid.NewGuid() ,
                Name = request.Name ,
                RegName = user.DisplayName ,
                ModName = user.DisplayName ,
                RegDate = DateTime.Now ,
                ModDate = DateTime.Now ,
                RegId = user.Id ,
                ModId = user.Id ,
            };
            
            // 데이터베이스에 데이터 추가 
            await _dbContext.CountryBusinessManagers.AddAsync(add);
            await _dbContext.SaveChangesAsync();
            
            // 커밋한다.
            await transaction.CommitAsync();
            
            // 추가된 데이터를 조회
            ResponseData<ResponseCountryBusinessManager> added = await GetAsync(add.Id.ToString());
            
            result = new ResponseData<ResponseCountryBusinessManager>{ Result = EnumResponseResult.Success , Data = added.Data };
            
            // 로그 기록
            await _logActionWriteService.WriteAddition(add.FromCopyValue<ResponseCountryBusinessManager>(), user , "",LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
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
            DbModelCountryBusinessManager? remove =
                await _dbContext.CountryBusinessManagers.Where(i => i.Id == id.ToGuid()).FirstOrDefaultAsync();

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
            await _logActionWriteService.WriteDeletion(remove.FromCopyValue<ResponseCountryBusinessManager>(), user, "", LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new Response(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 매니저에 비지니스 유닛을 추가한다.
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseCountryBusinessManager>> AddUnitAsync(string managerId, string unitId)
    {
        ResponseData<ResponseCountryBusinessManager> result;

        // 요청이 유효하지 않은경우
        if (managerId.IsEmpty() || unitId.IsEmpty())
            return new ResponseData<ResponseCountryBusinessManager> {Code = "ERROR_INVALID_PARAMETER", Message = "필수 값을 입력해주세요"};

        // 매니저 조회
        DbModelCountryBusinessManager? manager = await _dbContext.CountryBusinessManagers
            .Where(i => i.Id == managerId.ToGuid())
            .FirstOrDefaultAsync();
        
        // 찾지 못한경우
        if (manager == null)
            return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error, "ERROR_IS_NONE_EXIST",
                "대상이 존재하지 않습니다.", null);
        
        // 유닛 조회
        DbModelBusinessUnit? unit = await _dbContext.BusinessUnits
            .Where(i => i.Id == unitId.ToGuid())
            .FirstOrDefaultAsync();
        
        // 찾지 못한경우
        if (unit == null)
            return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error, "ERROR_IS_NONE_EXIST",
                "대상이 존재하지 않습니다.", null);
        
        // 이미 추가된 관계인지 확인
        bool exist = await _dbContext.CountryBusinessManagerBusinessUnits
            .AnyAsync(i =>
                i.CountryBusinessManagerId == managerId.ToGuid() &&
                i.BusinessUnitId == unitId.ToGuid()
            );
        
        // 추가된 관계인경우
        if (exist)
            return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error, "ERROR_IS_NONE_EXIST",
                "이미 추가되어있습니다.", null);
        
        // 트랜잭션을 시작한다.
        using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
      
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if (user == null)
                return new ResponseData<ResponseCountryBusinessManager> {Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};

            // Insert 될 데이터를 생성한다.
            DbModelCountryBusinessManagerBusinessUnit relation = new DbModelCountryBusinessManagerBusinessUnit
            {
                CountryBusinessManagerId = managerId.ToGuid(),
                BusinessUnitId = unitId.ToGuid(),
            };
            
            // 대상을 추가한다.
            await _dbContext.AddAsync(relation);
            await _dbContext.SaveChangesAsync();

            // 커밋한다.
            await transaction.CommitAsync();

            // 데이터조회
            result = await this.GetAsync(managerId);

            // 로그 기록
            await _logActionWriteService.WriteDeletion(relation.FromCopyValue<ResponseCountryBusinessManager>(), user, "매니저 정보에서 비지니스 유닛 추가", LogCategory);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            result = new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error, "ERROR_DATA_EXCEPTION", "처리중 예외가 발생했습니다.", null);
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 매니저에 비지니스 유닛을 제거한다..
    /// </summary>
    /// <param name="managerId"></param>
    /// <param name="unitId"></param>
    /// <returns></returns>
    public async Task<Response> DeleteUnitAsync(string managerId, string unitId)
    {
        Response result;
        
        // 요청이 유효하지 않은경우
        if (managerId.IsEmpty() || unitId.IsEmpty())
            return new ResponseData<ResponseCountryBusinessManager> {Code = "ERROR_INVALID_PARAMETER", Message = "필수 값을 입력해주세요"};
        
        // 이미 추가된 관계인지 확인
        DbModelCountryBusinessManagerBusinessUnit? remove = await _dbContext.CountryBusinessManagerBusinessUnits
            .Where(i =>
                i.CountryBusinessManagerId == managerId.ToGuid() &&
                i.BusinessUnitId == unitId.ToGuid()
            ).FirstOrDefaultAsync();
        
        // 찾지 못한경우
        if(remove == null)
            return new ResponseData<ResponseCountryBusinessManager>(EnumResponseResult.Error, "ERROR_IS_NONE_EXIST",
                "대상이 존재하지 않습니다", null);

        // 트랜잭션을 시작한다.
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if (user == null)
                return new Response {Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};

            // 대상을 삭제한다.
            _dbContext.Remove(remove);
            await _dbContext.SaveChangesAsync();

            // 커밋한다.
            await transaction.CommitAsync();
            result = new Response(EnumResponseResult.Success, "", "");

            // 로그 기록
            await _logActionWriteService.WriteDeletion(remove.FromCopyValue<ResponseCountryBusinessManager>(), user, $"매니저 정보에서 비지니스 유닛 삭제", LogCategory);
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