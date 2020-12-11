using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.StartingItems
{
    public class DoransRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Ring");
            Tooltip.SetDefault("+2 magic and minion damage" +
                "\nIncreases health by 5" +
                "\nIncreases mana regeneration by 1" +
                "\nRestore 5 mana on kill");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 10000;
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().dring = true;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegen += 1;
            player.statLifeMax2 += 5;
            player.GetModPlayer<PLAYERGLOBAL>().magicFlatDamage += 2;
            player.GetModPlayer<PLAYERGLOBAL>().minionFlatDamage += 2;
        }
    }
}
