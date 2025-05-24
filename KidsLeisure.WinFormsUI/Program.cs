using Autofac;
using AutoMapper;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.BLL.Services;
using KidsLeisure.BLL.Calculator;
using KidsLeisure.BLL.Repositories;
using KidsLeisure.DAL.DBContext;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using KidsLeisure.BLL;
using KidsLeisure.UI;

namespace KidsLeisure.WinFormsUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // �������� Autofac ����������
            var builder = new ContainerBuilder();

            // ��������� DbContext (������� � ������������� ����� ����������)
            var connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["DefaultConnection"].ConnectionString;

            builder.Register(ctx =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<LeisureDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new LeisureDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            // AutoMapper ������������ � ���������
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            // ��������� ����������, ������ � ������������
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

            // ��������� ����������
            var container = builder.Build();

            // ����������� WinForms
            ApplicationConfiguration.Initialize();

            // ������������ LifetimeScope ��� ��������� ������
            using (var scope = container.BeginLifetimeScope())
            {
                // ���������� �� ����������� ������, ���� �� ����
                SeedDatabase(scope.Resolve<LeisureDbContext>());

                var orderService = scope.Resolve<IOrderService>();
                var customerService = scope.Resolve<ICustomerService>();
                var mapperInstance = scope.Resolve<IMapper>();

                Application.Run(new AuthorizationWin(orderService, customerService, mapperInstance));
            }
        }

        private static void SeedDatabase(LeisureDbContext context)
        {
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
    }
}
