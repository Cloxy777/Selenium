using Selenium.Heroes.Common.Managers;
using Selenium.Heroes.Common.Models;

namespace Selenium.Heroes.Common.CardDescriptors;

public class DEFECTIVE_ORE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DEFECTIVE ORE",
            Description = "-8 ore to all players",
            Cost = 0,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            GetActualNegativeEffect(playerManager, enemyManager, -8, ResourceType.Ore, Side.Player),
            GetActualNegativeEffect(playerManager, enemyManager, -8, ResourceType.Ore, Side.Enemy),
        };

        return actualCardEffect;
    }
}

public class EARTHQUAKE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "EARTHQUAKE",
            Description = "-1 to all mines",
            Cost = 0,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            GetActualNegativeEffect(playerManager, enemyManager, -1, ResourceType.Mines, Side.Player),
            GetActualNegativeEffect(playerManager, enemyManager, -1, ResourceType.Mines, Side.Enemy),
        };

        return actualCardEffect;
    }
}

public class LUCKY_COIN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "LUCKY COIN",
            Description = "+2 ore, +2 mana, play again",
            Cost = 0,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Ore, 2, Side.Player),
            new ResourceEffect(ResourceType.Mana, 2, Side.Player)
        },
        PlayType = PlayType.PlayAgain
    };
}

public class MINE_COLLAPSE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "MINE COLLAPSE",
            Description = "-1 to mine, +10 to wall, +5 mana",
            Cost = 0,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            GetActualNegativeEffect(playerManager, enemyManager, -1, ResourceType.Mines, Side.Player),
            new ResourceEffect(ResourceType.Wall, 10, Side.Player),
            new ResourceEffect(ResourceType.Mana, 5, Side.Player)
        };

        return actualCardEffect;
    }
}

public class ABUNDANT_SOIL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ABUNDANT SOIL",
            Description = "+1 to wall, play again",
            Cost = 1,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, 1, Side.Player)
        },
        PlayType = PlayType.PlayAgain
    };
}

public class STONE_GARDEN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "STONE GARDEN",
            Description = "+1 to wall, +1 to tower, +2 stacks",
            Cost = 1,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mana, 1, Side.Player),
            new ResourceEffect(ResourceType.Tower, 1, Side.Player),
            new ResourceEffect(ResourceType.Stacks, 2, Side.Player),
        }
    };
}

public class INNOVATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "INNOVATION",
            Description = "+1 to all mines, +4 mana",
            Cost = 2,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, 1, Side.Player),
            new ResourceEffect(ResourceType.Mines, 1, Side.Enemy),
            new ResourceEffect(ResourceType.Mana, 4, Side.Player),
        }
    };
}

public class ORDINARY_WALL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ORDINARY WALL",
            Description = "+3 to wall",
            Cost = 2,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 3, Side.Player),
        }
    };
}

public class OVERTIME_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "OVERTIME",
            Description = "+5 to wall, -6 mana",
            Cost = 2,
            CardType = CardType.Ore
        }        
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 5, Side.Player),
            GetActualNegativeEffect(playerManager, enemyManager, -6, ResourceType.Mana, Side.Player),
        };

        return actualCardEffect;
    }
}

public class FOUNDATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FOUNDATION",
            Description = "If wall = 0, +6 to wall, else +3 to wall",
            Cost = 3,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 3, Side.Player)
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Wall == 0)
        {
            actualCardEffect.ResourceEffects = new List<ResourceEffect>
            {
                new ResourceEffect(ResourceType.Wall, 6, Side.Player)
            };
        }

        return actualCardEffect;
    }
}

public class LARGE_WALL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "LARGE WALL",
            Description = "+4 to wall",
            Cost = 3,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 4, Side.Player)
        }
    };
}

public class MINERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "MINERS",
            Description = "+1 to mine",
            Cost = 3,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, 1, Side.Player)
        }
    };
}

