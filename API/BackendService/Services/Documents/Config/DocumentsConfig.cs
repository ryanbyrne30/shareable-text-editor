using BackendService.Services.Documents.UseCases;

namespace BackendService.Services.Documents.Config;

public class DocumentsConfig
{
    public static void Setup(IServiceCollection services)
    {
        services.AddTransient<CreateDocumentService>();
        services.AddTransient<GetDocumentByIdService>();
        services.AddTransient<UpdateDocumentService>();
    }
}