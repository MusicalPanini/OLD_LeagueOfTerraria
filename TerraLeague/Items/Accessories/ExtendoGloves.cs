using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class ExtendoGloves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extendo Gloves");
            Tooltip.SetDefault("Increases block placement & tool range by 3" +
                "\n50% increased mining speed");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 28;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(0, 18, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.blockRange += 3;
            player.pickSpeed += 0.5f;
        }
    }
}
