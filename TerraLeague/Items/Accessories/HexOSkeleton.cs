using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Accessories
{
    public class HexOSkeleton : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex-O-Skeleton");
            Tooltip.SetDefault("Gain vision of Enemies and Traps" +
                "\nIncreases block placement & tool range by 3" +
                "\n50% increased mining speed" +
                "\nYou can jump 6 extra times really high and sprint" +
                "\nYou are immune to fall damage" +
                "\nPeriodically when you deal damage, cast 1 - 3 random magic spells that deal 50 magic damage.");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(0, 36, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.dangerSense = true;
            player.detectCreature = true;

            modPlayer.flashofBrilliance = true;

            player.blockRange += 3;
            player.pickSpeed += 0.5f;

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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<XrayGoggles>());
            recipe.AddIngredient(ModContent.ItemType<PulseBoots>());
            recipe.AddIngredient(ModContent.ItemType<FlashofBrilliance>());
            recipe.AddIngredient(ModContent.ItemType<ExtendoGloves>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
