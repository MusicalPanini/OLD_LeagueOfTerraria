using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class ManaBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Infused Bar");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 30;
            item.height = 24;
            item.uniqueStack = false;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(0, 2, 0, 0); 
            item.createTile = TileType<Tiles.ManaBarTile>();
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
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 4);
            recipe.AddIngredient(ItemType<ManaStone>(), 16);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
