using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Models.Responses.Process.Owner;
using Models.Responses.Users;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Repositories.Implements;

/// <summary>
/// Budget 관련 Repository
/// </summary>
public class BudgetProcessRepository : IBudgetProcessRepository
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
    /// 유저 서비스
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// 페이지를 접근하는데 필요한 권한
    /// </summary>
    private readonly List<string> _requiredClaims = ["process-result", "process-result-view"];
    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="userRepository"></param>
    /// <param name="userService"></param>
    public BudgetProcessRepository(
          AnalysisDbContext dbContext
        , ILogger<BudgetApprovedRepository> logger
        , IUserRepository userRepository
        , IUserService userService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userRepository = userRepository;
        _userService = userService;
    }

    /// <summary>
    /// 오너별 예산 편성 진행상황을 가져온다.
    /// 단, process-result-view 의 Claim 만 소유한 경우 로그인한 사용자의 CountryManagerId 가 일치하는
    /// 정보만 나와야한다. 
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseOwnerSummary>> GetOwnerBudgetAsync()
    {
        ResponseData<ResponseOwnerSummary> result = new ResponseData<ResponseOwnerSummary>();
        try
        {
            // 로그인한 사용자의 정보를 가져온다.
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseOwnerSummary>(EnumResponseResult.Error,"ERROR_SESSION_TIMEOUT", "로그인 상태를 확인해주세요", null);
            
            // 로그인한 사용자의 모든 권한을 가져온다.
            ResponseList<ResponseUserRole> roleResponse = await _userService.GetRolesByUserAsync();
            
            // 권한이 없을경우 
            if(roleResponse.Items is {Count: 0} or null)
                return new ResponseData<ResponseOwnerSummary>(EnumResponseResult.Error,"NOT_ALLOWED", "권한이 없습니다.", null);
            
            // 대상하는 Claim 을 찾는다.
            List<string> foundClaims = FindClaims(roleResponse.Items);

            // 전체 권한을 가지고 있는지 여부 
            bool isPermitAll = foundClaims.Contains("process-result");

            // 현재 날짜 
            DateTime today = DateTime.Now;
            
            // 현재 년도를 가져온다.
            string year = today.ToString("yyyy");
            
            // 지난 년도를 가져온다.
            string beforeYear = today.AddYears(-1).ToString("yyyy");
            
            // 현재년도와 지난년도에 해당하는 예산 계획을 가져온다.
            List<DbModelBudgetPlan> budgetPlans = await _dbContext.BudgetPlans
                .AsNoTracking()
                .ToListAsync();
            
            // 모든 CBM 을 조회한다. 
            IQueryable<DbModelCountryBusinessManager> managersQuery = _dbContext.CountryBusinessManagers
                .AsNoTracking();
            
            // 전체 Permit 이 아닌경우 
            if (!isPermitAll)
                // 본인의 부서만 확인한다.
                managersQuery = managersQuery.Where(i => i.Id == user.CountryBusinessManagerId);

            // 전체 매니저를 조회한다.
            List<DbModelCountryBusinessManager> managers = await managersQuery.ToListAsync();
            
            // 조회된 정보로 CHK 500K 이하 정보를 찾는다.
            List<ResponseOwner> below500K = ComputeOwner(  budgetPlans, managers, year, beforeYear , false );
            
            // 조회된 정보로 CHK 500K 이상 정보를 찾는다.
            List<ResponseOwner> above500K = ComputeOwner(  budgetPlans, managers, year, beforeYear , true );

            // 객체를 생성한다.
            ResponseOwnerSummary data = new ResponseOwnerSummary();
            
            // 전체 Summary 정보
            // Below 500K 추가
            ResponseOwnerSummaryDetail detailBelow500K = new ResponseOwnerSummaryDetail
            {
                Sequence = 1,
                Title = "CAPEX below CHF500K",
                Items = below500K
            };
            // Above 500K 추가
            ResponseOwnerSummaryDetail detailAbove500K = new ResponseOwnerSummaryDetail
            {
                Sequence = 2,
                Title = "CAPEX above CHF500K",
                Items = above500K
            };
            // 전체 Sum
            List<ResponseOwner> total = SumOwner(below500K,above500K);
            ResponseOwnerSummaryDetail detailTotal = new ResponseOwnerSummaryDetail
            {
                Sequence = 3,
                Title = "Total",
                Items = total
            };
            
            // 모든 리스트를 주입한다.
            data.Items.Add(detailBelow500K);
            data.Items.Add(detailAbove500K);
            data.Items.Add(detailTotal);
            return new ResponseData<ResponseOwnerSummary>(EnumResponseResult.Success, "", "", data);
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseOwnerSummary>(EnumResponseResult.Error,"" ,"처리중 예외가 발생했습니다.", null);
            e.LogError(_logger);
        }

        return result;
    }

    private List<ResponseOwner> SumOwner(List<ResponseOwner> below500K, List<ResponseOwner> above500K)
    {
        List<ResponseOwner> result = new List<ResponseOwner>();

        try
        {
            // 두개의 리스트를 합친다.
            return
            below500K.Zip(above500K, (below, above) => new ResponseOwner
            {
                CountryBusinessManagerId = below.CountryBusinessManagerId, // 둘은 동일한 ID를 가정
                CountryBusinessManagerName = below.CountryBusinessManagerName, // 이름도 동일하다고 가정
                BudgetYear = below.BudgetYear + above.BudgetYear,
                BudgetApprovedYearBefore = below.BudgetApprovedYearBefore + above.BudgetApprovedYearBefore,
                BudgetApprovedYear = below.BudgetApprovedYear + above.BudgetApprovedYear,
                BudgetApprovedYearBeforeSum = below.BudgetApprovedYearBeforeSum + above.BudgetApprovedYearBeforeSum,
                BudgetRemainingYear = below.BudgetRemainingYear + above.BudgetRemainingYear
            }).ToList();
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }


    /// <summary>
    /// Below500K 의 정보를 찾아 바인딩한다.
    /// </summary>
    /// <param name="budgetPlans"></param>
    /// <param name="managers"></param>
    /// <param name="year"></param>
    /// <param name="beforeYear"></param>
    /// <param name="isAbove500K"></param>
    /// <returns></returns>
    private List<ResponseOwner> ComputeOwner( 
        List<DbModelBudgetPlan> budgetPlans ,
        List<DbModelCountryBusinessManager> managers ,
        string year, string beforeYear ,
        bool isAbove500K
        )
    {
        List<ResponseOwner> result = new List<ResponseOwner>();

        try
        {
            // 모든 매니저에 대해 처리
            foreach (DbModelCountryBusinessManager manager in managers)
            {
                // 쿼리 베이스
                IEnumerable<DbModelBudgetPlan> query = budgetPlans.Where(i =>
                    i.CountryBusinessManagerId == manager.Id &&
                    i.IsAbove500K == isAbove500K 
                ).ToList();
                
                // 당해년도 승인된/승인안됨 포함 전체 예산
                double budgetYear = query.Sum(i => i.BudgetTotal);
                
                // 승인된 전 년도 전체 예산
                double budgetApprovedYearBefore = query.Where(i =>
                    i.Year == beforeYear &&
                    i.IsApprovalDateValid
                ).Sum(i => i.BudgetTotal);
                
                // 승인된 이번년도 전체 예산
                double budgetApprovedYear = query.Where(i =>
                    i.Year == year &&
                    i.IsApprovalDateValid
                ).Sum(i => i.BudgetTotal);
                
                // 승인된 작년 + 이번년도 전체 예산 의 합
                double budgetApprovedYearBeforeSum = query.Where(i =>
                    new []{ year, beforeYear }.Contains(i.Year) &&
                    i.IsApprovalDateValid
                ).Sum(i => i.BudgetTotal);
                
                // 2024 년 남은 Budget
                // [올해 Budget] - [승인된 작년 + 이번년도 전체 예산]
                double budgetRemainingYear = budgetYear - budgetApprovedYearBeforeSum;

                // 데이터를 만든다.
                ResponseOwner add = new ResponseOwner
                {
                    CountryBusinessManagerId = manager.Id ,
                    CountryBusinessManagerName = manager.Name ,
                    BudgetYear = budgetYear,
                    BudgetApprovedYearBefore = budgetApprovedYearBefore,
                    BudgetApprovedYear = budgetApprovedYear,
                    BudgetApprovedYearBeforeSum = budgetApprovedYearBeforeSum,
                    BudgetRemainingYear = budgetRemainingYear
                };
                
                result.Add(add);
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 사용자의 Claim 을 찾는다.
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    private List<string> FindClaims(List<ResponseUserRole> roles)
    {
        List<string> result = new List<string>();
        try
        {
            // 모든 권한에서 찾는다.
            foreach (ResponseUserRole role in roles)
            {
                // Claim 이 없는경우 
                if(role.Claims == null || role.Claims.Count == 0)
                    continue;
                
                // 지정해놓은 Claim 에 해당하는 Claim 을 가지고 있다면
                List<string> foundClaims = role.Claims.Where(i => _requiredClaims.Contains(i.Value))
                    .Select(i => i.Value)
                    .ToList();
                
                // 추가한다.
                result.AddRange(foundClaims);
            }
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }
}