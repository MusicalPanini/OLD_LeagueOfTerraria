using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class CelestialStaff_StarcallRejuv : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Starcall Rejuvenation");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            Vector2 move = player.Center - projectile.Center;

            AdjustMagnitude(ref move);
            projectile.velocity = (projectile.velocity + move);
            AdjustMagnitude(ref projectile.velocity);

            if (projectile.Hitbox.Intersects(player.Hitbox))
            {
                HitPlayer(player);
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 0, new Color(248, 137, 89), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 0, new Color(237, 137, 164), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 10f)
            {
                vector *= 10f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public void HitPlayer(Player player)
        {
            projectile.netUpdate = true;
            player.AddBuff(BuffType<Rejuvenation>(), 300);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 263, 0, -2, 0, new Color(237, 137, 164));
                dust.noGravity = true;

                dust = Dust.NewDustDirect(player.position, player.width, player.height, 263, 0, -2, 0, new Color(248, 137, 89));
                dust.noGravity = true;
            }

            projectile.Kill();
        }



        public override void Kill(int timeLeft)
        {
        }
    }
}
