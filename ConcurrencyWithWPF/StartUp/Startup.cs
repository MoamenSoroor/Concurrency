using ConcurrencyWithWPF.SearchHandlers;
using ConcurrencyWithWPF.SearchHandlers.Base;
using ConcurrencyWithWPF.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyWithWPF
{
    public class Startup
    {
        public ServiceProvider ServiceProvider { get; private set; }
        private ServiceCollection services = new ServiceCollection();


        public Startup()
        {
        }

        public IServiceProvider Build()
        {
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            return ServiceProvider;
        }


        private void ConfigureServices(ServiceCollection services)
        {
            
            services.AddHttpClient<IClient, Client>(d => d.BaseAddress = new Uri("https://localhost:5001"));
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<DefaultSearchHandler>();
            services.AddScoped<WhenAllSearchHandler>();

            //TODO *** Here you can add your registeration
            // add any handler of the template like the previous 3 handlers.

            //services.AddScoped<ISearchHandler, DefaultSearchHandler>();
            services.AddScoped<ISearchHandlerFactory, SearchHandlerFactory>();


            services.AddSingleton<TemplateWindow>();
        }




    }
}
