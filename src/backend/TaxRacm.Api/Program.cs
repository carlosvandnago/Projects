using TaxRacm.Api.Extensions;
using TaxRacm.Api.Middleware;
using TaxRacm.Api.Seed;
using TaxRacm.Clients.Infrastructure.Persistence;
using TaxRacm.Controls.Infrastructure.Persistence;
using TaxRacm.Intelligence.Infrastructure.Persistence;
using TaxRacm.Risks.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TaxRACM API", Version = "v1" });
});

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddModules(builder.Configuration);

var claudeApiKey = builder.Configuration["Claude:ApiKey"];
if (string.IsNullOrWhiteSpace(claudeApiKey))
    claudeApiKey = Environment.GetEnvironmentVariable("CLAUDE_API_KEY") ?? string.Empty;
builder.Configuration["Claude:ApiKey"] = claudeApiKey;

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.GetRequiredService<ClientsDbContext>().Database.EnsureCreatedAsync();
    await services.GetRequiredService<RisksDbContext>().Database.EnsureCreatedAsync();
    await services.GetRequiredService<ControlsDbContext>().Database.EnsureCreatedAsync();
    await services.GetRequiredService<IntelligenceDbContext>().Database.EnsureCreatedAsync();
    await DataSeeder.SeedAsync(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
