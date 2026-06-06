using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaxRacm.Api.Infrastructure;
using TaxRacm.Clients.Domain.Repositories;
using TaxRacm.Clients.Infrastructure.Persistence;
using TaxRacm.Clients.Infrastructure.Repositories;
using TaxRacm.Controls.Domain.Repositories;
using TaxRacm.Controls.Infrastructure.Persistence;
using TaxRacm.Controls.Infrastructure.Repositories;
using TaxRacm.Intelligence.Domain.Interfaces;
using TaxRacm.Intelligence.Infrastructure.Claude;
using TaxRacm.Intelligence.Infrastructure.Persistence;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Infrastructure.Persistence;
using TaxRacm.Risks.Infrastructure.Repositories;
using TaxRacm.SharedKernel.Application;

using ClientsRef = TaxRacm.Clients.Application.AssemblyReference;
using ControlsRef = TaxRacm.Controls.Application.AssemblyReference;
using IntelligenceRef = TaxRacm.Intelligence.Application.AssemblyReference;
using RisksRef = TaxRacm.Risks.Application.AssemblyReference;

namespace TaxRacm.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ClientsDbContext>(o => o.UseSqlServer(connectionString));
        services.AddDbContext<RisksDbContext>(o => o.UseSqlServer(connectionString));
        services.AddDbContext<ControlsDbContext>(o => o.UseSqlServer(connectionString));
        services.AddDbContext<IntelligenceDbContext>(o => o.UseSqlServer(connectionString));

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IRiskBankRepository, RiskBankRepository>();
        services.AddScoped<IRacmRepository, RacmRepository>();
        services.AddScoped<IControlRepository, ControlRepository>();

        services.AddScoped<IUnitOfWork, CompositeUnitOfWork>();

        services.Configure<ClaudeOptions>(configuration.GetSection(ClaudeOptions.SectionName));
        services.AddHttpClient<IClaudeClient, ClaudeClient>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ClientsRef).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(RisksRef).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(ControlsRef).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(IntelligenceRef).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(ClientsRef).Assembly);
        services.AddValidatorsFromAssembly(typeof(RisksRef).Assembly);
        services.AddValidatorsFromAssembly(typeof(ControlsRef).Assembly);
        services.AddValidatorsFromAssembly(typeof(IntelligenceRef).Assembly);

        return services;
    }
}
