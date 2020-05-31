using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class NetherBladeofHorok_NullSphere : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Null Sphere");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 0.75f;
        }

        public override void AI()
        {
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
        }

        int GetTarget()
        {
            float distance = 500;
            NPC target = null;
            for (int k = 0; k < 200; k++)
            {
                NPC npcCheck = Main.npc[k];

                if (npcCheck.active && !npcCheck.friendly && npcCheck.lifeMax > 5 && !npcCheck.dontTakeDamage && !npcCheck.immortal)
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 15);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 8, 8, 112, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, 255, new Color(59, 0, 255), 2f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return true;
        }
    }
}
