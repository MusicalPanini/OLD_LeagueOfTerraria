using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class HextechChloroBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech CH-300 Balistic");
            Tooltip.SetDefault("A experimental round that splits into 3 Chlorophite Bolts" +
                "\n...But not always");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Bullet_HextechChloroShot>();
            item.damage = 14;
            item.width = 8;
            item.height = 16;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = AmmoID.Bullet;
            item.knockBack = 2.5f;
            item.value = 50;
            item.ranged = true;
            item.rare = ItemRarityID.Orange;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<HextechCore>(), 1);
            recipe.AddIngredient(ItemID.EmptyBullet, 100);
            recipe.AddIngredient(ItemID.ChlorophyteBullet, 300);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
