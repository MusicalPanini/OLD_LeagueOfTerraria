using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class DarksteelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Bar");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 30;
            item.height = 24;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(0, 1, 50, 0);
            item.uniqueStack = false;
            item.createTile = TileType<Tiles.DarksteelBarTile>();
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
            recipe.AddRecipeGroup("TerraLeague:IronGroup", 2);
            recipe.AddIngredient(ItemType<Ferrospike>(), 12);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}
