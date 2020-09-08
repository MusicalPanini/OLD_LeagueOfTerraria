using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class DarkSovereignsStaff_UnleashedPower : ModProjectile
	{
        int totalProj = 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unleashed Power");
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.alpha = 255;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)projectile.ai[0]];

            if (projectile.soundDelay == 0)
            {
                totalProj = Main.player[projectile.owner].ownedProjectileCounts[projectile.type];
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, 112, 0, 0, projectile.alpha);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            projectile.soundDelay = 100;

            if (!target.active)
            {
                projectile.Kill();
                return;
            }
            if (projectile.timeLeft == 1000 - (int)(45f * projectile.ai[1] / (float)totalProj))
            {
                projectile.friendly = true;
                projectile.alpha = 0;
                projectile.localAI[1] = 1;
                projectile.velocity = new Vector2(12, 0).RotatedBy(projectile.AngleTo(target.Center));
                projectile.extraUpdates = 1;
                Main.PlaySound(SoundID.Item1, projectile.Center);
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 45), projectile.Center);
            }
            if ((int)projectile.localAI[1] == 1)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, 112, 0, 0, projectile.alpha);
                dust.noGravity = true;
                dust.noLight = true;

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
            else
            {
                projectile.Center = Main.player[projectile.owner].MountedCenter + new Vector2(0, -48).RotatedBy(((MathHelper.TwoPi * projectile.ai[1]) / (float)totalProj));
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, 112, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, projectile.alpha);
                dust.noGravity = true;
                dust.noLight = true;
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
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
            base.PostDraw(spriteBatch, lightColor);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)projectile.ai[0])
                return base.CanHitNPC(target);
            else
                return false;

        }
    }
}
