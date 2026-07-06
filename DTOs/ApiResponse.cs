using System.Text.Json.Serialization;

namespace WarehouseApi.DTOs;

public class ApiResponse<T>
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("data")]
    public required T Data { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0.0";
}
