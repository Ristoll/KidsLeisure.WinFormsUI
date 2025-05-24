using Autofac;
using Autofac.Extensions.DependencyInjection;
using KidsLeisure.BLL;
using KidsLeisure.BLL.Calculator;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.BLL.Repositories;
using KidsLeisure.BLL.Services;
using KidsLeisure.DAL.DBContext;
using KidsLeisure.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.UI;

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
                        new ZoneEntity { Name = "������ ����", Price = 200 },
                        new ZoneEntity { Name = "��������� ����", Price = 150 },
                        new ZoneEntity { Name = "���� �������", Price = 175 },
                        new ZoneEntity { Name = "�����-����", Price = 300 },
                        new ZoneEntity { Name = "��������� ����", Price = 290 }
                    );
                    context.SaveChanges();
                }

                if (!context.Attractions.Any())
                {
                    context.Attractions.AddRange(
                        new AttractionEntity { Name = "�����", Price = 150 },
                        new AttractionEntity { Name = "�������", Price = 200 },
                        new AttractionEntity { Name = "������ ������", Price = 250 },
                        new AttractionEntity { Name = "��������", Price = 100 },
                        new AttractionEntity { Name = "������ ��������", Price = 180 }
                    );
                    context.SaveChanges();
                }

                if (!context.Characters.Any())
                {
                    context.Characters.AddRange(
                        new CharacterEntity { Name = "̳�� ����", Price = 300 },
                        new CharacterEntity { Name = "��� �������", Price = 350 },
                        new CharacterEntity { Name = "���� � ����������", Price = 250 },
                        new CharacterEntity { Name = "ʳ� ��������", Price = 270 },
                        new CharacterEntity { Name = "����������", Price = 400 }
                    );
                    context.SaveChanges();
                }
            }

            var orderService = host.Services.GetRequiredService<IOrderService>();
            var customerService = host.Services.GetRequiredService<ICustomerService>();
            var mapper = host.Services.GetRequiredService<IMapper>();

            ApplicationConfiguration.Initialize();
            Application.Run(new AuthorizationWin(orderService, customerService, mapper));
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((context, services) =>
                {
                    var connectionString = System.Configuration.ConfigurationManager
                        .ConnectionStrings["DefaultConnection"].ConnectionString;

                    services.AddDbContext<LeisureDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    services.AddAutoMapper(typeof(MappingProfile).Assembly);
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    // ���������, ������, ������������
                    builder.RegisterGeneric(typeof(Repository<>))
                        .As(typeof(IRepository<>))
                        .InstancePerLifetimeScope();

                    builder.RegisterType<UnitOfWork>()
                        .As<IUnitOfWork>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<CustomerService>()
                        .As<ICustomerService>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<OrderService>()
                        .As<IOrderService>()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<CustomProgramPriceCalculator>()
                        .AsSelf()
                        .InstancePerLifetimeScope();

                    builder.RegisterType<DefaultPriceCalculator>()
                        .AsSelf()
                        .InstancePerLifetimeScope();

                    builder.Register<Func<ProgramType, IPriceCalculatorStrategy>>(ctx =>
                    {
                        var c = ctx.Resolve<IComponentContext>();
                        return programType =>
                        {
                            return programType switch
                            {
                                ProgramType.Custom => c.Resolve<CustomProgramPriceCalculator>(),
                                ProgramType.Birthday => c.Resolve<DefaultPriceCalculator>(),
                                _ => c.Resolve<DefaultPriceCalculator>()
                            };
                        };
                    });
                    builder.RegisterType<PriceCalculatorSelector>()
                        .As<IPriceCalculatorSelector>()
                        .InstancePerLifetimeScope();
                });
    }
}
