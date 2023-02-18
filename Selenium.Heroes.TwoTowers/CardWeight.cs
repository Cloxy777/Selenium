using Selenium.Heroes.Common.CardDescriptors;

namespace Selenium.Heroes.TwoTowers;

public class CardWeight
{
    public decimal Weight { get; set; }

    public ICardDescriptor CardDescriptor { get; set; } = default!;
}
