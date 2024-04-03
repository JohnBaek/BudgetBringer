using Newtonsoft.Json;

namespace Features.Extensions;

/// <summary>
/// Object 확장
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    /// T 데이터를 T로 클로닝 한다.
    /// </summary>
    /// <param name="source">원본 데이터</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GenerateClone<T>(this T? source) where T : class
    {
        // 원본데이터가 유효하지 않을경우 
        if (source == null) 
            return null;
        
        // 질렬화 한다.
        string serialized = JsonConvert.SerializeObject(source);
        
        // 역직렬화 해서 반환한다.
        return JsonConvert.DeserializeObject<T>(serialized);
    }
}