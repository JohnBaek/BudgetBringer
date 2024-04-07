using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Newtonsoft.Json;
using Providers.Repositories.Interfaces;

namespace Providers.Repositories.Implements;

/// <summary>
/// Budget 관련 통계 정보 캐싱 테이블 리파지토리 
/// </summary>
public class BudgetAnalysisCacheRepository : IBudgetAnalysisCacheRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<BudgetAnalysisCacheRepository> _logger;
    
    /// <summary>
    /// 사용자 리파지토리
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="userRepository"></param>
    public BudgetAnalysisCacheRepository(
          AnalysisDbContext dbContext
        , ILogger<BudgetAnalysisCacheRepository> logger, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userRepository = userRepository;
    }

    /// <summary>
    /// 캐시가 있는지 확인한다.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<ResponseData<bool>> HasCachedAsync(EnumBudgetAnalysisCacheType type)
    {
        ResponseData<bool> result;
        
        try
        {
            bool hasCached = await _dbContext.BudgetAnalysisCache.Where(i => i.CacheType == type).AnyAsync();
            return new ResponseData<bool>(EnumResponseResult.Success, "", "", hasCached);
        }
        catch (Exception e)
        {
            result = new ResponseData<bool>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.", false);
            e.LogError(_logger); 
        }
    
        return result;
    }

    /// <summary>
    /// 캐시를 추가한다.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public async Task<Response> AddCacheAsync(EnumBudgetAnalysisCacheType type, object target)
    {
        Response result;
        
        try
        {
            // 직렬화 한다.
            string serialized = JsonConvert.SerializeObject(target);
            
            // 트랜잭션을 시작한다.
            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // 로그인한 사용자 정보를 가져온다.
                DbModelUser? user = await _userRepository.GetAuthenticatedUser();

                // 사용자 정보가 없는경우 
                if(user == null)
                    return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
                
                // 기존 캐시 데이터를 가져온다.
                DbModelBudgetAnalysisCache? cache = await _dbContext.BudgetAnalysisCache.Where(i => i.CacheType == type).FirstOrDefaultAsync();
                
                // 데이터가 존재하는경우
                if (cache != null)
                {
                    cache.JsonRaw = serialized ;
                    cache.ModName = user.DisplayName;
                    cache.ModDate = DateTime.Now;
                    
                    // 업데이트 후 저장
                    _dbContext.Update(cache);
                    await _dbContext.SaveChangesAsync();
                }
                // 데이터가 존재하지 않는경우 
                else
                {
                    // 신규로 등록할 캐시
                    DbModelBudgetAnalysisCache newCache = new DbModelBudgetAnalysisCache
                    {
                        CacheType = type ,
                        JsonRaw = serialized ,
                        RegName = user.DisplayName,
                        ModName = user.DisplayName,
                        RegDate = DateTime.Now,
                        ModDate = DateTime.Now,
                    };
                    
                    // 추가 후 저장
                    await _dbContext.AddAsync(newCache);
                    await _dbContext.SaveChangesAsync();
                }
                
                // 커밋한다.
                await transaction.CommitAsync();
                result = new Response(EnumResponseResult.Success,"","");
            
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
                e.LogError(_logger);
            }
            
        }
        catch (Exception e)
        {
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger); 
        }
    
        return result;
    }

    /// <summary>
    /// 시리얼라이즈된 통계 데이터를 리턴한다.
    /// </summary>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<ResponseData<T>> GetDeserializedData<T>(EnumBudgetAnalysisCacheType type) where T : class
    {
        ResponseData<T> result;
        
        try
        {
            // 기존 캐시 데이터를 가져온다.
            DbModelBudgetAnalysisCache? cache = await _dbContext.BudgetAnalysisCache.Where(i => i.CacheType == type).FirstOrDefaultAsync();

            // 캐시가 비어있을경우
            if (cache?.JsonRaw == null || cache.JsonRaw.IsEmpty())
                return new ResponseData<T>(EnumResponseResult.Error, "", "", null);
            
            // Deserialize 한다.
            T? deserialized = JsonConvert.DeserializeObject<T>(cache.JsonRaw);
            
            // 실패했을 경우
            if(deserialized == null)
                return new ResponseData<T>(EnumResponseResult.Error, "", "", null);
            
            return new ResponseData<T>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",deserialized);
        }
        catch (Exception e)
        {
            result = new ResponseData<T>(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger); 
        }
    
        return result;
    }
}