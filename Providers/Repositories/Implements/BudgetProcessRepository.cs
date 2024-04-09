using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Models.Responses.Process.ProcessApproved;
using Models.Responses.Process.ProcessBusinessUnit;
using Models.Responses.Process.ProcessOwner;
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
    public async Task<ResponseData<ResponseProcessOwnerSummary>> GetOwnerBudgetSummaryAsync()
    {
        ResponseData<ResponseProcessOwnerSummary> result = new ResponseData<ResponseProcessOwnerSummary>();
        try
        {
            // 로그인한 사용자의 정보를 가져온다.
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseProcessOwnerSummary>(EnumResponseResult.Error,"ERROR_SESSION_TIMEOUT", "로그인 상태를 확인해주세요", null);
            
            // 로그인한 사용자의 모든 권한을 가져온다.
            ResponseList<ResponseUserRole> roleResponse = await _userService.GetRolesByUserAsync();
            
            // 권한이 없을경우 
            if(roleResponse.Items is {Count: 0} or null)
                return new ResponseData<ResponseProcessOwnerSummary>(EnumResponseResult.Error,"NOT_ALLOWED", "권한이 없습니다.", null);
            
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
            List<ResponseProcessOwner> below500K = ComputeOwner(  budgetPlans, managers, year, beforeYear , false );
            
            // 조회된 정보로 CHK 500K 이상 정보를 찾는다.
            List<ResponseProcessOwner> above500K = ComputeOwner(  budgetPlans, managers, year, beforeYear , true );

            // 객체를 생성한다.
            ResponseProcessOwnerSummary data = new ResponseProcessOwnerSummary();
            
            // 전체 Summary 정보
            // Below 500K 추가
            ResponseProcessOwnerSummaryDetail detailBelow500K = new ResponseProcessOwnerSummaryDetail
            {
                Sequence = 1,
                Title = "CAPEX below CHF500K",
                Items = below500K
            };
            // Above 500K 추가
            ResponseProcessOwnerSummaryDetail detailAbove500K = new ResponseProcessOwnerSummaryDetail
            {
                Sequence = 2,
                Title = "CAPEX above CHF500K",
                Items = above500K
            };
            // 전체 Sum
            List<ResponseProcessOwner> total = SumOwner(below500K,above500K);
            ResponseProcessOwnerSummaryDetail detailTotal = new ResponseProcessOwnerSummaryDetail
            {
                Sequence = 3,
                Title = "Total",
                Items = total
            };
            
            // 모든 리스트를 주입한다.
            data.Items.Add(detailBelow500K);
            data.Items.Add(detailAbove500K);
            data.Items.Add(detailTotal);
            return new ResponseData<ResponseProcessOwnerSummary>(EnumResponseResult.Success, "", "", data);
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseProcessOwnerSummary>(EnumResponseResult.Error,"" ,"처리중 예외가 발생했습니다.", null);
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 비지니스 유닛별 프로세스 정보를 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessBusinessUnitSummary>> GetBusinessUnitBudgetSummaryAsync()
    {
        ResponseData<ResponseProcessBusinessUnitSummary> result = new ResponseData<ResponseProcessBusinessUnitSummary>();
        try
        {
            // 로그인한 사용자의 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseProcessBusinessUnitSummary>(EnumResponseResult.Error,"ERROR_SESSION_TIMEOUT", "로그인 상태를 확인해주세요", null);
            
            // 로그인한 사용자의 모든 권한을 가져온다.
            ResponseList<ResponseUserRole> roleResponse = await _userService.GetRolesByUserAsync();
            
            // 권한이 없을경우 
            if(roleResponse.Items is {Count: 0} or null)
                return new ResponseData<ResponseProcessBusinessUnitSummary>(EnumResponseResult.Error,"NOT_ALLOWED", "권한이 없습니다.", null);
            
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
            
            // 모든 BusinessUnit 을 조회한다. 
            IQueryable<DbModelBusinessUnit> managersQuery = _dbContext.BusinessUnits
                .AsNoTracking();

            // 전체 Permit 이 아닌경우 
            if (!isPermitAll)
            {
                // 로그인한 사용자의 유닛정보를 조회한다.
                IQueryable<Guid> userUnitIds =
                    _dbContext.CountryBusinessManagerBusinessUnits
                        .AsNoTracking()
                        .Where(i => i.CountryBusinessManagerId == user.CountryBusinessManagerId)
                        .Select(i => i.BusinessUnitId);
 
                // 본인의 부서만 확인한다.
                managersQuery = managersQuery.Where(i => userUnitIds.Contains(i.Id));
            }
        
            // 전체 비지니스 유닛을 조회한다.
            List<DbModelBusinessUnit> managers = await managersQuery.ToListAsync();
            
            // 조회된 정보로 CHK 500K 이하 정보를 찾는다.
            List<ResponseProcessBusinessUnit> below500K = ComputeBusinessUnit(  budgetPlans, managers, year, beforeYear , false );
            
            // 조회된 정보로 CHK 500K 이상 정보를 찾는다.
            List<ResponseProcessBusinessUnit> above500K = ComputeBusinessUnit(  budgetPlans, managers, year, beforeYear , true );

            // 객체를 생성한다.
            ResponseProcessBusinessUnitSummary data = new ResponseProcessBusinessUnitSummary();
            
            // 전체 Summary 정보
            // Below 500K 추가
            ResponseProcessBusinessUnitSummaryDetail detailBelow500K = new ResponseProcessBusinessUnitSummaryDetail
            {
                Sequence = 1,
                Title = "CAPEX below CHF500K",
                Items = below500K
            };
            // Above 500K 추가
            ResponseProcessBusinessUnitSummaryDetail detailAbove500K = new ResponseProcessBusinessUnitSummaryDetail
            {
                Sequence = 2,
                Title = "CAPEX above CHF500K",
                Items = above500K
            };
            // 전체 Sum
            List<ResponseProcessBusinessUnit> total = SumUnit(below500K,above500K);
            ResponseProcessBusinessUnitSummaryDetail detailTotal = new ResponseProcessBusinessUnitSummaryDetail
            {
                Sequence = 3,
                Title = "Total",
                Items = total
            };
            
            // 모든 리스트를 주입한다.
            data.Items.Add(detailBelow500K);
            data.Items.Add(detailAbove500K);
            data.Items.Add(detailTotal);
            return new ResponseData<ResponseProcessBusinessUnitSummary>(EnumResponseResult.Success, "", "", data);
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseProcessBusinessUnitSummary>(EnumResponseResult.Error,"" ,"처리중 예외가 발생했습니다.", null);
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 승인된 금액 편성 진행상황을 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetApprovedAmountSummaryAsync()
    {
        ResponseData<ResponseProcessApprovedSummary> result = new ResponseData<ResponseProcessApprovedSummary>();
        try
        {
            // 로그인한 사용자의 정보를 가져온다.
            // 로그인한 사용자 정보를 가져온다.
            DbModelUser? user = await _userRepository.GetAuthenticatedUser();

            // 사용자 정보가 없는경우 
            if(user == null)
                return new ResponseData<ResponseProcessApprovedSummary>(EnumResponseResult.Error,"ERROR_SESSION_TIMEOUT", "로그인 상태를 확인해주세요", null);
            
            // 로그인한 사용자의 모든 권한을 가져온다.
            ResponseList<ResponseUserRole> roleResponse = await _userService.GetRolesByUserAsync();
            
            // 권한이 없을경우 
            if(roleResponse.Items is {Count: 0} or null)
                return new ResponseData<ResponseProcessApprovedSummary>(EnumResponseResult.Error,"NOT_ALLOWED", "권한이 없습니다.", null);
            
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
            List<DbModelBudgetApproved> budgetApproveds = await _dbContext.BudgetApproved
                .Where(i => new[] { year,beforeYear }.Contains(i.Year))
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
            List<DbModelCountryBusinessManager> managers = await managersQuery
                .Include(i => i.CountryBusinessManagerBusinessUnits)
                    .ThenInclude(v => v.BusinessUnit)
                .AsNoTracking()
                .ToListAsync();
            
            // 조회된 정보로 CHK 500K 이하 전년도 정보를 찾는다.
            List<ResponseProcessApproved> yearBeforeBelow500K = ComputeApproved(  budgetApproveds, managers, beforeYear , false );
            
            // 조회된 정보로 CHK 500K 이하 당해년도 정보를 찾는다.
            List<ResponseProcessApproved> yearBelow500K = ComputeApproved(  budgetApproveds, managers, year , false );

            // 객체를 생성한다.
            ResponseProcessApprovedSummary data = new ResponseProcessApprovedSummary();
            
            // 전체 Summary 정보
            // 전년
            ResponseProcessApprovedSummaryDetail beforeYearDetailBelow500K = new ResponseProcessApprovedSummaryDetail
            {
                Sequence = 1,
                Title = $"{beforeYear}FY",
                Items = yearBeforeBelow500K
            };
            // 당해년
            ResponseProcessApprovedSummaryDetail yearDetailAbove500K = new ResponseProcessApprovedSummaryDetail
            {
                Sequence = 2,
                Title = $"{year}FY",
                Items = yearBelow500K
            };
            // 전체 Sum
            List<ResponseProcessApproved> total = SumApproved(yearBeforeBelow500K,yearBelow500K);
            ResponseProcessApprovedSummaryDetail detailTotal = new ResponseProcessApprovedSummaryDetail
            {
                Sequence = 3,
                Title = $"{beforeYear}FY & {year}FY",
                Items = total
            };
            
            // 모든 리스트를 주입한다.
            data.Items.Add(beforeYearDetailBelow500K);
            data.Items.Add(yearDetailAbove500K);
            data.Items.Add(detailTotal);
            return new ResponseData<ResponseProcessApprovedSummary>(EnumResponseResult.Success, "", "", data);
        }
        catch (Exception e)
        {
            result = new ResponseData<ResponseProcessApprovedSummary>(EnumResponseResult.Error,"" ,"처리중 예외가 발생했습니다.", null);
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 오너 정보를 합산한다.
    /// </summary>
    /// <param name="below500K"></param>
    /// <param name="above500K"></param>
    /// <returns></returns>
    private List<ResponseProcessOwner> SumOwner(List<ResponseProcessOwner> below500K, List<ResponseProcessOwner> above500K)
    {
        List<ResponseProcessOwner> result = new List<ResponseProcessOwner>();

        try
        {
            // 두개의 리스트를 합친다.
            return
            below500K.Zip(above500K, (below, above) => new ResponseProcessOwner
            {
                CountryBusinessManagerId = below.CountryBusinessManagerId, // 둘은 동일한 ID를 가정
                CountryBusinessManagerName = below.CountryBusinessManagerName, // 이름도 동일하다고 가정
                BudgetYear = below.BudgetYear + above.BudgetYear,
                BudgetApprovedYearBefore = below.BudgetApprovedYearBefore + above.BudgetApprovedYearBefore,
                BudgetApprovedYear = below.BudgetApprovedYear + above.BudgetApprovedYear,
                BudgetApprovedYearSum = below.BudgetApprovedYearSum + above.BudgetApprovedYearSum,
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
    /// 오너 정보를 합산한다.
    /// </summary>
    /// <param name="below500K"></param>
    /// <param name="above500K"></param>
    /// <returns></returns>
    private List<ResponseProcessBusinessUnit> SumUnit(List<ResponseProcessBusinessUnit> below500K, List<ResponseProcessBusinessUnit> above500K)
    {
        List<ResponseProcessBusinessUnit> result = new List<ResponseProcessBusinessUnit>();

        try
        {
            // 두개의 리스트를 합친다.
            return
                below500K.Zip(above500K, (below, above) => new ResponseProcessBusinessUnit
                {
                    BusinessUnitId = below.BusinessUnitId, // 둘은 동일한 ID를 가정
                    BusinessUnitName = below.BusinessUnitName, // 이름도 동일하다고 가정
                    BudgetYear = below.BudgetYear + above.BudgetYear,
                    BudgetApprovedYearBefore = below.BudgetApprovedYearBefore + above.BudgetApprovedYearBefore,
                    BudgetApprovedYear = below.BudgetApprovedYear + above.BudgetApprovedYear,
                    BudgetApprovedYearSum = below.BudgetApprovedYearSum + above.BudgetApprovedYearSum,
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
    /// 승인정보를 합산한다.
    /// </summary>
    /// <param name="beforeYear"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    private List<ResponseProcessApproved> SumApproved(List<ResponseProcessApproved> beforeYear, List<ResponseProcessApproved> year)
    {
        List<ResponseProcessApproved> result = new List<ResponseProcessApproved>();

        try
        {
            // 두개의 리스트를 합친다.
            return
                beforeYear.Zip(year, (below, above) => new ResponseProcessApproved()
                {
                    CountryBusinessManagerId = below.CountryBusinessManagerId, // 둘은 동일한 ID를 가정
                    CountryBusinessManagerName = below.CountryBusinessManagerName, // 이름도 동일하다고 가정
                    PoIssueAmountSpending = below.PoIssueAmountSpending + above.PoIssueAmountSpending,
                    PoIssueAmount = below.PoIssueAmount + above.PoIssueAmount,
                    NotPoIssueAmount = below.NotPoIssueAmount + above.NotPoIssueAmount ,
                    ApprovedAmount = below.ApprovedAmount + above.ApprovedAmount,
                    BusinessUnits = SumApprovedBusinessUnits(below.BusinessUnits, above.BusinessUnits)
                }).ToList();
        }
        catch (Exception e)
        {
            e.LogError(_logger);
        }

        return result;
    }
    
    
    /// <summary>
    /// 내부의 비지니스 유닛별 합산을 처리한다.
    /// </summary>
    /// <param name="beforeYearBusinessUnits"></param>
    /// <param name="yearBusinessUnits"></param>
    /// <returns></returns>
    private List<ResponseProcessApproved> SumApprovedBusinessUnits(List<ResponseProcessApproved> beforeYearBusinessUnits, List<ResponseProcessApproved> yearBusinessUnits)
    {
        var combined = new List<ResponseProcessApproved>();

        // 비즈니스 유닛 ID 별로 그룹핑하고, 각각에 대해 합산을 진행
        var allBusinessUnits = beforeYearBusinessUnits.Concat(yearBusinessUnits).ToList();
        var groupedById = allBusinessUnits.GroupBy(bu => bu.CountryBusinessManagerId);

        foreach (var group in groupedById)
        {
            var groupedBusinessUnits = group.ToList();
            var aggregatedBusinessUnit = groupedBusinessUnits.First();

            foreach (var bu in groupedBusinessUnits.Skip(1))
            {
                aggregatedBusinessUnit.PoIssueAmountSpending += bu.PoIssueAmountSpending;
                aggregatedBusinessUnit.PoIssueAmount += bu.PoIssueAmount;
                aggregatedBusinessUnit.NotPoIssueAmount += bu.NotPoIssueAmount;
                aggregatedBusinessUnit.ApprovedAmount += bu.ApprovedAmount;
                // 재귀 호출
                aggregatedBusinessUnit.BusinessUnits = SumApprovedBusinessUnits(aggregatedBusinessUnit.BusinessUnits, bu.BusinessUnits);
            }

            combined.Add(aggregatedBusinessUnit);
        }

        return combined;
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
    private List<ResponseProcessOwner> ComputeOwner( 
        List<DbModelBudgetPlan> budgetPlans ,
        List<DbModelCountryBusinessManager> managers ,
        string year, string beforeYear ,
        bool isAbove500K
        )
    {
        List<ResponseProcessOwner> result = new List<ResponseProcessOwner>();

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
                ResponseProcessOwner add = new ResponseProcessOwner
                {
                    CountryBusinessManagerId = manager.Id ,
                    CountryBusinessManagerName = manager.Name ,
                    BudgetYear = budgetYear,
                    BudgetApprovedYearBefore = budgetApprovedYearBefore,
                    BudgetApprovedYear = budgetApprovedYear,
                    BudgetApprovedYearSum = budgetApprovedYearBeforeSum,
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
    /// Below500K 의 정보를 찾아 바인딩한다.
    /// </summary>
    /// <param name="budgetPlans"></param>
    /// <param name="units"></param>
    /// <param name="year"></param>
    /// <param name="beforeYear"></param>
    /// <param name="isAbove500K"></param>
    /// <returns></returns>
    private List<ResponseProcessBusinessUnit> ComputeBusinessUnit( 
        List<DbModelBudgetPlan> budgetPlans ,
        List<DbModelBusinessUnit> units ,
        string year, string beforeYear ,
        bool isAbove500K
        )
    {
        List<ResponseProcessBusinessUnit> result = new List<ResponseProcessBusinessUnit>();

        try
        {
            // 모든 매니저에 대해 처리
            foreach (DbModelBusinessUnit unit in units)
            {
                // 쿼리 베이스
                IEnumerable<DbModelBudgetPlan> query = budgetPlans.Where(i =>
                    i.BusinessUnitId == unit.Id &&
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
                ResponseProcessBusinessUnit add = new ResponseProcessBusinessUnit
                {
                    BusinessUnitId = unit.Id ,
                    BusinessUnitName = unit.Name ,
                    BudgetYear = budgetYear,
                    BudgetApprovedYearBefore = budgetApprovedYearBefore,
                    BudgetApprovedYear = budgetApprovedYear,
                    BudgetApprovedYearSum = budgetApprovedYearBeforeSum,
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
    /// Below500K 의 승인금액 정보를 찾아 바인딩한다.
    /// </summary>
    /// <param name="budgetApproveds"></param>
    /// <param name="managers"></param>
    /// <param name="year"></param>
    /// <param name="isAbove500K"></param>
    /// <returns></returns>
    private List<ResponseProcessApproved> ComputeApproved( 
        List<DbModelBudgetApproved> budgetApproveds ,
        List<DbModelCountryBusinessManager> managers ,
        string year ,
        bool isAbove500K
        )
    {
        List<ResponseProcessApproved> result = new List<ResponseProcessApproved>();

        try
        {
            // 모든 매니저에 대해 처리
            foreach (DbModelCountryBusinessManager manager in managers)
            {
                // 쿼리 베이스
                IEnumerable<DbModelBudgetApproved> query = budgetApproveds.Where(i =>
                    i.CountryBusinessManagerId == manager.Id &&
                    i.IsAbove500K == isAbove500K &&
                    i.Year == year
                ).ToList();
                
                // 들어갈 데이터 
                ResponseProcessApproved managerApproved = new ResponseProcessApproved();

                // 매니저가 소유한 비지니스 유닛에 대한 처리 
                foreach (var businessUnit in manager.CountryBusinessManagerBusinessUnits)
                {
                    // 승인된 금액 중 PO 발행건 합산금액
                    double poIssueAmountSpending = query
                        .Where(i => i.ApprovalStatus == EnumApprovalStatus.InVoicePublished)
                        .Sum(i => i.ApprovalAmount );
                
                    // 승인된 금액 중 PO 미 발행건 합산금액
                    double poIssueAmount = query
                        .Where(i => i.ApprovalStatus == EnumApprovalStatus.PoPublished)
                        .Sum(i => i.ApprovalAmount );
                
                    // PO 미 발행건 합산금액 
                    double notPoIssueAmount = query
                        .Where(i => i.ApprovalStatus == EnumApprovalStatus.PoNotYetPublished)
                        .Sum(i => i.ApprovalAmount );
                
                    // 승인된 금액 전체 [승인된 금액 중 PO 발행건 합산금액] + [PO 미 발행건 합산금액 ]
                    double approvedAmount = poIssueAmountSpending + notPoIssueAmount;
                
                    // 데이터를 만든다.
                    ResponseProcessApproved add = new ResponseProcessApproved
                    {
                        BusinessUnitId = businessUnit.BusinessUnit!.Id ,
                        BusinessUnitName = businessUnit.BusinessUnit!.Name ,
                        CountryBusinessManagerId = manager.Id ,
                        CountryBusinessManagerName = manager.Name ,
                        PoIssueAmountSpending = poIssueAmountSpending,
                        PoIssueAmount = poIssueAmount,
                        NotPoIssueAmount = notPoIssueAmount,
                        ApprovedAmount = approvedAmount,
                    };
                    managerApproved.BusinessUnits.Add(add);
                }
                
                // 데이터를 만든다.
                managerApproved.CountryBusinessManagerId = manager.Id ;
                managerApproved.CountryBusinessManagerName = manager.Name ;
                managerApproved.PoIssueAmountSpending = managerApproved.BusinessUnits.Sum(i => i.PoIssueAmountSpending);
                managerApproved.PoIssueAmount = managerApproved.BusinessUnits.Sum(i => i.PoIssueAmount);
                managerApproved.NotPoIssueAmount = managerApproved.BusinessUnits.Sum(i => i.NotPoIssueAmount);
                managerApproved.ApprovedAmount = managerApproved.BusinessUnits.Sum(i => i.ApprovedAmount);
                result.Add(managerApproved);
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