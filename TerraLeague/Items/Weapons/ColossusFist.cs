using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class ColossusFist : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Colossus Fist");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 10;
            item.value = 2400;
            item.rare = 2;

            item.noMelee = true;
            item.useStyle = 5; // Set the correct useStyle.
            item.useAnimation = 40; // Determines how long the animation lasts. 
            item.useTime = 40; // Determines how fast you can use this weapon (a lower value results in a faster use time).
            item.knockBack = 7F;
            item.damage = 20;
            item.scale = 1;
            item.noUseGraphic = false; // Do not use the item graphic when using the item (we just want the ball to spawn).
            item.UseSound = new Terraria.Audio.LegacySoundStyle(4, 3);
            item.shootSpeed = 8f;
            item.melee = true;
            item.autoReuse = true;
            item.shoot = ProjectileType<ColossusFistP>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.Y += 4;

            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 4);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Petricite>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
