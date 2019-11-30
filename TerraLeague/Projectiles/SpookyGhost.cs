using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class SpookyGhost : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spooky Ghost");
            ProjectileID.Sets.Homing[projectile.type] = true;
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().summonAbility = true;
        }

        public override void AI()
        {
            if(projectile.timeLeft < 1170)
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
                    if (npc.active && !npc.immortal && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage)
                    {
                        Vector2 newMove = Main.npc[k].Center - projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
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
                    projectile.velocity = (10 * projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref projectile.velocity);
                }
            }
            
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-90);
            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color));
            Main.dust[dustIndex].noGravity = true;
            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 6f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            crit = false;
            target.AddBuff(BuffType<Slowed>(), 300);
            projectile.Kill();
            base.OnHitNPC(target, damage, knockback, false);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 80, 0f, 0f, 100, default(Color), 0.7f);

            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            crit = false;
            if (target.GetModPlayer<PLAYERGLOBAL>().slowed)
            {
                crit = true;
                float multiplier = (Main.player[projectile.owner].rangedCrit + 75) * 0.01333f;
                float dam = damage * 0.5f * multiplier;
                damage = (int)dam;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;
            if (target.GetGlobalNPC<NPCsGLOBAL>().slowed)
            {
                crit = true;
                float multiplier = (Main.player[projectile.owner].rangedCrit + 75) * 0.01333f;
                float dam = damage * 0.5f * multiplier;
                damage = (int)dam;
            }
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 3;
                projectile.frameCounter = 0;
            }
        }

    }
}
