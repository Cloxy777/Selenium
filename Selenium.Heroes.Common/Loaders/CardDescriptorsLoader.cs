using Selenium.Heroes.Common.CardDescriptors;

namespace Selenium.Heroes.Common.Loaders;

public class CardDescriptorsLoader
{
    public static IReadOnlyList<ICardDescriptor> AllCardDescriptors { get; } = GetCardDescriptors();

    private static ICardDescriptor[] GetCardDescriptors()
    {
        var oreCardDescriptors = new ICardDescriptor[]
        {
            new DEFECTIVE_ORE_CardDescriptor(),
            new EARTHQUAKE_CardDescriptor(),
            new LUCKY_COIN_CardDescriptor(),
            new MINE_COLLAPSE_CardDescriptor(),
            new ABUNDANT_SOIL_CardDescriptor(),
            new STONE_GARDEN_CardDescriptor(),
            new INNOVATION_CardDescriptor(),
            new ORDINARY_WALL_CardDescriptor(),
            new OVERTIME_CardDescriptor(),
            new FOUNDATION_CardDescriptor(),
            new LARGE_WALL_CardDescriptor(),
            new MINERS_CardDescriptor(),
            new BIG_VEIN_CardDescriptor(),
            new COLLAPSE_CardDescriptor(),
            new FORTIFIED_WALL_CardDescriptor(),
            new STEAL_TECHNOLOGY_CardDescriptor(),
            new NEW_EQUIPMENT_CardDescriptor(),
            new SUBSOIL_WATERS_CardDescriptor(),
            new DWARF_MINERS_CardDescriptor(),
            new SLAVE_LABOR_CardDescriptor(),
            new TREMOR_CardDescriptor(),
            new GREAT_WALL_CardDescriptor(),
            new SECRET_CAVERN_CardDescriptor(),
            new GALLERIES_CardDescriptor(),
            new MAGIC_MOUNT_CardDescriptor(),
            new BARRACKS_CardDescriptor(),
            new SINGING_COAL_CardDescriptor(),
            new BASTION_CardDescriptor(),
            new FORTIFICATION_CardDescriptor(),
            new NEW_SUCCESSES_CardDescriptor(),
            new GREATER_WALL_CardDescriptor(),
            new SHIFT_CardDescriptor(),
            new ROCKCASTER_CardDescriptor(),
            new DRAGONS_HEART_CardDescriptor(),
        };

        var manaCardDescriptors = new ICardDescriptor[]
        {
            new  JEWELLERY_CardDescriptor(),
            new  RAINBOW_CardDescriptor(),
            new  QUARTZ_CardDescriptor(),
            new  AMETHYST_CardDescriptor(),
            new  CRACK_CardDescriptor(),
            new  PRISM_CardDescriptor(),
            new  SMOKY_QUARTZ_CardDescriptor(),
            new  POWER_EXPLOSION_CardDescriptor(),
            new  RUBY_CardDescriptor(),
            new  SPELL_WEAVERS_CardDescriptor(),
            new  COLLABORATION_CardDescriptor(),
            new  ECLIPSE_CardDescriptor(),
            new  SPEAR_CardDescriptor(),
            new  DISSENSION_CardDescriptor(),
            new  INITIATION_CardDescriptor(),
            new ORE_VEIN_CardDescriptor(),
            new  DIE_MOULD_CardDescriptor(),
            new  EMERALD_CardDescriptor(),
            new  HARMONY_CardDescriptor(),
            new  MILD_STONE_CardDescriptor(),
            new  PARITY_CardDescriptor(),
            new  FISSION_CardDescriptor(),
            new  SOLIDIFICATION_CardDescriptor(),
            new  WISDOM_PEARL_CardDescriptor(),
            new  SAPPHIRE_CardDescriptor(),
            new  LIGHTNING_CardDescriptor(),
            new  CRYSTAL_SHIELD_CardDescriptor(),
            new  FIRE_RUBY_CardDescriptor(),
            new  EMPATHY_CardDescriptor(),
            new  SANCTUARY_CardDescriptor(),
            new  DIAMOND_CardDescriptor(),
            new  SHINING_STONE_CardDescriptor(),
            new  MEDITATION_CardDescriptor(),
            new  DRAGONS_EYE_CardDescriptor(),
        };

        var stacksCardDescriptors = new ICardDescriptor[]
        {
            new  COW_RABIES_CardDescriptor(),
            new  FULL_MOON_CardDescriptor(),
            new  GOBLINS_CardDescriptor(),
            new  SPRITE_CardDescriptor(),
            new  DWARF_CardDescriptor(),
            new  ELVEN_SCOUTS_CardDescriptor(),
            new  SPEARMAN_CardDescriptor(),
            new  GOBLIN_ARMY_CardDescriptor(),
            new  MINOTAUR_CardDescriptor(),
            new  ORC_CardDescriptor(),
            new  BERSERKER_CardDescriptor(),
            new  GOBLIN_ARCHERS_CardDescriptor(),
            new  CRUSHER_CardDescriptor(),
            new  FAMILIAR_CardDescriptor(),
            new  GNOMES_CardDescriptor(),
            new  GHOST_FAIRY_CardDescriptor(),
            new  OGRE_CardDescriptor(),
            new  RABID_SHEEP_CardDescriptor(),
            new  TINY_SNAKES_CardDescriptor(),
            new  TROLL_INSTRUCTOR_CardDescriptor(),
            new  BEETLE_CardDescriptor(),
            new  TOWER_GREMLIN_CardDescriptor(),
            new  UNICORN_CardDescriptor(),
            new  WEREWOLF_CardDescriptor(),
            new  ELVEN_ARCHERS_CardDescriptor(),
            new  CAUSTIC_CLOUD_CardDescriptor(),
            new  STONE_DEVOURERS_CardDescriptor(),
            new  THIEF_CardDescriptor(),
            new  WARRIOR_CardDescriptor(),
            new  SUCCUBI_CardDescriptor(),
            new  STONE_GIANT_CardDescriptor(),
            new  VAMPIRE_CardDescriptor(),
            new  PEGASUS_RIDER_CardDescriptor(),
            new  DRAGON_CardDescriptor(),
        };

        return oreCardDescriptors.Union(manaCardDescriptors).Union(stacksCardDescriptors).ToArray();
    }
}
