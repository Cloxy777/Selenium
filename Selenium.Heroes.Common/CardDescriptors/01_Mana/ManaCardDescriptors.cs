using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.CardDescriptors;

public class JEWELLERY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "JEWELLERY",
            Description = "If tower < enemy tower, +2 to tower, else +1 to tower",
            Cost = 0,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 1, Side.Player)
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Tower < enemyManager.Player.Tower)
        {
            actualCardEffect.ResourceEffects = new List<ResourceEffect>
            {
                new ResourceEffect(ResourceType.Tower, 2, Side.Player)
            };
        }

        return actualCardEffect;
    }
}

public class RAINBOW_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "RAINBOW",
            Description = "+1 to all towers, you get 3 mana",
            Cost = 0,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 1, Side.Player),
            new ResourceEffect(ResourceType.Tower, 1, Side.Enemy),
            new ResourceEffect(ResourceType.Mana, 3, Side.Player)
        }
    };
}

public class QUARTZ_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "QUARTZ",
            Description = "+1 to tower, play again",
            Cost = 1,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 1, Side.Player)
        },
        PlayType = PlayType.PlayAgain
    };
}

public class AMETHYST_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "AMETHYST",
            Description = "+3 to tower",
            Cost = 2,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 3, Side.Player)
        }
    };
}

public class CRACK_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "CRACK",
            Description = "3 damage to enemy tower",
            Cost = 2,
            CardType = CardType.Mana
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 3, Side.Enemy)
        }
    };
}

public class PRISM_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "PRISM",
            Description = "Draw a card, discard a card, play again",
            Cost = 2,
            CardType = CardType.Mana
        },
        PlayType = PlayType.DrawDiscardAndPlayAgain
    };
}

public class SMOKY_QUARTZ_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SMOKY QUARTZ",
            Description = "1 damage to enemy tower, play again",
            Cost = 2,
            CardType = CardType.Mana
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 1, Side.Enemy)
        },
        PlayType = PlayType.PlayAgain
    };
}

public class POWER_EXPLOSION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "POWER EXPLOSION",
            Description = "5 damage to your tower, +2 to monastery",
            Cost = 3,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, 2, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 5, Side.Player)
        }
    };
}

public class RUBY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "RUBY",
            Description = "+5 to tower",
            Cost = 3,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 5, Side.Player)
        }
    };
}

public class SPELL_WEAVERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SPELL WEAVERS",
            Description = "+1 to monastery",
            Cost = 3,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, 1, Side.Player)
        }
    };
}

public class COLLABORATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "COLLABORATION",
            Description = "+7 to tower, -10 ore",
            Cost = 4,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 7, Side.Player),
            new ResourceEffect(ResourceType.Ore, -10, Side.Player),
        }
    };
}

public class ECLIPSE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ECLIPSE",
            Description = "+2 to tower, 2 damage to enemy tower",
            Cost = 4,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 2, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 2, Side.Enemy)
        }
    };
}

public class SPEAR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SPEAR",
            Description = "5 damage to enemy tower",
            Cost = 4,
            CardType = CardType.Mana
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 5, Side.Enemy)
        }
    };
}

public class DISSENSION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DISSENSION",
            Description = "7 damage to all towers, -1 to all monasteries",
            Cost = 5,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, -1, Side.Player),
            new ResourceEffect(ResourceType.Monasteries, -1, Side.Enemy),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 7, Side.Player),
            new DamageEffect(DamageType.Tower, 7, Side.Enemy),
        }
    };
}

public class INITIATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "INITIATION",
            Description = "+4 to tower, -3 stacks, 2 damage to enemy tower",
            Cost = 5,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 4, Side.Player),
            new ResourceEffect(ResourceType.Stacks, -3, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 2, Side.Enemy),
        }
    };
}

public class ORE_VEIN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ORE VEIN",
            Description = "+8 to tower",
            Cost = 5,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 8, Side.Player)
        }
    };
}

public class DIE_MOULD_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DIE MOULD",
            Description = "+1 to monastery, +3 to tower, +1 to enemy tower",
            Cost = 6,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, 1, Side.Player),
            new ResourceEffect(ResourceType.Tower, 3, Side.Player),
            new ResourceEffect(ResourceType.Tower, 1, Side.Enemy),
        }
    };
}

public class EMERALD_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "EMERALD",
            Description = "+8 to tower",
            Cost = 6,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 8, Side.Player),
        }
    };
}

