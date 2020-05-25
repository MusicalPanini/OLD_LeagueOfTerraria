using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Bullet_HextechChloroShotSplit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Chlorophyte Bolt");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 900;
            projectile.ranged = true;
            projectile.extraUpdates = 12;
        }

        public override void AI()
        {
            if (projectile.friendly && projectile.timeLeft < 700)
            {
                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 700f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];

                    if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal && npc.whoAmI != (int)projectile.ai[0])
                    {
                        Vector2 newMove = Main.npc[k].Center - projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance && Collision.CanHit(projectile.position, projectile.width*2, projectile.height*2, npc.position, npc.width, npc.height))
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    projectile.velocity = (10 * projectile.velocity + move) / 20f;
                    AdjustMagnitude(ref projectile.velocity);
                }
            }



            Lighting.AddLight(projectile.Left, 0.00f, 0.80f, 0.30f);

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            Dust dust = Dust.NewDustPerfect(projectile.position, 75, Vector2.Zero, 0, new Color(0,255,0), 1f);
            dust.noGravity = true;
            dust.alpha = 100;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] != target.whoAmI && !target.friendly)
                return true;
            else
                return false;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }
    }
}
