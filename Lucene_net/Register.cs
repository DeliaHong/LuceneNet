using Lucene.Infrastructure.pojo;
using Lucene.Infrastructure.Repositories;
using Lucene.Logic.Interfaces;
using Lucene.Logic.Options;
using Lucene.Logic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucene_net
{
    public static class Register
    {
        public static IConfiguration Configuration;
        
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(Configuration);
            serviceCollection.AddTransient<DapperContext>();
            serviceCollection.AddSingleton(p => 
            {
                var config = new IndexOption();
                config.IndexFileFolder = Configuration.GetValue<string>("IndexFileFolder");
                return config;
            });

            serviceCollection.AddScoped<NewsService>();
            serviceCollection.AddScoped<INewsRepository,NewsRepository>();
        }
    }
}
