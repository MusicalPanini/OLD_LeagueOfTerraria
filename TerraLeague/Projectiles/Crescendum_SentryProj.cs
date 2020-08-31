using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class Crescendum_SentryProj : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum Sentry");
        }

        Projectile sentry;

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 0;
            projectile.timeLeft = 1000;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 1000)
            {
                sentry = Main.projectile.FirstOrDefault(x => x.owner == projectile.owner && x.type == ModContent.ProjectileType<Crescendum_Sentry>());
                if (sentry == null)
                {
                    projectile.Kill();
                }
            }

            if (sentry == null)
            {
                projectile.Kill();
            }

            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 8;
                Main.PlaySound(SoundID.Item7, projectile.position);
            }

            if (!sentry.active)
            {
                sentry = Main.projectile.FirstOrDefault(x => x.owner == projectile.owner && x.type == ModContent.ProjectileType<Crescendum_Sentry>());
                if (sentry == null)
                {
                    projectile.Kill();
                }
            }

            if (projectile.ai[0] == 0f)
            {
                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= 25f)
                {
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                projectile.tileCollide = false;
                float num51 = 12;
                float num52 = 0.4f;

                Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num53 = sentry.position.X + (float)(sentry.width / 2) - vector3.X;
                float num54 = sentry.position.Y + (float)(sentry.height / 2) - vector3.Y;
                float num55 = (float)Math.Sqrt((double)(num53 * num53 + num54 * num54));
                if (num55 > 3000f)
                {
                    projectile.Kill();
                }
                num55 = num51 / num55;
                num53 *= num55;
                num54 *= num55;

                {
                    if (projectile.velocity.X < num53)
                    {
                        projectile.velocity.X += num52;
                        if (projectile.velocity.X < 0f && num53 > 0f)
                        {
                            projectile.velocity.X += num52;
                        }
                    }
                    else if (projectile.velocity.X > num53)
                    {
                        projectile.velocity.X -= num52;
                        if (projectile.velocity.X > 0f && num53 < 0f)
                        {
                            projectile.velocity.X -= num52;
                        }
                    }
                    if (projectile.velocity.Y < num54)
                    {
                        projectile.velocity.Y += num52;
                        if (projectile.velocity.Y < 0f && num54 > 0f)
                        {
                            projectile.velocity.Y += num52;
                        }
                    }
                    else if (projectile.velocity.Y > num54)
                    {
                        projectile.velocity.Y -= num52;
                        if (projectile.velocity.Y > 0f && num54 < 0f)
                        {
                            projectile.velocity.Y -= num52;
                        }
                    }
                }
                if (Main.myPlayer == projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    Rectangle value2 = new Rectangle((int)sentry.position.X, (int)sentry.position.Y, sentry.width, sentry.height);
                    if (rectangle.Intersects(value2))
                    {
                        projectile.Kill();
                    }
                }
            }
            projectile.rotation += 0.6f * (float)projectile.direction;
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 91, 0, 0, 0, default(Color), 0.5f);
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if ((int)projectile.ai[0] == 0)
            {
                projectile.velocity = -projectile.velocity;
                projectile.netUpdate = true;
                projectile.ai[0] = 1;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            projectile.ai[0] = 1;
            projectile.velocity = -projectile.velocity;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }
    }
}
