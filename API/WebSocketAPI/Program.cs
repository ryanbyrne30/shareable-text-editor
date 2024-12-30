using WebSocketAPI.Config;
using WebSocketAPI.Processes.EstablishConnection;
using WebSocketAPI.Services.DocumentService;
using WebSocketAPI.Services.HttpRequestService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appConfig = new AppConfig{ DocumentApiUrl = ""};
builder.Configuration.GetSection("AppConfig").Bind(appConfig);
builder.Services.AddSingleton(appConfig);

builder.Services.AddTransient<EstablishConnectionService>();
builder.Services.AddTransient<HttpRequestService>();
builder.Services.AddTransient<DocumentService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapSwagger();
app.UseWebSockets();
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();
