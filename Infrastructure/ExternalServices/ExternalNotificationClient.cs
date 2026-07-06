using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WarehouseApi.DTOs;

namespace WarehouseApi.Infrastructure.ExternalServices;

public class ExternalNotificationClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalNotificationClient> _logger;

    public ExternalNotificationClient(HttpClient httpClient, ILogger<ExternalNotificationClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task SendMovementNotificationAsync(MovementPayload payload)
    {
        _logger.LogInformation("Mengirim notifikasi mutasi barang ke API eksternal... SKU: {Sku}, Quantity: {Quantity}", payload.Sku, payload.Quantity);

        var response = await _httpClient.PostAsJsonAsync("notifications", payload);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Gagal mengirim notifikasi ke API eksternal. Status Code: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"API Eksternal merespon dengan status {response.StatusCode}");
        }

        _logger.LogInformation("Notifikasi mutasi barang berhasil dikirim ke API eksternal.");
    }
}
