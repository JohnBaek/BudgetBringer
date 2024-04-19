using Features.Extensions;
using Microsoft.Extensions.Logging;
using Models.Common.Enums;
using Models.Responses;
using Models.Responses.Process.ProcessApproved;
using Models.Responses.Process.ProcessBusinessUnit;
using Models.Responses.Process.ProcessOwner;
using Providers.Repositories.Interfaces;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 예산 계획 서비스 인터페이스
/// </summary>
public class BudgetProcessService : IBudgetProcessService
{
    /// <summary>
    /// 리파지토리
    /// </summary>
    private readonly IBudgetProcessRepository _repository;
    
    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<BudgetProcessService> _logger;


    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="logger"></param>
    public BudgetProcessService(IBudgetProcessRepository repository, ILogger<BudgetProcessService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// 오너별 예산 편성 진행상황을 가져온다.
    /// 단, process-result-view 의 Claim 만 소유한 경우 로그인한 사용자의 CountryManagerId 가 일치하는
    /// 정보만 나와야한다. 
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessOwnerSummary>> GetOwnerBudgetSummaryAsync()
    {
        ResponseData<ResponseProcessOwnerSummary> response;
        
        try
        {
            response = await _repository.GetOwnerBudgetSummaryAsync();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseProcessOwnerSummary>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// 비지니스 유닛별 프로세스 정보를 가져온다.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessBusinessUnitSummary>> GetBusinessUnitBudgetSummaryAsync()
    {
        ResponseData<ResponseProcessBusinessUnitSummary> response;
        
        try
        {
            response = await _repository.GetBusinessUnitBudgetSummaryAsync();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseProcessBusinessUnitSummary>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// Get Approved Analysis for Below Amount
    /// ! If an authenticated user has only 'process-result-view' permissions, they can only view results they own.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetApprovedBelowAmountSummaryAsync()
    {
        ResponseData<ResponseProcessApprovedSummary> response;
        
        try
        {
            response = await _repository.GetApprovedBelowAmountSummaryAsync();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseProcessApprovedSummary>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }

    /// <summary>
    /// Get Approved Analysis for Above Amount
    /// ! If an authenticated user has only 'process-result-view' permissions, they can only view results they own.
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<ResponseProcessApprovedSummary>> GetApprovedAboveAmountSummaryAsync()
    {
        ResponseData<ResponseProcessApprovedSummary> response;
        
        try
        {
            response = await _repository.GetApprovedAboveAmountSummaryAsync();
        }
        catch (Exception e)
        {
            response = new ResponseData<ResponseProcessApprovedSummary>(EnumResponseResult.Error,"","처리중 예외가 발생했습니다.",null);
            e.LogError(_logger);
        }

        return response;
    }
}