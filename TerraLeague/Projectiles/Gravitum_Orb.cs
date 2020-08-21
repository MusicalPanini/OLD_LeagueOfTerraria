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
	public class Gravitum_Orb : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravitum");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            AnimateProjectile();
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 71, 0f, 0f, 100, default(Color), 1.5f);
            dust.noGravity = true;
            dust.velocity *= 0.1f;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 15;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            if (projectile.timeLeft < 90 - 10)
            {
                if ((int)projectile.ai[0] == -1)
                {
                    projectile.ai[0] = GetTarget();
                }

                if (projectile.ai[0] != -1)
                {
                    NPC target = Main.npc[(int)projectile.ai[0]];

                    if (!target.active)
                    {
                        projectile.ai[0] = -1;
                        return;
                    }

                    float velocity = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
                    float num133 = projectile.localAI[0];
                    if (num133 == 0f)
                    {
                        projectile.localAI[0] = velocity;
                        num133 = velocity;
                    }
                    float num134 = projectile.position.X;
                    float num135 = projectile.position.Y;
                    bool flag3 = false;

                    float num143 = target.position.X + (float)(target.width / 2);
                    float num144 = target.position.Y + (float)(target.height / 2);
                    if (Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num143) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num144) < 1000f)
                    {
                        flag3 = true;
                        num134 = target.position.X + (float)(target.width / 2);
                        num135 = target.position.Y + (float)(target.height / 2);
                    }

                    if (flag3)
                    {
                        float num145 = num133;
                        Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                        float num146 = num134 - vector10.X;
                        float num147 = num135 - vector10.Y;
                        float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                        num148 = num145 / num148;
                        num146 *= num148;
                        num147 *= num148;
                        int num149 = 8;
                        projectile.velocity.X = (projectile.velocity.X * (float)(num149 - 1) + num146) / (float)num149;
                        projectile.velocity.Y = (projectile.velocity.Y * (float)(num149 - 1) + num147) / (float)num149;
                    }
                }
            }

            if (projectile.timeLeft == 3)
            {
                Prime();
            }
        }

        int GetTarget()
        {
            float distance = 200;
            NPC target = null;
            for (int k = 0; k < 200; k++)
            {
                NPC npcCheck = Main.npc[k];

                if (npcCheck.active && !npcCheck.friendly && npcCheck.lifeMax > 5 && !npcCheck.dontTakeDamage && !npcCheck.immortal && npcCheck.CanBeChasedBy())
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        distance = distanceTo;
                        target = npcCheck;
                    }
                }
            }

            return target == null ? -1 : target.whoAmI;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X + projectile.width / 4, projectile.position.Y + projectile.width / 4), projectile.width / 2, projectile.height / 2, 54, 0f, 0f, 100, new Color(0, 0, 0), 3f);
                dust.noGravity = true;
                dust.velocity = (dust.position - projectile.Center) * -0.1f;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X + projectile.width / 4, projectile.position.Y + projectile.width / 4), projectile.width / 2, projectile.height / 2, 71, 0f, 0f, 100, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.velocity = (dust.position - projectile.Center) * 0.1f;
                dust.fadeIn = 2.5f;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 71, 0f, 0f, 100, default(Color), 1f);
                dust.noGravity = true;
                dust.velocity = (dust.position - projectile.Center) * -0.05f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.width != 128)
                Prime();

            target.AddBuff(ModContent.BuffType<GravitumMark>(), 60 * 6);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 128;
            projectile.height = 128;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height - (texture.Height / 4) * 0.5f
                ),
                new Rectangle(0, (texture.Height / 4) * projectile.frame, texture.Width, texture.Height / 4),
                Color.White,
                projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
