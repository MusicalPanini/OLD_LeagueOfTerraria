using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class MagicArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Arrow");
            Tooltip.SetDefault("Creates a homing magic projectile on hit");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Arrow_MagicArrow>();
            item.damage = 9;
            item.width = 10;
            item.height = 28;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = AmmoID.Arrow;
            item.knockBack = 3f;
            item.value = 40;
            item.rare = ItemRarityID.Blue;
            item.ranged = true;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ManaBar>(), 1);
            recipe.AddIngredient(ItemID.WoodenArrow, 100);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
