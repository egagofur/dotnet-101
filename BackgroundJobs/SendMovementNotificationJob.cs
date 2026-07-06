using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using WarehouseApi.DTOs;
using WarehouseApi.Infrastructure.ExternalServices;

namespace WarehouseApi.BackgroundJobs;

public class SendMovementNotificationJob : IInvocable, IInvocableWithPayload<MovementPayload>
{
    private readonly ExternalNotificationClient _client;
    private readonly ILogger<SendMovementNotificationJob> _logger;

    public MovementPayload Payload { get; set; } = null!;

    public SendMovementNotificationJob(ExternalNotificationClient client, ILogger<SendMovementNotificationJob> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task Invoke()
    {
        _logger.LogInformation("Menjalankan job SendMovementNotificationJob di background queue...");

        try
        {
            await _client.SendMovementNotificationAsync(Payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saat menjalankan SendMovementNotificationJob untuk SKU: {Sku}", Payload.Sku);
            throw;
        }
    }
}
