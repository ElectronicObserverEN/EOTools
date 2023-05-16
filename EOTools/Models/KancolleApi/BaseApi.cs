using System.Text.Json.Serialization;

namespace EOTools.Models.KancolleApi;

public class BaseApi<T>
{
    [JsonPropertyName("api_data")]
    public T? ApiData { get; set; }
}
