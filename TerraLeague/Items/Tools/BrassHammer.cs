using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class BrassHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Brass Hammer");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5.5f;
            item.value = 1600;
            item.rare = ItemRarityID.Blue;
            item.hammer = 48;
            item.scale = 1.2f;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<BrassBar>(), 9);
            recipe.AddIngredient(ItemID.Wood, 3);
            recipe.anyWood = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
