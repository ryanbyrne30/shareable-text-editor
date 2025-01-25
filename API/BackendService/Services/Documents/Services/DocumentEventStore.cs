using System.Text;
using BackendService.Common.Repositories;

namespace BackendService.Services.Documents.Services;

public static class DocumentEventStore
{
    private static Dictionary<string, List<DocumentEvent>> Store = new();
    
    public static async Task ApplyEvent(byte[] bytes, string documentId, AppRepository repository)
    {
        var document = await repository.Documents.FindAsync(documentId);
        if (document == null) return;
        var docEvent = DocumentEvent.FromBytes(bytes);
        document.Content = docEvent.Apply(document.Content);
        repository.Documents.Update(document);
        await repository.SaveChangesAsync();
    }
        
    public static void AddEvent(string documentId, DocumentEvent documentEvent)
    {
        if (Store.TryGetValue(documentId, out var events))
        {
            events.Add(documentEvent);
        }
        else
        {
            Store.Add(documentId, [documentEvent]);
        }
    }
    
    public static List<DocumentEvent> GetEvents(string documentId)
    {
        return Store.TryGetValue(documentId, out var events) ? events : [];
    }
    
    public class DocumentEvent
    {
        private int Position { get; set; } = 0;
        private int Delete { get; set; } = 0;
        private string Insert { get; set; } = "";
        
        public static DocumentEvent FromBytes(byte[] bytes)
        {
            var position = BitConverter.ToInt32(bytes[0..4].Reverse().ToArray(), 0);
            var delete = BitConverter.ToInt32(bytes[4..8].Reverse().ToArray(), 0);
            var insertLength = BitConverter.ToInt32(bytes[8..12].Reverse().ToArray(), 0);
            var insert = insertLength > 0 ? Encoding.UTF8.GetString(bytes[12..]) : "";
            return new DocumentEvent
            {
                Position = position,
                Delete = delete,
                Insert = insert
            };
        }

        public string Apply(string text)
        {
            var sb = new StringBuilder(text);
            if (Delete > 0)
            {
                sb.Remove(Position, Delete);
            }
            if (Insert.Length > 0)
            {
                sb.Insert(Position, Insert);
            }
            return sb.ToString();
        }
    }
    
}