using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ExhaustProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exhaust");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 120;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if(projectile.timeLeft == 301)
                Main.PlaySound(new LegacySoundStyle(2, 88).WithPitchVariance(-0.3f), projectile.Center);

            if (!Main.npc[(int)projectile.ai[0]].active)
            {
                projectile.Kill();
            }
            else
            {
                projectile.timeLeft = 300;

                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;

                NPC npc = Main.npc[(int)projectile.ai[0]];

                Vector2 newMove = npc.Center - projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 20f;
                AdjustMagnitude(ref projectile.velocity);

                Dust dust = Dust.NewDustPerfect(projectile.position, 262, Vector2.Zero, 0, default(Color), 0.5f);
                dust.noGravity = true;
                dust.alpha = 100;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Exhausted>(), 240);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 262, 0, 0, 0, default(Color), 1f);
                dust.noGravity = true;
                dust.alpha = 100;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
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
