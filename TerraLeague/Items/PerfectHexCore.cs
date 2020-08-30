using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class PerfectHexCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perfect Hex Core");
            Tooltip.SetDefault("It emits a strong energy");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 26;
            item.height = 32;
            item.rare = ItemRarityID.Pink;
            item.value = 50000;
            item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HexCrystal>());
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
