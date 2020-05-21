using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class BlackIceChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Ice Chunk");
            Tooltip.SetDefault("What was once pure is now cursed..");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 24;
            item.height = 22;
            item.rare = ItemRarityID.LightRed;
            item.value = 20000;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 2);
            recipe.AddIngredient(ItemID.PurpleIceBlock, 32);
            recipe.AddIngredient(ItemID.SoulofNight, 8);
            recipe.AddTile(TileID.IceMachine);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<TrueIceChunk>(), 2);
            recipe2.AddIngredient(ItemID.RedIceBlock, 32);
            recipe2.AddIngredient(ItemID.SoulofNight, 8);
            recipe2.AddTile(TileID.IceMachine);
            recipe2.SetResult(this, 2);
            recipe2.AddRecipe();
        }
    }
}
