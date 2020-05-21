using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class BrassPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Brass Pickaxe");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 7;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 14;
            item.useAnimation = 17;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2f;
            item.value = 2000;
            item.rare = ItemRarityID.Blue;
            item.pick = 48;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 18);
            recipe.AddIngredient(ItemID.Wood, 4);
            recipe.anyWood = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
