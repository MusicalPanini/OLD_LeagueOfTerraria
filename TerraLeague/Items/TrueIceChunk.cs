using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class TrueIceChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Chunk");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 24;
            item.height = 22;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(0, 0, 7, 50);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceBlock, 32);
            recipe.AddTile(TileID.IceMachine);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}
