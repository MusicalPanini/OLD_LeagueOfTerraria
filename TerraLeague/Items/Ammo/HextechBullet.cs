using System;
using System.Collections.Generic;
using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class HextechBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Bullet");
            Tooltip.SetDefault("An experimental round that splits into 3 shots");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Bullet_HextechShot>();
            item.damage = 10;
            item.width = 8;
            item.height = 16;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = AmmoID.Bullet;
            item.knockBack = 2.5f;
            item.value = 40;
            item.ranged = true;
            item.rare = ItemRarityID.Orange;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HextechCore>(), 1);
            recipe.AddIngredient(ItemID.EmptyBullet, 100);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
