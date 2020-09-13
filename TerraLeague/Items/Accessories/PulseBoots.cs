using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class PulseBoots : ModItem
    {

        /// Add cool extra jumps when they add the functionalliy
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pulse Boots");
            Tooltip.SetDefault("You can jump 6 extra times really high and sprint" +
                "\nYou are immune to fall damage");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(0, 18, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            //modPlayer.PulseJump = true;
            modPlayer.T4Boots = true;
            player.noFallDmg = true;
            player.jumpSpeedBoost += 4;
            player.jumpBoost = true;

            player.doubleJumpCloud = true;
            player.doubleJumpBlizzard = true;
            player.doubleJumpSandstorm = true;
            player.doubleJumpSail = true;
            player.doubleJumpUnicorn = true;
            player.doubleJumpFart = true;
        }
    }
}
