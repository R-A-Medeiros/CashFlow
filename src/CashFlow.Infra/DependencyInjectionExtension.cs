using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infra.DataAccess;
using CashFlow.Infra.DataAccess.Repositories;
using CashFlow.Infra.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infra;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddToken(services, configuration);
        AddRepositories(services);

        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
    }

    public static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config =>
            new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IExpenseUpdateOnlyRepository, ExpensesRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        var version = new Version(8, 0, 39);
        var serverVersion = new MySqlServerVersion(version);

        //optionsBuilder.UseMySql(connectionString, serverVersion);

        services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
