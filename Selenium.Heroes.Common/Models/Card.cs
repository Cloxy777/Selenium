using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Selenium.Heroes.Common.Models;

public class Card
{
    public string Header { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Cost { get; set; }

    public decimal TransformedCost 
    { 
        get 
        { 
            return Cost / 150m * 10;
        }
    }

    public CardType CardType { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Card)
        {
            return Equals((Card)obj);
        }

        return false;
    }

    private bool Equals(Card card)
    {
        return Header.Equals(card.Header);
    }

    public override int GetHashCode()
    {
        return Header.GetHashCode();
    }

    public override string ToString()
    {
        return $"Header: {Header}. Description: {Description}.";
    }
}
