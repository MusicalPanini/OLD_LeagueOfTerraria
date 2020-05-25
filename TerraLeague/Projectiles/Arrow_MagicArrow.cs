using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Arrow_MagicArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Arrow");
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 1;
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 113, 0f, 0f, 200, default(Color), 0.5f);
            dust.noGravity = true;
            dust.noLight = true;
            Lighting.AddLight(projectile.position, 0.2f, 0f, 0.5f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal)
            {
                Projectile proj = Projectile.NewProjectileDirect(target.Center, new Vector2(0, 6).RotatedByRandom(MathHelper.TwoPi), ProjectileType<Item_Echo>(), projectile.damage/2, 0, projectile.owner, target.whoAmI);
            }

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X / 5, projectile.velocity.Y / 5, 100, new Color(0, 0, 255), 0.7f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

    }
}
