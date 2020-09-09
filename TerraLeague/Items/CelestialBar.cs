using TerraLeague.Items.Placeable;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class CelestialBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Bar");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 30;
            item.height = 24;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.uniqueStack = false;
            item.createTile = TileType<CelestialBarTile>();
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TargonGraniteBlock>(), 16);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
