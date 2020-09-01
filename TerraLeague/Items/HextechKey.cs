using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class HextechKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Key");
            Tooltip.SetDefault("Used to open Hextech Chests");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 16;
            item.height = 30;
            item.rare = ItemRarityID.LightRed;
            item.value = 30000;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HextechKeyFragment>(), 3);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
