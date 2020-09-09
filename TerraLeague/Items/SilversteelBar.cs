using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.Placeable;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class SilversteelBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver-Steel Bar");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 30;
            item.height = 24;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(0, 2, 75, 0);
            item.uniqueStack = false;
            item.createTile = TileType<SilversteelBarTile>();
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
            recipe.AddIngredient(ItemType<Petricite>(), 1);
            recipe.AddRecipeGroup("TerraLeague:SilverGroup", 2);
            recipe.AddIngredient(ItemID.HellstoneBar, 1);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
