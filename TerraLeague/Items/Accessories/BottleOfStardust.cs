using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class BottleOfStardust : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottle of Stardust");
            Tooltip.SetDefault("Enhances the effects of Targon's Blessings");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.rare = ItemRarityID.Expert;
            item.value = 100000;
            item.expertOnly = true;
            item.expert = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.bottleOfStarDustBuffer = true;
        }
    }
}
