using Autofac;
using Autofac.Extensions.DependencyInjection;
using FSADProjectBackend.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FSADProjectBackend.Autofac;

public static class AutofacRegister
{
    public static void Register(WebApplicationBuilder appBuilder)
    {
        appBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        appBuilder.Configuration.GetConnectionString("PostgresConnection");
        appBuilder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.Register(c => new PgDbContext(DbContextOptionsFactory.Get(appBuilder)))
                .InstancePerLifetimeScope();
        });
    }

    private class DbContextOptionsFactory
    {
        public static DbContextOptions<PgDbContext> Get(WebApplicationBuilder appBuilder)
        {
            var connectionString = appBuilder.Configuration.GetConnectionString("PostgresConnection");
            var builder = new DbContextOptionsBuilder<PgDbContext>();
            DbContextConfigurer.Configure(builder, connectionString);
            return builder.Options;
        }
    }

    private class DbContextConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<PgDbContext> builder, 
            string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }
    }
}