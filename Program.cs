var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Access-Control-Allow-Origin");
    });
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8333);
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHealthChecks("/health");

app.MapReverseProxy();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
