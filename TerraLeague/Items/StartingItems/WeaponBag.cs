using TerraLeague.Items.Weapons;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.StartingItems
{
    public class WeaponKit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic Weapon Kit");
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
            recipe.SetResult(ItemType<SunsteelBroadsword>());
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(this, 1);
            recipe2.SetResult(ItemType<ChemCrossbow>());
            recipe2.AddRecipe();

            ModRecipe recipe3 = new ModRecipe(mod);
            recipe3.AddIngredient(this, 1);
            recipe3.SetResult(ItemType<CrystalStaff>());
            recipe3.AddRecipe();

            ModRecipe recipe4 = new ModRecipe(mod);
            recipe4.AddIngredient(this, 1);
            recipe4.SetResult(ItemType<HextechWrench>());
            recipe4.AddRecipe();
        }
    }
}
