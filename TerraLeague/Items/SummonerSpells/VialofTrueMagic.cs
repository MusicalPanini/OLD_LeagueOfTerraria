using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class VialofTrueMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vial of True Magic");
            
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.rare = ItemRarityID.Orange;
            item.width = 12;
            item.height = 30;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RawMagic>(), 10);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<RawMagic>(), 5);
            recipe2.AddIngredient(ItemID.Bottle);
            recipe2.AddTile(TileID.CrystalBall);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}
