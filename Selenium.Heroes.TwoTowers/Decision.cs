using Selenium.Heroes.Common.CardDescriptors;

namespace Selenium.Heroes.TwoTowers
{
    public class Decision
    {
        public ActionType ActionType { get; set; }

        public ICardDescriptor CardDescriptor { get; set; } = default!;
    }

    public enum ActionType
    {
        Play,
        Discard
    }
}