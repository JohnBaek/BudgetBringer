using System.Linq.Expressions;
using System.Reflection;
using Features.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DataModels;
using Providers.Services.Interfaces;

namespace Providers.Services.Implements;

/// <summary>
/// 디스패처 서비스 구현체
/// </summary>
public class DispatchService : IDispatchService
{
    /// <summary>
    /// DB Context
    /// </summary>
    private readonly AnalysisDbContext _dbContext;

    /// <summary>
    /// 로거
    /// </summary>
    private readonly ILogger<DispatchService> _logger;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="logger">로거</param>
    /// <param name="dbContext">dbContext</param>
    public DispatchService(ILogger<DispatchService> logger, AnalysisDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// 특정 테이블 T DbSet 으로부터 keyColumn 과 nameColumn 에 해당하는 컬럼이 있으면
    /// keyColumn 컬럼에 id 으로 nameColumn 을 조회해 리턴한다.
    /// </summary>
    /// <param name="keyColumn"></param>
    /// <param name="nameColumn"></param>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> GetNameByIdAsync<T>(string keyColumn, string nameColumn, Guid id) where T : class
    {
        string result;
        try
        {
            // T 를 이용하여 Queryable 생성
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            
            // 테이블 alias 설정
            ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
            
            // 멤버 지정 alias 에서 nameColumn
            MemberExpression keyColumnExpression = Expression.PropertyOrField(parameter, keyColumn);
            
            // 조건 생성 key : id
            BinaryExpression condition = Expression.Equal(keyColumnExpression, Expression.Constant(id));
            
            // 동적 Where 쿼리 생성
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            
            // 데이터를 조회한다.
            T? entity = await query.FirstOrDefaultAsync(lambda);
            
            // 데이터가 존재하지 않는 경우 
            if (entity == null) 
                return "";
            
            PropertyInfo? nameProperty = entity.GetType().GetProperty(nameColumn);
            return nameProperty?.GetValue(entity)?.ToString() ?? "";
        }
        catch (Exception e)
        {
            result = "";
            e.LogError(_logger);
        }

        return result;
    }

    /// <summary>
    /// 특정 테이블 T DbSet 으로부터 keyColumn 과 nameColumn 에 해당하는 컬럼이 있으면
    /// keyColumn 컬럼에 id 으로 nameColumn 을 조회해 리턴한다.
    /// </summary>
    /// <param name="keyColumn"></param>
    /// <param name="nameColumn"></param>
    /// <param name="findKeyword"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<string> GetValueFromAsync<T>(string keyColumn, string nameColumn, string findKeyword) where T : class
    {
        string result;
        try
        {
            // T 를 이용하여 Queryable 생성
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            
            // 테이블 alias 설정
            ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
            
            // 멤버 지정 alias 에서 nameColumn
            MemberExpression keyColumnExpression = Expression.PropertyOrField(parameter, keyColumn);
            
            // 조건 생성 key : id
            BinaryExpression condition = Expression.Equal(keyColumnExpression, Expression.Constant(findKeyword));
            
            // 동적 Where 쿼리 생성
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            
            // 데이터를 조회한다.
            T? entity = await query.FirstOrDefaultAsync(lambda);
            
            // 데이터가 존재하지 않는 경우 
            if (entity == null) 
                return "";
            
            PropertyInfo? nameProperty = entity.GetType().GetProperty(nameColumn);
            return nameProperty?.GetValue(entity)?.ToString() ?? "";
        }
        catch (Exception e)
        {
            result = "";
            e.LogError(_logger);
        }

        return result;
    }
    //
    // /// <summary>
    // /// Get Id value by name column
    // /// </summary>
    // /// <param name="nameColumn"></param>
    // /// <param name="idColumn"></param>
    // /// <typeparam name="T"></typeparam>
    // /// <returns></returns>
    // public Task<string> GetIdByNameAsync<T>(string nameColumn, string idColumn) where T : class
    // {
    //     string result;
    //     try
    //     {
    //         // T 를 이용하여 Queryable 생성
    //         IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
    //         
    //         // 테이블 alias 설정
    //         ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
    //         
    //         // 멤버 지정 alias 에서 nameColumn
    //         MemberExpression keyColumnExpression = Expression.PropertyOrField(parameter, nameColumn);
    //         
    //         // 조건 생성 key : id
    //         BinaryExpression condition = Expression.Equal(keyColumnExpression, Expression.Constant(id));
    //         
    //         // 동적 Where 쿼리 생성
    //         Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
    //         
    //         // 데이터를 조회한다.
    //         T? entity = await query.FirstOrDefaultAsync(lambda);
    //         
    //         // 데이터가 존재하지 않는 경우 
    //         if (entity == null) 
    //             return "";
    //         
    //         PropertyInfo? nameProperty = entity.GetType().GetProperty(nameColumn);
    //         return nameProperty?.GetValue(entity)?.ToString() ?? "";
    //     }
    //     catch (Exception e)
    //     {
    //         result = "";
    //         e.LogError(_logger);
    //     }
    //
    //     return result;
    // }
}