using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class BrassBuckShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brass Buckshot");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Bullet_BrassShot>();
            item.damage = 6;
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = AmmoID.Bullet;
            item.value = 5;
            item.knockBack = 4f;
            item.value = 5;
            item.ranged = true;
            item.rare = ItemRarityID.Blue;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 70);
            recipe.AddRecipe();
        }
    }
}
