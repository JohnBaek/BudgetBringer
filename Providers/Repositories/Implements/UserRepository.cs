using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using Features.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Requests.Query;
using Models.Requests.Users;
using Models.Responses;
using Models.Responses.Users;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Repositories.Implements;


/// <summary>
/// 유저 정보 리파지토리 인터페이스
/// </summary>
[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
[SuppressMessage("ReSharper", "SpecifyStringComparison")]
public class UserRepository : IUserRepository
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// 사용자 매니저
    /// </summary>
    private readonly UserManager<DbModelUser> _userManager;
    
    /// <summary>
    /// IHttpContextAccessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;
    
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
    private const string LogCategory = "[User]";

    /// <summary>
    /// SystemConfig Service
    /// </summary>
    private ISystemConfigService _systemConfigService;

    /// <summary>
    /// HashService
    /// </summary>
    private readonly IHashService _hashService;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext">DB Context</param>
    /// <param name="logger">Logger</param>
    /// <param name="userManager"></param>
    /// <param name="httpContextAccessor">IHttpContextAccessor</param>
    /// <param name="queryService">queryService</param>
    /// <param name="logActionWriteService">logActionWriteService</param>
    /// <param name="systemConfigService">systemConfigService</param>
    /// <param name="hashService"></param>
    public UserRepository(
          AnalysisDbContext dbContext
        , ILogger<UserRepository> logger
        , UserManager<DbModelUser> userManager
        , IHttpContextAccessor httpContextAccessor, IQueryService queryService, ILogActionWriteService logActionWriteService, ISystemConfigService systemConfigService, IHashService hashService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _queryService = queryService;
        _logActionWriteService = logActionWriteService;
        _systemConfigService = systemConfigService;
        _hashService = hashService;
    }
    
    
    /// <summary>
    /// 셀렉터 매핑 정의
    /// </summary>
    private Expression<Func<DbModelUser, ResponseUser>> MapDataToResponse { get; init; } = item => new ResponseUser()
    {
        Id = item.Id,
        DisplayName = item.DisplayName,
        LoginId = item.LoginId ,
    };
    
    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자가 존재하는지 확인한다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <returns>결과</returns>
    public async Task<bool> ExistUserAsync(string loginId)
    {
        try
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(i => i.LoginId == loginId);
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
    
        return false;
    }
    
    /// <summary>
    /// 사용자의 아이디와 패스워드로 사용자를 가져온다.
    /// </summary>
    /// <param name="loginId">로그인 아이디</param>
    /// <param name="password">패스워드 (SHA 256 인크립트 된 원본)</param>
    /// <returns>결과</returns>
    public async Task<DbModelUser?> GetUserWithIdPasswordAsync(string loginId, string password)
    {
        DbModelUser? result;
    
        try
        {
            // 사용자의 정보를 찾는다.
            DbModelUser? findUser = await _dbContext.Users.AsNoTracking()
                .Where(i => i.LoginId == loginId).FirstOrDefaultAsync();
                
            // 찾을수 없는경우 
            if (findUser == null)
                return null;
            
            return findUser;
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }
    
        return result;
    }


    /// <summary>
    /// 사용자의 Id 값으로 권한 값의 리스트를 가져온다.
    /// </summary>
    /// <param name="userId">사용자 아이디</param>
    /// <returns></returns>
    public async Task<List<string>> GetRolesByUserAsync(Guid userId)
    {
        List<string> result = new List<string>();
    
        try
        {
            result = (await _dbContext.Roles.AsNoTracking()
                .Join(_dbContext.UserRoles.Where(i => i.UserId == userId)
                    , role => role.Id
                    , userRole => userRole.RoleId
                    , (role, userRole)
                        => role.Name
                    )
                .ToListAsync())!;
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }
    
        return result;
    }

    /// <summary>
    /// 로그인한 사용자의 정보를 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<DbModelUser?> GetAuthenticatedUser()
    {
        DbModelUser? result;
    
        try
        {
            // 세션이 비어있을 경우 
            if (_httpContextAccessor.HttpContext?.User == null)
                return new DbModelUser();
            
            result = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
        catch (Exception e)
        {
            result = null;
            e.LogError(_logger);
        }
    
        return result;
    }

    /// <summary>
    /// 리스트를 가져온다.
    /// </summary>
    /// <param name="requestQuery">쿼리 정보</param>
    /// <returns></returns>
    public async Task<ResponseList<ResponseUser>> GetListAsync(RequestQuery requestQuery)
    {
        ResponseList<ResponseUser> result = new ResponseList<ResponseUser>();
        try
        {
            // 로그인한 사용자를 가져온다.
            DbModelUser? loginUser = await GetAuthenticatedUser();
            
            // 사용자 정보가 없는경우 
            if(loginUser == null)
                return new ResponseList<ResponseUser> { Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 관리자가 아닌경우 
            if(loginUser.LoginId.ToLower() != "admin")
                return new ResponseList<ResponseUser> { Code = "ERROR_SESSION_TIMEOUT", Message = "접근권한이 없습니다."};
            
            // 기본 Sort가 없을 경우 
            if (requestQuery.SortOrders is { Count: 0 })
            {
                requestQuery.SortOrders.Add("Asc");
                requestQuery.SortFields?.Add(nameof(ResponseUser.DisplayName));
            }
            
            // 검색 메타정보 추가
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(DbModelUser.DisplayName));
            requestQuery.AddSearchAndSortDefine(EnumQuerySearchType.Contains , nameof(DbModelUser.LoginId));
            
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
    /// 패스워드를 변경한다 ( Role Admin 인 사용자만 가능 )
    /// </summary>
    /// <param name="request">패스워드 변경정보</param>
    /// <returns></returns>
    public async Task<Response> UpdatePasswordAsync(RequestUserChangePassword request)
    {
        Response result;
        
        try
        {
            // 요청이 유효하지 않은경우
            if(request.IsInValid())
                return new Response{ Code = "ERROR_INVALID_PARAMETER", Message = request.GetFirstErrorMessage() };

            // Get password Pattern
            DbModelSystemConfigDetail? passwordConfig = await _systemConfigService.GetValueAsync("USER", "PASSWORD_PATTERN");

            // If password conditions are present , and check password conditions
            if (passwordConfig != null && !passwordConfig.Value.IsMatch(request.Password))
            {
                return new Response{ Code = "ERROR_INVALID_PARAMETER", Message = passwordConfig.Description! };
            }

            // 로그인한 사용자를 가져온다.
            DbModelUser? loginUser = await GetAuthenticatedUser();
            
            // 사용자 정보가 없는경우 
            if(loginUser == null)
                return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "로그인 상태를 확인해주세요"};
            
            // 관리자가 아닌경우 
            if(loginUser.LoginId.ToLower() != "admin")
                return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "접근권한이 없습니다."};

            // 대상사용자를 검색한다.
            DbModelUser? userPrincipal = await _userManager.FindByIdAsync(request.Id.ToString());
            
            // 사용자 정보가 없는경우 
            if(userPrincipal == null)
                return new Response{ Code = "ERROR_SESSION_TIMEOUT", Message = "사용자가 존재하지 않습니다."};

            StringBuilder message = new StringBuilder();

            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                userPrincipal.PasswordHash = _hashService.ComputeHash(request.Password);
                _dbContext.Update(userPrincipal);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                message.AppendLine($"사용자 [{userPrincipal.DisplayName}] 의 대한 패스워드를 변경하였습니다.");
                result = new Response(EnumResponseResult.Success, "", message.ToString());
            }
            catch (Exception e)
            {

                message.AppendLine($"사용자 [{userPrincipal.DisplayName}] 의 대한 패스워드 변경을 실패하였습니다.");
                result = new Response(EnumResponseResult.Error, "", message.ToString());

                await transaction.RollbackAsync();
                e.LogError(_logger);
            }


            
            // // Token 토큰을 생성한다.
            // string token = await _userManager.GeneratePasswordResetTokenAsync(userPrincipal);
            //
            // // 사용자의 패스워드를 초기화한다.
            // IdentityResult resetPasswordResult = await _userManager.ResetPasswordAsync(userPrincipal, token, request.Password);
            // StringBuilder message = new StringBuilder();
            //
            // 성공한 경우 
            // if (resetPasswordResult.Succeeded)
            // {
            //     message.AppendLine($"사용자 [{userPrincipal.DisplayName}] 의 대한 패스워드를 변경하였습니다.");
            //     result = new Response(EnumResponseResult.Success, "", "");
            //
            // }
            // // 실패한 경우
            // else
            // {
            //     message.AppendLine($"사용자 [{userPrincipal.DisplayName}] 의 대한 패스워드 변경을 실패하였습니다.");
            //     result = new Response(EnumResponseResult.Error, "", message.ToString());
            // }

            // 로그 기록
            await _logActionWriteService.WriteUpdate(new ResponseUser(), new ResponseUser(), userPrincipal , message.ToString() ,LogCategory);
        }
        catch (Exception e)
        {
            result = new Response(EnumResponseResult.Error,"ERROR_DATA_EXCEPTION","처리중 예외가 발생했습니다.");
            e.LogError(_logger);
        }
    
        return result;
    }
}