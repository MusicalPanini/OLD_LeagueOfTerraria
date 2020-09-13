using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class XrayGoggles : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("X-ray Goggles");
            Tooltip.SetDefault("Gain vision of Enemies and Traps");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(0, 18, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.dangerSense = true;
            player.detectCreature = true;
        }
    }
}
