using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class EvolutionSet_Lazer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Death Ray");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 100;
            projectile.extraUpdates = 100;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0 && (int)projectile.ai[0] == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 15));
            }
            projectile.soundDelay = 100;

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = projectile.position;
                pos -= projectile.velocity * ((float)i * 0.25f);

                Dust dust = Dust.NewDustDirect(pos, 1, 1, 162, 0f, 0f, 0, default(Color), 1f);
                dust.position = pos;
                dust.position.X += (float)(projectile.width / 2);
                dust.position.Y += (float)(projectile.height / 2);
                dust.scale = (float)Main.rand.Next(70, 110) * 0.013f;
                dust.velocity *= 0.2f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 1, 1, 158, 0f, 0f, 0, default(Color),2f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 20f)
            {
                vector *= 8f / magnitude;
            }
        }
    }
}
