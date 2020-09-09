using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items
{
    public class HextechCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Artifical Hex Core");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 20;
            item.height = 24;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.buyPrice(0, 0, 44 * 5, 20);
            item.uniqueStack = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrystalShard, 30);
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddIngredient(ItemID.MeteoriteBar, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