public class HARMONY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "HARMONY",
            Description = "+1 to monastery, +3 to tower, +3 to wall",
            Cost = 7,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, 1, Side.Player),
            new ResourceEffect(ResourceType.Tower, 3, Side.Player),
            new ResourceEffect(ResourceType.Wall, 3, Side.Player),
        }
    };
}

public class MILD_STONE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "MILD STONE",
            Description = "+5 to tower, -6 ore to enemy",
            Cost = 7,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 5, Side.Player),
            new ResourceEffect(ResourceType.Ore, -6, Side.Enemy),
        }
    };
}

public class PARITY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "PARITY",
            Description = "All monasteries tie up for highest",
            Cost = 7,
            CardType = CardType.Mana
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        var difference = enemyManager.Player.Monasteries - playerManager.Player.Monasteries;
        if (difference != 0) 
        {
            var side = difference > 0 ? Side.Player : Side.Enemy;

            actualCardEffect.ResourceEffects = new List<ResourceEffect>
            {
                new ResourceEffect(ResourceType.Monasteries, difference, side)
            };
        }

        return actualCardEffect;
    }
}

public class FISSION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FISSION",
            Description = "-1 to monastery, 9 damage to enemy tower",
            Cost = 8,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, -1, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 9, Side.Enemy)
        }
    };
}

public class SOLIDIFICATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SOLIDIFICATION",
            Description = "+11 to tower, -6 to wall",
            Cost = 8,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 11, Side.Player),
            new ResourceEffect(ResourceType.Wall, -6, Side.Player)
        }
    };
}

public class WISDOM_PEARL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "WISDOM PEARL",
            Description = "+5 to tower, +1 to monastery",
            Cost = 9,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 5, Side.Player),
            new ResourceEffect(ResourceType.Monasteries, 1, Side.Player)
        }
    };
}

public class SAPPHIRE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SAPPHIRE",
            Description = "+11 to tower",
            Cost = 10,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 11, Side.Player)
        }
    };
}

public class LIGHTNING_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "LIGHTNING",
            Description = "If tower > enemy wall, 8 damage to enemy tower, else 8 damage to all",
            Cost = 11,
            CardType = CardType.Mana
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 8, Side.Player),
            new DamageEffect(DamageType.Pure, 8, Side.Enemy),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Tower > enemyManager.Player.Wall)
        {
            actualCardEffect.DamageEffects = new List<DamageEffect>
            {
                new DamageEffect(DamageType.Tower, 8, Side.Enemy),
            };
        }

        return actualCardEffect;
    }
}

public class CRYSTAL_SHIELD_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "CRYSTAL SHIELD",
            Description = "+8 to tower, +3 to wall",
            Cost = 12,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 8, Side.Player),
            new ResourceEffect(ResourceType.Wall, 3, Side.Player),
        }
    };
}

public class FIRE_RUBY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FIRE RUBY",
            Description = "+6 to tower, 4 damage to enemy tower",
            Cost = 13,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 6, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 4, Side.Enemy)
        }
    };
}

public class EMPATHY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "EMPATHY",
            Description = "+8 to tower, +1 to barracks",
            Cost = 14,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 8, Side.Player),
            new ResourceEffect(ResourceType.Barracks, 1, Side.Player),
        }
    };
}

public class SANCTUARY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SANCTUARY",
            Description = "+10 to tower, +5 to wall, +5 stacks",
            Cost = 15,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 10, Side.Player),
            new ResourceEffect(ResourceType.Wall, 5, Side.Player),
            new ResourceEffect(ResourceType.Stacks, 5, Side.Player),
        }
    };
}

public class DIAMOND_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DIAMOND",
            Description = "+15 to tower",
            Cost = 16,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 15, Side.Player),
        }
    };
}

public class SHINING_STONE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SHINING STONE",
            Description = "+12 to tower, 6 damage",
            Cost = 17,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 12, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 6, Side.Enemy)
        }
    };
}

public class MEDITATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "MEDITATION",
            Description = "+13 to tower, +6 stacks, +6 ore",
            Cost = 18,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 13, Side.Player),
            new ResourceEffect(ResourceType.Stacks, 6, Side.Player),
            new ResourceEffect(ResourceType.Ore, 6, Side.Player),
        }
    };
}

public class DRAGONS_EYE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DRAGON'S EYE",
            Description = "+20 to tower",
            Cost = 21,
            CardType = CardType.Mana
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 20, Side.Player)
        }
    };
}