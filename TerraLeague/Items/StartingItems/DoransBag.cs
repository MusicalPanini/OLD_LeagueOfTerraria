using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.StartingItems
{
    public class DoransBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doran's Crafting Kit");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.material = true;
            item.rare = ItemRarityID.LightRed;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(this, 1);
            recipe.SetResult(ItemType<DoransBlade>());
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(this, 1);
            recipe2.SetResult(ItemType<DoransRing>());
            recipe2.AddRecipe();

            ModRecipe recipe3 = new ModRecipe(mod);
            recipe3.AddIngredient(this, 1);
            recipe3.SetResult(ItemType<DoransShield>());
            recipe3.AddRecipe();
        }
    }
}
