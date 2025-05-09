using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.DBContext;
using KidsLeisure.BLL.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using KidsLeisure.UI;
using KidsLeisure.BLL.Calculator;
using KidsLeisure.BLL.Repositories;
using KidsLeisure.BLL.Services;
using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.WinFormsUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LeisureDbContext>();

                if (!context.Zones.Any())
                {
                    context.Zones.AddRange(
                        new ZoneEntity { Name = "Ігрова зона", Price = 200 },
                        new ZoneEntity { Name = "Спортивна зона", Price = 150 },
                        new ZoneEntity { Name = "Зона релаксу", Price = 175 },
                        new ZoneEntity { Name = "Квест-зона", Price = 300 },
                        new ZoneEntity { Name = "Тематична зона", Price = 290 }
                    );
                    context.SaveChanges();
                }

                if (!context.Attractions.Any())
                {
                    context.Attractions.AddRange(
                        new AttractionEntity { Name = "Батут", Price = 150 },
                        new AttractionEntity { Name = "Лабіринт", Price = 200 },
                        new AttractionEntity { Name = "Колесо огляду", Price = 250 },
                        new AttractionEntity { Name = "Гойдалки", Price = 100 },
                        new AttractionEntity { Name = "Зимова карусель", Price = 180 }
                    );
                    context.SaveChanges();
                }

                if (!context.Characters.Any())
                {
                    context.Characters.AddRange(
                        new CharacterEntity { Name = "Міккі Маус", Price = 300 },
                        new CharacterEntity { Name = "Поні Іскорка", Price = 350 },
                        new CharacterEntity { Name = "Соня з Ведмедиком", Price = 250 },
                        new CharacterEntity { Name = "Кіт Матроскін", Price = 270 },
                        new CharacterEntity { Name = "Снігуронька", Price = 400 }
                    );
                    context.SaveChanges();
                }
            }
            var orderService = host.Services.GetRequiredService<IOrderService>();
            var customerService = host.Services.GetRequiredService<ICustomerService>();
            ApplicationConfiguration.Initialize();
            Application.Run(new AuthorizationWin(orderService, customerService));
        }
        static IHostBuilder CreateHostBuilder() =>
    Host.CreateDefaultBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            services.AddDbContext<LeisureDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<CustomProgramPriceCalculator>();
            services.AddScoped<DefaultPriceCalculator>();

            services.AddScoped<Func<ProgramType, IPriceCalculatorStrategy>>(serviceProvider => strategyType =>
            {
                switch (strategyType)
                {
                    case ProgramType.Custom:
                        return serviceProvider.GetRequiredService<CustomProgramPriceCalculator>();
                    case ProgramType.Birthday:
                        return serviceProvider.GetRequiredService<DefaultPriceCalculator>();
                    default:
                        return serviceProvider.GetRequiredService<DefaultPriceCalculator>();
                }
            });

            services.AddScoped<PriceCalculatorSelector>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<Lazy<IOrderService>>(provider => new Lazy<IOrderService>(() => provider.GetRequiredService<IOrderService>()));
        });
    }
}