using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.DataModels;
using Models.Responses;
using Models.Responses.Process;
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
    /// Logger
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
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessOwnerSummary>> GetOwnerBudgetSummaryAsync(string year)
    {
        ResponseData<ResponseProcessOwnerSummary> result;
        try
        {
            // 로그인한 사용자의 정보를 가져온다.
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
            bool isPermitAll = true;

            // Query to Budget Plan List this year
            IQueryable<DbModelBudgetPlan> budgetPlansQuery = _dbContext.BudgetPlans
                .AsNoTracking()
                .Where(i => 
                            i.IsIncludeInStatistics && 
                            i.BaseYearForStatistics == int.Parse(year));
            
            // Query to Budget Approved List this year
            IQueryable<DbModelBudgetApproved> approvedQuery = _dbContext.BudgetApproved
                .AsNoTracking()
                .Where(i => 
                            // i.ApprovalStatus != EnumApprovalStatus.None && 
                            i.BaseYearForStatistics == int.Parse(year));

            // Query to all Country Business Managers
            IQueryable<DbModelCountryBusinessManager> managersQuery = _dbContext.CountryBusinessManagers
                .AsNoTracking()
                .OrderBy(i => i.Sequence);
            
            // 전체 Permit 이 아닌경우 
            if (!isPermitAll)
            {
                // 본인의 부서만 확인한다.
                managersQuery = managersQuery.Where(i => i.Id == user.CountryBusinessManagerId);
            }
            
            // 조회된 정보로 CHK 500K 이하 정보를 찾는다.
            List<ResponseProcessOwner> below500K = await ComputeOwnerAsync(  budgetPlansQuery, approvedQuery, managersQuery , false );
            
            // 조회된 정보로 CHK 500K 이상 정보를 찾는다.
            List<ResponseProcessOwner> above500K = await ComputeOwnerAsync(  budgetPlansQuery, approvedQuery, managersQuery , true );

            // 객체를 생성한다.
            ResponseProcessOwnerSummary data = new ResponseProcessOwnerSummary();
            
            // 전체 Summary 정보
            // Below 500K 추가
            ResponseProcessSummaryDetail<ResponseProcessOwner> detailBelow500K = new ResponseProcessSummaryDetail<ResponseProcessOwner> 
            {
                Sequence = 3,
                Title = "CAPEX below CHF500K",
                Items = below500K
            };
            // Above 500K 추가
            ResponseProcessSummaryDetail<ResponseProcessOwner> detailAbove500K = new ResponseProcessSummaryDetail<ResponseProcessOwner>
            {
                Sequence = 2,
                Title = "CAPEX above CHF500K",
                Items = above500K
            };
            // 전체 Sum
            List<ResponseProcessOwner> total = SumOwner(below500K,above500K);
            ResponseProcessSummaryDetail<ResponseProcessOwner> detailTotal = new ResponseProcessSummaryDetail<ResponseProcessOwner>
            {
                Sequence = 1,
                Title = "Total",
                Items = total
            };
            
            // 모든 리스트를 주입한다.
            data.Items.Add(detailTotal);
            data.Items.Add(detailAbove500K);
            data.Items.Add(detailBelow500K);
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
    /// <param name="year">년도 정보</param>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessBusinessUnitSummary>> GetBusinessUnitBudgetSummaryAsync(string year)
    {
        ResponseData<ResponseProcessBusinessUnitSummary> result;
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
            bool isPermitAll = true;
            
            // Query to Budget Plan List this year
            IQueryable<DbModelBudgetPlan> budgetPlansQuery = _dbContext.BudgetPlans
                .AsNoTracking()
                .Where(i => 
                    i.IsIncludeInStatistics && 
                    i.BaseYearForStatistics == int.Parse(year));
        
            // Query to Budget Approved List this year
            IQueryable<DbModelBudgetApproved> approvedQuery = _dbContext.BudgetApproved
                .AsNoTracking()
                .Where(i => 
                    // i.ApprovalStatus != EnumApprovalStatus.None && 
                    i.BaseYearForStatistics == int.Parse(year));
            
            // 모든 BusinessUnit 을 조회한다. 
            IQueryable<DbModelBusinessUnit> businessUnitQuery = _dbContext.BusinessUnits
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
                businessUnitQuery = businessUnitQuery.Where(i => userUnitIds.Contains(i.Id));
            }
            
            // 조회된 정보로 CHK 500K 이하 정보를 찾는다.
            List<ResponseProcessBusinessUnit> below500K = await ComputeBusinessUnitAsync( budgetPlansQuery, approvedQuery , businessUnitQuery, false );
            
            // 조회된 정보로 CHK 500K 이상 정보를 찾는다.
            List<ResponseProcessBusinessUnit> above500K = await ComputeBusinessUnitAsync(  budgetPlansQuery, approvedQuery , businessUnitQuery,  true );

            // 객체를 생성한다.
            ResponseProcessBusinessUnitSummary data = new ResponseProcessBusinessUnitSummary();
            
            // 전체 Summary 정보
            // Below 500K 추가
            ResponseProcessSummaryDetail<ResponseProcessBusinessUnit> detailBelow500K = new ResponseProcessSummaryDetail<ResponseProcessBusinessUnit>
            {
                Sequence = 3,
                Title = "CAPEX below CHF500K",
                Items = below500K
            };
            // Above 500K 추가
            ResponseProcessSummaryDetail<ResponseProcessBusinessUnit> detailAbove500K = new ResponseProcessSummaryDetail<ResponseProcessBusinessUnit>
            {
                Sequence = 2,
                Title = "CAPEX above CHF500K",
                Items = above500K
            };
            // 전체 Sum
            List<ResponseProcessBusinessUnit> total = SumUnit(below500K,above500K);
            ResponseProcessSummaryDetail<ResponseProcessBusinessUnit> detailTotal = new ResponseProcessSummaryDetail<ResponseProcessBusinessUnit>
            {
                Sequence = 1,
                Title = "Total",
                Items = total
            };
            
            // 모든 리스트를 주입한다.
            data.Items.Add(detailTotal);
            data.Items.Add(detailAbove500K);
            data.Items.Add(detailBelow500K);
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
    /// Get Approved Analysis for Below Amount
    /// ! If an authenticated user has only 'process-result-view' permissions, they can only view results they own.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetComputeStateOfPurchaseBelowAsync(string year)
    {
        return await GetComputeStateOfPurchaseAsync(year, false);
    }
    
    /// <summary>
    /// Get Approved Analysis for Above Amount
    /// ! If an authenticated user has only 'process-result-view' permissions, they can only view results they own.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetComputeStateOfPurchaseAboveAsync(string year)
    {
        return await GetComputeStateOfPurchaseAsync(year, true);
    }

    /// <summary>
    /// Get Compute StatueOfPurchaseAysnc
    /// </summary>
    /// <param name="year"></param>
    /// <param name="isAbove"></param>
    /// <returns></returns>
    private async Task<ResponseData<ResponseProcessApprovedSummary>> GetComputeStateOfPurchaseAsync(string year, bool isAbove)
    {
        ResponseData<ResponseProcessApprovedSummary> result;
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
            bool isPermitAll = true;

            // Get Year Ranges 
            int yearCurrent = year.ToInt();
       
            // Get all approved amounts in this year and before year
            IQueryable<DbModelBudgetApproved> queryApproved = _dbContext.BudgetApproved.AsNoTracking()
                .Where(i =>
                    new[] {yearCurrent }.Contains(i.BaseYearForStatistics)
                );            
            
            // Get all CountryBusiness Managers
            IQueryable<DbModelCountryBusinessManager> queryManagers = _dbContext.CountryBusinessManagers
                .AsNoTracking();
            
            // 전체 Permit 이 아닌경우 
            if (!isPermitAll)
                // 본인의 부서만 확인한다.
                queryManagers = queryManagers.Where(i => i.Id == user.CountryBusinessManagerId);

            // Managers with include BusinessUnit
            queryManagers = queryManagers
                .AsNoTracking()
                .OrderBy(i => i.Sequence);

            List<ResponseProcessApproved> thisYear = await ComputeApprovedAsync( queryApproved, queryManagers, yearCurrent , isAbove );

            // 객체를 생성한다.
            ResponseProcessApprovedSummary data = new ResponseProcessApprovedSummary();
            ResponseProcessSummaryDetail<ResponseProcessApproved>  thisYearDetail = new ResponseProcessSummaryDetail<ResponseProcessApproved> 
            {
                Sequence = 2,
                Title = $"Total",
                Items = thisYear
            };
            data.Items.Add(thisYearDetail);
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
                CountryBusinessManagerId = below.CountryBusinessManagerId, 
                CountryBusinessManagerName = below.CountryBusinessManagerName, 
                BudgetYear = below.BudgetYear + above.BudgetYear,
                ApprovedYear = below.ApprovedYear + above.ApprovedYear,
                RemainingYear = below.RemainingYear + above.RemainingYear
                // BudgetApprovedYearBefore = below.BudgetApprovedYearBefore + above.BudgetApprovedYearBefore,
                // BudgetApprovedYearSum = below.BudgetApprovedYearSum + above.BudgetApprovedYearSum,
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
                    ApprovedYear = below.ApprovedYear + above.ApprovedYear,
                    RemainingYear = below.RemainingYear + above.RemainingYear,
                    // BudgetApprovedYearBefore = below.BudgetApprovedYearBefore + above.BudgetApprovedYearBefore,
                    // BudgetApprovedYear = below.BudgetApprovedYear + above.BudgetApprovedYear,
                    // BudgetApprovedYearSum = below.BudgetApprovedYearSum + above.BudgetApprovedYearSum,
                    // BudgetRemainingYear = below.BudgetRemainingYear + above.BudgetRemainingYear
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
                    BusinessUnitId = below.BusinessUnitId,
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
    /// <param name="queryBudgets"></param>
    /// <param name="queryApproved"></param>
    /// <param name="queryManagers"></param>
    /// <param name="isAbove500K"></param>
    /// <returns></returns>
    private async Task<List<ResponseProcessOwner>> ComputeOwnerAsync(
        IQueryable<DbModelBudgetPlan> queryBudgets,
        IQueryable<DbModelBudgetApproved> queryApproved,
        IQueryable<DbModelCountryBusinessManager> queryManagers,
        bool isAbove500K)
    {
        List<ResponseProcessOwner> result = new List<ResponseProcessOwner>();
        try
        {
            // Group and sum budgets by manager
            var managerBudgets = queryManagers
                .GroupJoin(queryBudgets.Where(b => b.IsAbove500K == isAbove500K),
                    manager => manager.Id,
                    budget => budget.CountryBusinessManagerId,
                    (manager, budgets) => new { Manager = manager, Budgets = budgets })
                .SelectMany(
                    x => x.Budgets.DefaultIfEmpty(),
                    (x, budget) => new { x.Manager, Budget = budget })
                .GroupBy(x => new { x.Manager.Id, x.Manager.Name, x.Manager.Sequence })
                .Select(g => new
                {
                    ManagerId = g.Key.Id,
                    ManagerName = g.Key.Name,
                    ManagerSequence = g.Key.Sequence ,
                    BudgetYear = g.Sum(x => x.Budget != null ? x.Budget.BudgetTotal : 0)
                });

            // Group and sum approvals by manager
            var managerApprovals = queryManagers
                .GroupJoin(queryApproved.Where(a => a.IsAbove500K == isAbove500K),
                    manager => manager.Id,
                    approval => approval.CountryBusinessManagerId,
                    (manager, approvals) => new { Manager = manager, Approvals = approvals })
                .SelectMany(
                    x => x.Approvals.DefaultIfEmpty(),
                    (x, approval) => new { x.Manager, Approval = approval })
                .GroupBy(x => new { x.Manager.Id, x.Manager.Name })
                .Select(g => new
                {
                    ManagerId = g.Key.Id,
                    ApprovedYear = g.Sum(x => x.Approval != null ? x.Approval.ApprovalAmount : 0)
                });

            // Join budgets and approvals to calculate remaining
            var managerData = await managerBudgets.Join(managerApprovals,
                budget => budget.ManagerId,
                approval => approval.ManagerId,
                (budget, approval) => new ResponseProcessOwner
                {
                    CountryBusinessManagerId = budget.ManagerId,
                    CountryBusinessManagerName = budget.ManagerName,
                    BudgetYear = budget.BudgetYear,
                    ApprovedYear = approval.ApprovedYear,
                    ManagerSequence = budget.ManagerSequence ,
                    RemainingYear = budget.BudgetYear - approval.ApprovedYear ,
                })
                .OrderBy(i => i.ManagerSequence)
                .ToListAsync();

            result.AddRange(managerData);
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
    /// <param name="queryBudgets"></param>
    /// <param name="queryApproved"></param>
    /// <param name="queryBusinessUnit"></param>
    /// <param name="isAbove500K"></param>
    /// <returns></returns>
    private async Task<List<ResponseProcessBusinessUnit>> ComputeBusinessUnitAsync( 
        IQueryable<DbModelBudgetPlan> queryBudgets,
        IQueryable<DbModelBudgetApproved> queryApproved,
        IQueryable<DbModelBusinessUnit> queryBusinessUnit ,
        bool isAbove500K
        )
    {
        List<ResponseProcessBusinessUnit> result = new List<ResponseProcessBusinessUnit>();
        try
        {
            // Group and sum budgets by business units
            var businessUnitsBudgets = queryBusinessUnit
                .GroupJoin(queryBudgets.Where(i => i.IsAbove500K == isAbove500K),
                    businessUnit => businessUnit.Id,
                    budget => budget.BusinessUnitId,
                    (businessUnits, budgets) => new
                    {
                        BusinessUnit = businessUnits,
                        Budget = budgets
                    })
                .SelectMany(b => b.Budget.DefaultIfEmpty(),
                    (b, budget) => new {b.BusinessUnit, Budget = budget})
                .GroupBy(b => new {b.BusinessUnit.Id, b.BusinessUnit.Name, b.BusinessUnit.Sequence})
                .Select(c => new
                {
                    BusinessUnitId = c.Key.Id,
                    BusinessUnitName = c.Key.Name,
                    BusinesUnitSequence = c.Key.Sequence ,
                    BudgetYear = c.Sum(d => d.Budget != null ? d.Budget.BudgetTotal : 0)
                });
            
            // Group and sum approvals by Business Unit
            var businessUnitApproved = queryBusinessUnit
                .GroupJoin(queryApproved.Where(i => i.IsAbove500K == isAbove500K),
                    businessUnit => businessUnit.Id,
                    approved => approved.BusinessUnitId,
                    (businessUnit, approved) => new {BusinessUnit = businessUnit, Approvals = approved})
                .SelectMany(b => b.Approvals.DefaultIfEmpty(),
                    (b, approved) => new {b.BusinessUnit, Approved = approved})
                .GroupBy(c => new {c.BusinessUnit.Id, c.BusinessUnit.Name})
                .Select(d => new
                {
                    BusinessUnitId = d.Key.Id ,
                    ApprovedYear = d.Sum( e => e.Approved != null ? e.Approved.ApprovalAmount : 0 )
                });

            // Join budgets and approved to calculate Remaining
            var businessUnitData = await businessUnitsBudgets.Join(businessUnitApproved,
                budget => budget.BusinessUnitId,
                approved => approved.BusinessUnitId,
                (budget, approved) => new ResponseProcessBusinessUnit()
                {
                    BusinessUnitId = budget.BusinessUnitId,
                    BusinessUnitName = budget.BusinessUnitName,
                    BudgetYear = budget.BudgetYear,
                    ApprovedYear = approved.ApprovedYear,
                    BusinessUnitSequence = budget.BusinesUnitSequence ,
                    RemainingYear = budget.BudgetYear - approved.ApprovedYear
                })
                .OrderBy(i => i.BusinessUnitSequence)
                .ToListAsync();
            
            result.AddRange(businessUnitData);
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
    /// <param name="queryApproved"></param>
    /// <param name="queryManagers"></param>
    /// <param name="year"></param>
    /// <param name="isAbove500K"></param>
    /// <returns></returns>
    private async Task<List<ResponseProcessApproved>> ComputeApprovedAsync( 
        IQueryable<DbModelBudgetApproved> queryApproved ,
        IQueryable<DbModelCountryBusinessManager> queryManagers ,
        int year ,
        bool isAbove500K
        )
    {
        List<ResponseProcessApproved> result = new List<ResponseProcessApproved>();
        try
        {
            List<DbModelCountryBusinessManager> managers = await queryManagers.ToListAsync();
            
            // 모든 매니저에 대해 처리
            foreach (DbModelCountryBusinessManager manager in managers)
            {
                // 쿼리 베이스
                IEnumerable<DbModelBudgetApproved> query = queryApproved.Where(i =>
                    i.CountryBusinessManagerId == manager.Id &&
                    i.IsAbove500K == isAbove500K &&
                    i.BaseYearForStatistics == year
                ).ToList();
                
                result.Add(new ResponseProcessApproved()
                {
                    CountryBusinessManagerId = manager.Id,
                    CountryBusinessManagerName = manager.Name ,
                    PoIssueAmountSpending = query.Sum(i => i.SpendingAndIssuePoAmount),
                    PoIssueAmount = query.Sum(i => i.PoIssueAmount),
                    NotPoIssueAmount = query.Sum(i => i.NotPoIssueAmount),
                    ApprovedAmount = query.Sum(i => i.ApprovalAmount),
                });
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