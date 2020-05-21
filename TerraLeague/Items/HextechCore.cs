using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class HextechCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Core");
            Tooltip.SetDefault("It emits a strong energy");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 24;
            item.height = 20;
            item.rare = ItemRarityID.LightRed;
            item.value = 20000;
            item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrystalShard, 30);
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
