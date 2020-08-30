using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class PrototypeHexCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype Hex Core");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 26;
            item.height = 16;
            item.rare = ItemRarityID.Green;
            item.value = 5000;
            item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst, 10);
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
