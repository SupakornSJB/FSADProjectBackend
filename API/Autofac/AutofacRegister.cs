using Autofac;
using Autofac.Extensions.DependencyInjection;
using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.Solution;
using FSADProjectBackend.Interfaces.Tag;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Services.Problem;
using FSADProjectBackend.Services.Solution;
using FSADProjectBackend.Services.Tag;
using FSADProjectBackend.Services.User;
using Microsoft.EntityFrameworkCore;

namespace FSADProjectBackend.Autofac;

public static class AutofacRegister
{
    public static void Register(WebApplicationBuilder appBuilder)
    {
        appBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        
        appBuilder.Configuration.GetConnectionString("PostgresConnection");
        appBuilder.Configuration.GetConnectionString("MongoConnection");
        
        appBuilder.Services.AddHttpContextAccessor();   
        appBuilder.Services.AddHttpClient();
        
        appBuilder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.Register(c => c.Resolve<IHttpClientFactory>().CreateClient()).As<HttpClient>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.Register(c => new PgDbContext(DbContextOptionsFactory.GetPg(appBuilder)))
                .InstancePerLifetimeScope();
            builder.Register(c => new MongoDbContext(DbContextOptionsFactory.GetMongo(appBuilder)))
                .InstancePerLifetimeScope();
            builder.Register(c => new UserInfoService(
                    c.Resolve<HttpClient>(), 
                    c.Resolve<IHttpContextAccessor>()
                ))
                .As<IUserInfoService>()
                .InstancePerLifetimeScope();
            builder.Register(c => new ProblemService(
                    c.Resolve<MongoDbContext>(), 
                    c.Resolve<IUserInfoService>(),
                    c.Resolve<ITagService>(),
                    c.Resolve<PgDbContext>()
                ))
                .As<IProblemService>()
                .InstancePerLifetimeScope();
            builder.Register(c => new ProblemCommentService(
                    c.Resolve<IProblemService>(),
                    c.Resolve<MongoDbContext>(),
                    c.Resolve<IUserInfoService>(),
                    c.Resolve<IProblemCommentUpvoteDownvoteService>() 
                ))
                .As<IProblemCommentService>()
                .InstancePerLifetimeScope();
            builder.Register(c => new ProblemCommentUpvoteDownvoteService(
                    c.Resolve<PgDbContext>(),
                    c.Resolve<IUserInfoService>(),
                    c.Resolve<IProblemService>()
                ))
                .As<IProblemCommentUpvoteDownvoteService>()
                .InstancePerLifetimeScope();

            builder.Register(c => new ProblemUpvoteDownvoteService(
                    c.Resolve<PgDbContext>(),
                    c.Resolve<IUserInfoService>(),
                    c.Resolve<IProblemService>()
                ))
                .As<IProblemUpvoteDownvoteService>()
                .InstancePerLifetimeScope();
            
            builder.Register(c => new TagService(
                    c.Resolve<PgDbContext>()))
                .As<ITagService>()
                .InstancePerLifetimeScope();
            
            builder.Register(c => new SolutionService(
                    c.Resolve<IProblemService>(),
                    c.Resolve<MongoDbContext>(),
                    c.Resolve<IUserInfoService>()
                ))
                .As<ISolutionService>()
                .InstancePerLifetimeScope();
        });
    }

    private class DbContextOptionsFactory
    {
        public static DbContextOptions<PgDbContext> GetPg(WebApplicationBuilder appBuilder) 
        {
            var connectionString = appBuilder.Configuration.GetConnectionString("PostgresConnection");
            var builder = new DbContextOptionsBuilder<PgDbContext>();
            DbContextConfigurer.Configure(builder, connectionString);
            return builder.Options;
        }
        
        public static DbContextOptions<MongoDbContext> GetMongo(WebApplicationBuilder appBuilder) 
        {
            var connectionString = appBuilder.Configuration.GetConnectionString("MongoConnection");
            var builder = new DbContextOptionsBuilder<MongoDbContext>();
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
        
        public static void Configure(
            DbContextOptionsBuilder<MongoDbContext> builder, 
            string connectionString)
        {
            builder.UseMongoDB(connectionString, "Database");
        }
    }
}