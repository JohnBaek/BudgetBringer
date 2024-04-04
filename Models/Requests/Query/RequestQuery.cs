using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using Models.Common.Enums;

namespace Models.Requests.Query;

/// <summary>
/// Query 요청 
/// </summary>
public class RequestQuery
{
    /// <summary>
    /// 스킵
    /// </summary>
    [Required]
    [DefaultValue(0)]
    public int Skip { get; set; } = 0;

    /// <summary>
    /// 페이지 카운트 
    /// </summary>
    [Required]
    [DefaultValue(20)]
    public int PageCount { get; set; } = 20;

    /// <summary>
    /// 검색 키워드 
    /// </summary>
    public List<string> SearchKeywords { get; set; } = [];
    
    /// <summary>
    /// 검색 필드
    /// </summary>
    public List<string> SearchFields { get; set; } = [];

    /// <summary>
    /// 검색 메타 정보 
    /// </summary>
    [JsonIgnore]
    public List<RequestQuerySearchMeta> SearchMetas { get; set; } = [];

    /// <summary>
    /// 메타 정보를 추가한다.
    /// </summary>
    /// <param name="searchType"></param>
    /// <param name="fieldName"></param>
    public void AddSearchDefine(EnumQuerySearchType searchType, string fieldName)
    {
        SearchMetas.Add(new RequestQuerySearchMeta
        {
            SearchType = searchType ,
            Field = fieldName ,
        });
    }
}