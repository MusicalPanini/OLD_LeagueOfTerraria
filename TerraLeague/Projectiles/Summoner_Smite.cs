using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Summoner_Smite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Smite");
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
            projectile.extraUpdates = 8;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if(projectile.soundDelay == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 88), projectile.Center);
                if (projectile.owner == Main.LocalPlayer.whoAmI)
                    projectile.netUpdate = true;
            }
            projectile.soundDelay = 100;

            if (!Main.npc[(int)projectile.ai[0]].active)
            {
                projectile.Kill();
            }
            else if (projectile.Hitbox.Intersects(Main.npc[(int)projectile.ai[0]].Hitbox))
            {
                projectile.velocity = Vector2.Zero;
                projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
            }
            else
            {
                projectile.timeLeft = 300;

                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }

                projectile.Center = new Vector2(Main.npc[(int)projectile.ai[0]].Center.X, projectile.Center.Y);

                Vector2 move = Vector2.Zero;

                NPC npc = Main.npc[(int)projectile.ai[0]];

                Vector2 newMove = npc.Center - projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 1f;
                AdjustMagnitude(ref projectile.velocity);

                for (int i = 0; i < 3; i++)
                {
                    Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                    int dustBoxWidth = projectile.width - 12;
                    int dustBoxHeight = projectile.height - 12;
                    Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 261, 0f, 0f, 100, new Color(255, 106, 0, 150), 1.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position.X -= projectile.velocity.X / 3f * (float)i;
                    dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
                }

                //for (int i = 0; i < 3; i++)
                //{
                //    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 106, 0, 150), 1f);
                //    dust.velocity *= 0f;
                //    dust.noGravity = true;
                //}
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 106, 0, 150), 1f);
                dust2.noGravity = true;
                dust2.velocity *= 3f;
                Lighting.AddLight(projectile.position, 1f, 1f, 0f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            

            if (target.life <= 0 || target.boss)
                Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += (int)(Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep * 0.15);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            Main.PlaySound(new LegacySoundStyle(2, 92), projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 106, 0, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
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
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
