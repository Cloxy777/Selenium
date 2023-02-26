using Selenium.Heroes.Common.CardDescriptors;
using Selenium.Heroes.Common.Extensions;
using Selenium.Heroes.Common.Loaders;
using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common;

public class DrawCard
{
    public ICardDescriptor CardDescriptor { get; set; } = default!;

    public int Age { get; set; }
}

public class Deck
{
    public Deck()
    {  }

    public Deck(Deck deck) : this(deck.DrawCards)
    {  }

    public Deck(List<DrawCard> drawCards)
    {
        DrawCards = new List<DrawCard>(drawCards);
    }

    public List<DrawCard> DrawCards { get; } = new List<DrawCard>();

    public int MaxAge { get; set; } = 15;

    public List<ICardDescriptor> LeftCards => GetLeftCards();

    private List<ICardDescriptor> GetLeftCards()
    {
        return CardDescriptorsLoader.AllCardDescriptors.Where(card => !DrawCards.Any(x => x.CardDescriptor.Equals(card))).ToList();
    }

    public Deck Draw(ICardDescriptor cardDescriptor)
    {
        return Draw(new List<ICardDescriptor> { cardDescriptor });
    }

    public Deck Draw(List<ICardDescriptor> cards)
    {
        var deck = new Deck(this);

        // filter only existing cards.
        cards = Exists(cards).ToList();

        var drawCards = cards.Select(card => new DrawCard { Age = 0, CardDescriptor = card }).ToList();

        var otherDrawCards = Except(drawCards).ToList();

        deck.DrawCards.Clear();
        deck.DrawCards.AddRange(otherDrawCards);
        deck.DrawCards.AddRange(drawCards);

        return deck;
    }

    public Deck Reset()
    {
        return new Deck(DrawCards.Where(x => ++x.Age < MaxAge).ToList());
    }

    public IEnumerable<DrawCard> Except(List<DrawCard> drawCards)
    {
        return DrawCards.Where(x => !drawCards.Any(y => y.CardDescriptor.Equals(x.CardDescriptor)));
    }

    public static IEnumerable<ICardDescriptor> Exists(List<ICardDescriptor> cards)
    {
        var allCards = CardDescriptorsLoader.AllCardDescriptors;
        return cards.Where(x => allCards.Any(y => y.Equals(x)));
    }
}
