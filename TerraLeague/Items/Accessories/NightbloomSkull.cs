using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class NightbloomSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nightbloom Skull");
            Tooltip.SetDefault("Increase max number of minions by 1" +
                "\nIncreases mana regeneration by 1" +
                "\nWhile below 50% life increase mana regeneration by 3");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
            item.rare = ItemRarityID.LightRed;
            item.value = 100000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.manaRegen += 1;
            player.maxMinions += 1;
            if (modPlayer.GetRealHeathWithoutShield(true)/2 > modPlayer.GetRealHeathWithoutShield())
            {
                modPlayer.manaRegen += 3;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Nightbloom>(), 1);
            recipe.AddIngredient(ItemType<PossessedSkull>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
