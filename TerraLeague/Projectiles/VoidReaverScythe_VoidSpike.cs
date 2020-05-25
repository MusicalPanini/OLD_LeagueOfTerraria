using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class VoidReaverScythe_VoidSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Spike");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 30;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 91, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 30);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 91, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 50, default(Color), 2f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
