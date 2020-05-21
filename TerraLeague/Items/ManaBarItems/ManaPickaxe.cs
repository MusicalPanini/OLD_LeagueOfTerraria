using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.ManaBarItems
{
    public class ManaPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Infused Pickaxe");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.melee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3f;
            item.value = 12000;
            item.rare = ItemRarityID.Orange;
            item.pick = 70;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.tileBoost += 3;
            item.scale = 1.2f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ManaBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
