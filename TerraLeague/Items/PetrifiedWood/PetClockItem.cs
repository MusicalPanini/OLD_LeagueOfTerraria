using TerraLeague.Tiles.PetFurniture;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    class PetClockItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood Clock");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = TileType<PetClock>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.anyIronBar = true;
            recipe.AddIngredient(ItemType<PetWood>(), 15);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
