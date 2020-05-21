using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class Nightbloom : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nightbloom");
            Tooltip.SetDefault("Increases mana regeneration by 1" +
                "\nWhile below 50% life increase mana regeneration by 3");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.rare = ItemRarityID.Orange;
            item.value = 27000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.manaRegen += 1;

            if (modPlayer.GetRealHeathWithoutShield(true)/2 > modPlayer.GetRealHeathWithoutShield())
            {
                modPlayer.manaRegen += 3;
            }
        }
    }
}
