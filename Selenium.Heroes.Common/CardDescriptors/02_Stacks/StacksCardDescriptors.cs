using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.CardDescriptors;

public class COW_RABIES_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "COW RABIES",
            Description = "-6 stacks to all players",
            Cost = 0,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Stacks, -6, Side.Player),
            new ResourceEffect(ResourceType.Stacks, -6, Side.Enemy),
        }
    };
}

public class FULL_MOON_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FULL MOON",
            Description = "+1 barracks to all players, +3 stacks",
            Cost = 0,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Barracks, 1, Side.Player),
            new ResourceEffect(ResourceType.Barracks, 1, Side.Enemy),
            new ResourceEffect(ResourceType.Stacks, 3, Side.Player),
        }
    };
}

public class GOBLINS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GOBLINS",
            Description = "4 damage, -3 mana",
            Cost = 1,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, -3, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 4, Side.Enemy),
        }
    };
}

public class SPRITE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SPRITE",
            Description = "2 damage, play again",
            Cost = 1,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 2, Side.Enemy),
        },
        PlayType = PlayType.PlayAgain
    };
}

public class DWARF_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DWARF",
            Description = "3 damage, +1 mana",
            Cost = 2,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, 1, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 3, Side.Enemy),
        },
    };
}

public class ELVEN_SCOUTS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ELVEN SCOUTS",
            Description = "Draw a card, discard a card, play again",
            Cost = 2,
            CardType = CardType.Stacks
        },
        PlayType = PlayType.DrawDiscardAndPlayAgain
    };
}

public class SPEARMAN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SPEARMAN",
            Description = "If wall > enemy wall, 3 damage, else 2 damage",
            Cost = 2,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 2, Side.Enemy),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Wall > enemyManager.Player.Wall)
        {
            actualCardEffect.DamageEffects = new List<DamageEffect>
            {
                new DamageEffect(DamageType.Pure, 3, Side.Enemy),
            };
        }

        return actualCardEffect;
    }
}

public class GOBLIN_ARMY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GOBLIN ARMY",
            Description = "6 damage, 3 damage to you",
            Cost = 3,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 3, Side.Player),
            new DamageEffect(DamageType.Pure, 6, Side.Enemy),
        }
    };
}

public class MINOTAUR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "MINOTAUR",
            Description = "+1 to barracks",
            Cost = 3,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Barracks, 1, Side.Player),
        }
    };
}

public class ORC_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ORC",
            Description = "5 damage",
            Cost = 3,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 5, Side.Enemy),
        }
    };
}

public class BERSERKER_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "BERSERKER",
            Description = "8 damage, 3 damage to your tower",
            Cost = 4,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {

            new DamageEffect(DamageType.Tower, 3, Side.Player),
            new DamageEffect(DamageType.Pure, 8, Side.Enemy),
        }
    };
}

public class GOBLIN_ARCHERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GOBLIN ARCHERS",
            Description = "3 damage to enemy tower, 1 damage to you",
            Cost = 4,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {

            new DamageEffect(DamageType.Tower, 3, Side.Enemy),
            new DamageEffect(DamageType.Pure, 1, Side.Player),
        }
    };
}

public class CRUSHER_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "CRUSHER",
            Description = "6 damage",
            Cost = 5,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 6, Side.Enemy),
        }
    };
}

public class FAMILIAR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FAMILIAR",
            Description = "6 damage, -5 ore, mana and stacks to all players",
            Cost = 5,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Ore, -5, Side.Player),
            new ResourceEffect(ResourceType.Mana, -5, Side.Player),
            new ResourceEffect(ResourceType.Stacks, -5, Side.Player),
            new ResourceEffect(ResourceType.Ore, -5, Side.Enemy),
            new ResourceEffect(ResourceType.Mana, -5, Side.Enemy),
            new ResourceEffect(ResourceType.Stacks, -5, Side.Enemy),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 6, Side.Enemy),
        }
    };
}

public class GNOMES_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GNOMES",
            Description = "4 damage, +3 to wall",
            Cost = 5,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 3, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 4, Side.Enemy),
        }
    };
}

public class GHOST_FAIRY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GHOST FAIRY",
            Description = "2 damage to enemy tower, play again",
            Cost = 6,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 2, Side.Enemy),
        },
        PlayType = PlayType.PlayAgain
    };
}

public class OGRE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "OGRE",
            Description = "7 damage",
            Cost = 6,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 7, Side.Enemy),
        }
    };
}

public class RABID_SHEEP_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "RABID SHEEP",
            Description = "6 damage, -3 stacks to enemy",
            Cost = 6,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Stacks, -3, Side.Enemy),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 3, Side.Enemy),
        }
    };
}

public class TINY_SNAKES_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "TINY SNAKES",
            Description = "4 damage to enemy tower",
            Cost = 6,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 4, Side.Enemy),
        }
    };
}

