using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class DuskFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dusk Feather");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Projectiles.MagicalPlumage_DuskFeather>();
            item.damage = 9;
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = ItemType<RazorFeather>();
            item.notAmmo = false;
            item.knockBack = 1f;
            item.value = 15;
            item.ranged = true;
            item.rare = ItemRarityID.Blue;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 1);
            recipe.AddIngredient(ItemType<RazorFeather>(), 100);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
