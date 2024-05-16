namespace Providers.Services.Interfaces;

/// <summary>
/// 디스패처 서비스
/// </summary>
public interface IDispatchService
{
    /// <summary>
    /// 특정 테이블 T DbSet 으로부터 keyColumn 과 nameColumn 에 해당하는 컬럼이 있으면
    /// keyColumn 컬럼에 id 으로 nameColumn 을 조회해 리턴한다.
    /// </summary>
    /// <param name="keyColumn"></param>
    /// <param name="nameColumn"></param>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<string> GetNameByIdAsync<T>(string keyColumn , string nameColumn , Guid id) where T : class;
    
    
    /// <summary>
    /// 특정 테이블 T DbSet 으로부터 keyColumn 과 nameColumn 에 해당하는 컬럼이 있으면
    /// keyColumn 컬럼에 id 으로 nameColumn 을 조회해 리턴한다.
    /// </summary>
    /// <param name="keyColumn"></param>
    /// <param name="nameColumn"></param>
    /// <param name="findKeyword"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<string> GetValueFromAsync<T>(string keyColumn , string nameColumn , string findKeyword) where T : class;
}