using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.StartingItems
{
    public class DoransBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Blade");
            Tooltip.SetDefault("+4 melee and ranged damage" +
                "\nIncreases health by 10");
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
            player.GetModPlayer<PLAYERGLOBAL>().dblade = true;
            player.statLifeMax2 += 10;
            player.GetModPlayer<PLAYERGLOBAL>().meleeFlatDamage += 4;
            player.GetModPlayer<PLAYERGLOBAL>().rangedFlatDamage += 4;
        }
    }
}