public class BIG_VEIN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "BIG VEIN",
            Description = "If mine < enemy mine, +2 to mine, else +1 to mine",
            Cost = 4,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, 1, Side.Player)
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        if (playerManager.Player.Mana > enemyManager.Player.Mines)
        {
            actualCardEffect.ResourceEffects = new List<ResourceEffect>
            {
                 new ResourceEffect(ResourceType.Mines, 2, Side.Player)
            };
        }

        return actualCardEffect;
    }
}

public class COLLAPSE_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "COLLAPSE",
            Description = "-1 to enemy mine",
            Cost = 4,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            GetActualNegativeEffect(playerManager, enemyManager, -1, ResourceType.Mines, Side.Enemy),
        };

        return actualCardEffect;
    }
}

public class FORTIFIED_WALL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FORTIFIED WALL",
            Description = "+6 to wall",
            Cost = 5,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 6, Side.Player)
        }
    };
}

public class STEAL_TECHNOLOGY_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "STEAL TECHNOLOGY",
            Description = "If mine < enemy mine, mine becomes equal to enemy mine",
            Cost = 5,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);
      
        if (playerManager.Player.Mines < enemyManager.Player.Mines)
        {
            var difference = enemyManager.Player.Mines - playerManager.Player.Mines;

            actualCardEffect.ResourceEffects = new List<ResourceEffect>
            {
                 new ResourceEffect(ResourceType.Mines, difference, Side.Player)
            };
        }

        return actualCardEffect;
    }
}

public class NEW_EQUIPMENT_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "NEW EQUIPMENT",
            Description = "+2 to mine",
            Cost = 6,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Mines, 2, Side.Player)
        }
    };
}

public class SUBSOIL_WATERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SUBSOIL WATERS",
            Description = "Player with lower wall gets -1 to barracks and 2 damage to tower",
            Cost = 6,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);
    
        if (playerManager.Player.Wall == enemyManager.Player.Wall)
        {
            actualCardEffect.ResourceEffects = new List<ResourceEffect>();
            return actualCardEffect;
        }

        var side = playerManager.Player.Wall > enemyManager.Player.Wall ? 
            Side.Enemy : 
            Side.Player;

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            GetActualNegativeEffect(playerManager, enemyManager, -1, ResourceType.Barracks, side),
            new ResourceEffect(ResourceType.Tower, -2, side), 
        };

        return actualCardEffect;
    }
}

public class DWARF_MINERS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DWARF MINERS",
            Description = "+4 to wall, +1 to mine",
            Cost = 7,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 4, Side.Player),
            new ResourceEffect(ResourceType.Mines, 1, Side.Player)
        }
    };
}

public class SLAVE_LABOR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SLAVE LABOR",
            Description = "+9 to wall, -5 stacks",
            Cost = 7,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 9, Side.Player),
            GetActualNegativeEffect(playerManager, enemyManager, -5, ResourceType.Stacks, Side.Player),
        };

        return actualCardEffect;
    }
}

public class TREMOR_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "TREMOR",
            Description = "All walls take 5 damage, play again",
            Cost = 7,
            CardType = CardType.Ore
        },
        PlayType = PlayType.PlayAgain
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
             GetActualNegativeEffect(playerManager, enemyManager, -5, ResourceType.Wall, Side.Player),
             GetActualNegativeEffect(playerManager, enemyManager, -5, ResourceType.Wall, Side.Enemy),
        };

        return actualCardEffect;
    }
}

public class GREAT_WALL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GREAT WALL",
            Description = "+8 to wall",
            Cost = 8,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 8, Side.Player)
        }
    };
}

public class SECRET_CAVERN_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SECRET CAVERN",
            Description = "+1 to monastery, play again",
            Cost = 8,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Monasteries, 1, Side.Player)
        },
        PlayType = PlayType.PlayAgain
    };
}

public class GALLERIES_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GALLERIES",
            Description = "+5 to wall, +1 to barracks",
            Cost = 9,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 5, Side.Player),
            new ResourceEffect(ResourceType.Barracks, 1, Side.Player)
        },
        PlayType = PlayType.PlayAgain
    };
}

