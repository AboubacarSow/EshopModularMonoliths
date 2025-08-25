namespace Basket.Basket.Models;

public class OutBoxMessage:Entity<Guid>
{
    public string Type { get; private set; } = default!;
    public string Content { get; private set; } = default!;

    public DateTime OccuredOn{ get; private set; } = default!;
    public DateTime? ProcessedOn{get;private set;}=default!;

    public OutBoxMessage(Guid id,string type, string content, DateTime occuredOn)
    {
        Id=id;
        Type = type;
        Content = content;
        OccuredOn = occuredOn;
    }
    public void SetProcessedOn()
    {
        this.ProcessedOn = DateTime.UtcNow;
    }
}
