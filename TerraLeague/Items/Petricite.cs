using TerraLeague.Items.PetrifiedWood;
using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class Petricite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricite Slab");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 30;
            item.height = 24;
            item.uniqueStack = false;
            item.rare = ItemRarityID.Blue;
            item.value = Item.buyPrice(0, 0, 15, 0);
            item.createTile = TileType<Tiles.PetriciteBarTile>();
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PetWood>(), 32);
            recipe.AddIngredient(ItemType<Limestone>(), 16);
            recipe.AddIngredient(ItemID.AshBlock, 16);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
