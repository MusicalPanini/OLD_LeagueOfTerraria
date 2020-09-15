using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Summoner_Syphon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Syphon");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 2;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 305;
            projectile.extraUpdates = 8;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if(projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 54, -0.5f);
            }
            projectile.soundDelay = 100;

            if (projectile.timeLeft == 301)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Main.npc[(int)projectile.ai[0]].position, Main.npc[(int)projectile.ai[0]].width, Main.npc[(int)projectile.ai[0]].height, 263, 0, 0, 0, new Color(255, 0, 0), 2);
                    dust2.noGravity = true;
                }
            }

            projectile.timeLeft = 300;

            if ((int)projectile.ai[1] == 0)
            {
                if (!Main.npc[(int)projectile.ai[0]].active)
                {
                    projectile.Kill();
                }
                else
                {
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
                    projectile.velocity = (10 * projectile.velocity + move) / 12f;
                    AdjustMagnitude(ref projectile.velocity);
                }
            }
            else
            {
                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;

                Player player = Main.player[projectile.owner];

                Vector2 newMove = player.Center - projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 15f;
                AdjustMagnitude(ref projectile.velocity);

                if (projectile.Hitbox.Intersects(player.Hitbox))
                {
                    player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(10, true);
                    projectile.Kill();
                }
            }

            Dust dust = Dust.NewDustPerfect(projectile.position, 263, Vector2.Zero, 0, new Color(255, 0, 110), 1f);
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;
            projectile.timeLeft = 302;
            projectile.ai[1] = 1;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, projectile.velocity.X, projectile.velocity.Y, 0, new Color(255, 0, 0), 2);
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI && (int)projectile.ai[1] == 0)
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

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