public class TROLL_INSTRUCTOR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "TROLL INSTRUCTOR",
            Description = "+2 to barracks",
            Cost = 7,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Barracks, 2, Side.Player),
        }
    };
}

public class BEETLE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "BEETLE",
            Description = "If enemy wall = 0, 10 damage, else 6 damage",
            Cost = 8,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 6, Side.Enemy),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (enemyManager.Player.Wall == 0)
        {
            actualCardEffect.DamageEffects = new List<DamageEffect>
            {
                new DamageEffect(DamageType.Pure, 10, Side.Enemy),
            };
        }

        return actualCardEffect;
    }
}

public class TOWER_GREMLIN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "TOWER GREMLIN",
            Description = "2 damage, +4 to wall, +2 to tower",
            Cost = 8,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 4, Side.Player),
            new ResourceEffect(ResourceType.Tower, 2, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 2, Side.Enemy),
        }
    };
}

public class UNICORN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "UNICORN",
            Description = "If monastery > enemy monastery, 12 damage, else 8 damage",
            Cost = 9,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 8, Side.Enemy),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Monasteries > enemyManager.Player.Monasteries)
        {
            actualCardEffect.DamageEffects = new List<DamageEffect>
            {
                new DamageEffect(DamageType.Pure, 12, Side.Enemy),
            };
        }

        return actualCardEffect;
    }
}

public class WEREWOLF_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "WEREWOLF",
            Description = "9 damage",
            Cost = 9,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 9, Side.Enemy),
        }
    };
}

public class ELVEN_ARCHERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ELVEN ARCHERS",
            Description = "If wall > enemy wall, 6 damage to enemy tower, else 6 damage",
            Cost = 10,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 6, Side.Enemy),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Wall > enemyManager.Player.Wall)
        {
            actualCardEffect.DamageEffects = new List<DamageEffect>
            {
                new DamageEffect(DamageType.Tower, 6, Side.Enemy),
            };
        }

        return actualCardEffect;
    }
}

public class CAUSTIC_CLOUD_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "CAUSTIC CLOUD",
            Description = "If enemy wall > 10, 10 damage, else 7 damage",
            Cost = 11,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 7, Side.Enemy),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (enemyManager.Player.Wall > 10)
        {
            actualCardEffect.DamageEffects = new List<DamageEffect>
            {
                new DamageEffect(DamageType.Tower, 10, Side.Enemy),
            };
        }

        return actualCardEffect;
    }
}

public class STONE_DEVOURERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "STONE DEVOURERS",
            Description = "8 damage, -1 to enemy mine",
            Cost = 11,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, -1, Side.Enemy),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 8, Side.Enemy),
        }
    };
}

public class THIEF_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "THIEF",
            Description = "-10 mana to enemy, -5 ore to enemy, you get half that much",
            Cost = 12,
            CardType = CardType.Stacks
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        var enemyManaDraw = enemyManager.Player.Mana % 10;
        var enemyOreDraw = enemyManager.Player.Ore % 5;
        var playerMana = enemyManaDraw / 2;
        var playerOre = enemyOreDraw / 2;

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, enemyManaDraw * -1, Side.Enemy),
            new ResourceEffect(ResourceType.Ore, enemyOreDraw * -1, Side.Enemy),
            new ResourceEffect(ResourceType.Mana, playerMana, Side.Player),
            new ResourceEffect(ResourceType.Ore, playerOre, Side.Player),
        };

        return actualCardEffect;
    }
}

public class WARRIOR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "WARRIOR",
            Description = "13 damage, -3 mana",
            Cost = 13,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, -3, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 13, Side.Enemy)
        }
    };
}

public class SUCCUBI_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SUCCUBI",
            Description = "5 damage to enemy tower, -8 stacks to enemy",
            Cost = 14,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Stacks, -8, Side.Enemy)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 5, Side.Enemy)
        }
    };
}

public class STONE_GIANT_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "STONE GIANT",
            Description = "10 damage, +4 to wall",
            Cost = 15,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 4, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 10, Side.Enemy)
        }
    };
}

public class VAMPIRE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "VAMPIRE",
            Description = "10 damage, -5 stacks to enemy, -1 to enemy barracks",
            Cost = 17,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Stacks, -5, Side.Enemy),
            new ResourceEffect(ResourceType.Barracks, -1, Side.Enemy),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 10, Side.Enemy)
        }
    };
}

public class PEGASUS_RIDER_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "PEGASUS RIDER",
            Description = "12 damage to enemy tower",
            Cost = 18,
            CardType = CardType.Stacks
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Tower, 12, Side.Enemy)
        }
    };
}

public class DRAGON_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DRAGON",
            Description = "20 damage, -10 mana to enemy, -1 to enemy barracks",
            Cost = 25,
            CardType = CardType.Stacks
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, -10, Side.Enemy),
            new ResourceEffect(ResourceType.Barracks, -1, Side.Enemy),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 20, Side.Enemy)
        }
    };
}