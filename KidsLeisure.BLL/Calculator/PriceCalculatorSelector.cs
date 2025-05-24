using Autofac;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.BLL.Calculator
{
    public class PriceCalculatorSelector : IPriceCalculatorSelector
    {
        private readonly IComponentContext _context;

        public PriceCalculatorSelector(IComponentContext context)
        {
            _context = context;
        }

        public IPriceCalculatorStrategy SelectStrategy(ProgramType type)
        {
            return type switch
            {
                ProgramType.Custom => _context.Resolve<CustomProgramPriceCalculator>(),
                ProgramType.Birthday => _context.Resolve<DefaultPriceCalculator>(),
                _ => _context.Resolve<DefaultPriceCalculator>()
            };
        }
    }
}