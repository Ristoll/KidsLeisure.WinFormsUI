using KidsLeisure.BLL.Calculator;
using KidsLeisure.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.BLL.Helpers
{
    public class PriceCalculatorSelector
    {
        private readonly IServiceProvider _serviceProvider;

        public PriceCalculatorSelector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPriceCalculatorStrategy SelectStrategy(ProgramType strategyType)
        {
            switch (strategyType)
            {
                case ProgramType.Custom:
                    return _serviceProvider.GetRequiredService<CustomProgramPriceCalculator>();
                case ProgramType.Birthday:
                    return _serviceProvider.GetRequiredService<DefaultPriceCalculator>();
                default:
                    return _serviceProvider.GetRequiredService<DefaultPriceCalculator>();
            }
        }
    }
}