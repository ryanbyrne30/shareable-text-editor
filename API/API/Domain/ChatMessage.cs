namespace API.Domain;

public class ChatMessage
{
    public required string Message { get; set; }
    public required string Sender { get; set; }
    public DateTime Time { get; set; }
}