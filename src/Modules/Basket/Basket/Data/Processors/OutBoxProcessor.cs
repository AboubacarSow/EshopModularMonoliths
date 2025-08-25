using System.Text.Json;

namespace Basket.Data.Processors;

public class OutBoxProcessor(IServiceProvider serviceProvider,
IBus bus, ILogger<OutBoxProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = serviceProvider.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();

                var outBoxMessages = await dbContext.OutBoxMessages
                    .Where(o => o.ProcessedOn == null)
                    .ToListAsync(stoppingToken);

                foreach (var message in outBoxMessages)
                {
                    var eventType = Type.GetType(message.Type);
                    if (eventType == null)
                    {
                        logger.LogWarning("Could not resole type: {Type}", message.Type);
                        continue;
                    }
                    var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                    if (eventMessage == null)
                    {
                        logger.LogWarning("Could not deserialize message content:{@payload}", message.Content);
                        continue;
                    }
                    await bus.Publish(eventMessage, stoppingToken);

                    message.SetProcessedOn();

                    logger.LogInformation("Successfully processed OutBox Message with Id:{MessageId}", message.Id);
                }
                await dbContext.SaveChangesAsync(stoppingToken);
            }catch (Exception exception)
            {
                logger.LogError("An error occured while processing outbox Message :{Detailed}",exception);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}