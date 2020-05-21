using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class DarksteelPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Pick-Axe");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.melee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 10;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3f;
            item.value = 22500;
            item.rare = ItemRarityID.Orange;
            item.pick = 90;
            item.axe = 25;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.scale = 1.2f;
            item.tileBoost -= 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
