using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.StartingItems
{
    public class DoransShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Shield");
            Tooltip.SetDefault("Increases defence, armor and resist by 2" +
                "\nIncreases health by 15");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 10000;
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().dsheild = true;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 2;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 2;
        }
    }
}
