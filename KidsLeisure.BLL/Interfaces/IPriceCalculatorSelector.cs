using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.BLL.Interfaces
{
    public interface IPriceCalculatorSelector
    {
        IPriceCalculatorStrategy SelectStrategy(ProgramType type);
    }
}