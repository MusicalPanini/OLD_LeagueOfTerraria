using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class RazorFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Razor Feather");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 1f;
            item.shoot = ProjectileType<MagicalPlumage_DeadlyFeather>();
            item.damage = 6;
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = ItemType<RazorFeather>();
            item.notAmmo = false;
            item.knockBack = 1f;
            item.value = 10;
            item.ranged = true;
            item.rare = ItemRarityID.Blue;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<ManaBar>(), 1);
            recipe.AddIngredient(ItemID.Feather, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
