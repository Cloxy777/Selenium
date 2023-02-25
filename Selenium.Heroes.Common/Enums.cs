using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Heroes.Common;

public enum CardType
{
    Ore,
    Mana,
    Stacks
}

public enum ResourceType
{
    Mines,
    Ore,
    Monasteries,
    Mana,
    Barracks,
    Stacks,
    Tower,
    Wall
}

public enum DamageType
{
    Pure,
    Tower
}

public enum Side
{
    Player,
    Enemy
}

public enum PlayType
{
    Default,
    PlayAgain,
    DrawDiscardAndPlayAgain
}
