using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class DarksteelBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Bullet");
            Tooltip.SetDefault("Struck enemies will start to 'Hemorrhage'" +
                "\nHemorrage stacks up to 5 times");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Bullet_DarksteelShot>();
            item.damage = 7;
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.value = 7;
            item.consumable = true;
            item.ammo = AmmoID.Bullet;
            item.knockBack = 1f;
            item.value = 10;
            item.ranged = true;
            item.rare = ItemRarityID.Blue;


            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 1);
            recipe.AddIngredient(ItemID.MusketBall, 150);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }
    }
}
