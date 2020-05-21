using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class BrassAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Brass Axe");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 7;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.width = 32;
            item.height = 28;
            item.useTime = 20;
            item.useAnimation = 23;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4.5f;
            item.value = 1600;
            item.rare = ItemRarityID.Blue;
            item.axe = 11;
            item.scale = 1.1f;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 9);
            recipe.AddIngredient(ItemID.Wood, 3);
            recipe.anyWood = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