public class MAGIC_MOUNT_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "MAGIC MOUNT",
            Description = "+7 to wall, +7 mana",
            Cost = 9,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 7, Side.Player),
            new ResourceEffect(ResourceType.Mana, 7, Side.Player),
        }
    };
}

public class BARRACKS_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "BARRACKS",
            Description = "+6 stacks, +6 to wall, if barracks < enemy barracks, +1 to barracks",
            Cost = 10,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Stacks, 6, Side.Player),
            new ResourceEffect(ResourceType.Wall, 6, Side.Player),
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);
        
        if (playerManager.Player.Barracks < enemyManager.Player.Barracks)
        {
            actualCardEffect.ResourceEffects = new List<ResourceEffect>
            {
                new ResourceEffect(ResourceType.Stacks, 6, Side.Player),
                new ResourceEffect(ResourceType.Wall, 6, Side.Player),
                new ResourceEffect(ResourceType.Barracks, 1, Side.Player)
            };
        }
        
        return actualCardEffect;
    }
}

public class SINGING_COAL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SINGING COAL",
            Description = "+6 to wall, +3 to tower",
            Cost = 11,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Tower, 3, Side.Player),
            new ResourceEffect(ResourceType.Wall, 6, Side.Player),
        }
    };
}

public class BASTION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "BASTION",
            Description = "+12 to wall",
            Cost = 13,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 12, Side.Player),
        }
    };
}

public class FORTIFICATION_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "FORTIFICATION",
            Description = "+7 to wall, 6 damage",
            Cost = 14,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 7, Side.Player),
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 6, Side.Enemy)
        }
    };
}

public class NEW_SUCCESSES_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "NEW SUCCESSES",
            Description = "+8 to wall, +5 to tower",
            Cost = 15,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 8, Side.Player),
            new ResourceEffect(ResourceType.Tower, 5, Side.Player),
        }
    };
}

public class GREATER_WALL_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "GREATER WALL",
            Description = "+15 to wall",
            Cost = 16,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 15, Side.Player)
        }
    };
}

public class SHIFT_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "SHIFT",
            Description = "Players switch walls",
            Cost = 17,
            CardType = CardType.Ore
        }
    };

    public override CardEffect GetActualCardEffect(PlayerManager playerManager, PlayerManager enemyManager, List<ICardDescriptor> cardDescriptors, ICardDescriptor cardDescriptor)
    {
        var actualCardEffect = base.GetActualCardEffect(playerManager, enemyManager, cardDescriptors, cardDescriptor);

        actualCardEffect.ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, -1 * playerManager.Player.Wall, Side.Player),
            new ResourceEffect(ResourceType.Wall, enemyManager.Player.Wall, Side.Player),
            new ResourceEffect(ResourceType.Wall, -1 * enemyManager.Player.Wall, Side.Enemy),
            new ResourceEffect(ResourceType.Wall, playerManager.Player.Wall, Side.Enemy),
        };

        return actualCardEffect;
    }
}

public class ROCKCASTER_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "ROCKCASTER",
            Description = "+6 to wall, 10 damage",
            Cost = 18,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 6, Side.Player)
        },
        DamageEffects = new List<DamageEffect>
        {
            new DamageEffect(DamageType.Pure, 10, Side.Enemy)
        }
    };
}

public class DRAGONS_HEART_CardDescriptor : CardDescriptor
{
    public override CardEffect BaseCardEffect { get; } = new CardEffect
    {
        Card = new Card
        {
            Header = "DRAGON'S HEART",
            Description = "+20 to wall, +8 to tower",
            Cost = 24,
            CardType = CardType.Ore
        },
        ResourceEffects = new List<ResourceEffect>
        {
            new ResourceEffect(ResourceType.Wall, 20, Side.Player),
            new ResourceEffect(ResourceType.Tower, 8, Side.Player)
        }
    };
}