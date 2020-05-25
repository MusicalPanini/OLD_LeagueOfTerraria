using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class OrbofDeception_Orb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            DisplayName.SetDefault("Orb of Deception");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.scale = 1f;
            projectile.tileCollide = false;
            projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            AnimateProjectile();

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X, projectile.position.Y);
                int dustBoxWidth = projectile.width;
                int dustBoxHeight = projectile.height;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 263, 0f, 0f, 100, new Color(229, 242, 249), 1.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }


            player.itemTime = 5;
            if (projectile.timeLeft > 210)
            {
                if (projectile.position.X + (float)(projectile.width / 2) > player.position.X + (float)(player.width / 2))
                {
                    player.ChangeDir(1);
                }
                else
                {
                    player.ChangeDir(-1);
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
                float returnSpeed = 16f;
                float acceleration = 0.5f;

                float xDif = player.Center.X - projectile.Center.X;
                float yDif = player.Center.Y - projectile.Center.Y;
                float distance = projectile.Distance(player.Center);

                if (distance > 3000f)
                {
                    projectile.Kill();
                }
                distance = returnSpeed / distance;
                xDif *= distance;
                yDif *= distance;

                if (projectile.velocity.X < xDif)
                {
                    projectile.velocity.X = projectile.velocity.X + acceleration;
                    if (projectile.velocity.X < 0f && xDif > 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X + acceleration;
                    }
                }
                else if (projectile.velocity.X > xDif)
                {
                    projectile.velocity.X = projectile.velocity.X - acceleration;
                    if (projectile.velocity.X > 0f && xDif < 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X - acceleration;
                    }
                }

                if (projectile.velocity.Y < yDif)
                {
                    projectile.velocity.Y = projectile.velocity.Y + acceleration;
                    if (projectile.velocity.Y < 0f && yDif > 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y + acceleration;
                    }
                }
                else if (projectile.velocity.Y > yDif)
                {
                    projectile.velocity.Y = projectile.velocity.Y - acceleration;
                    if (projectile.velocity.Y > 0f && yDif < 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - acceleration;
                    }
                }

                if (Main.myPlayer == projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (rectangle.Intersects(value2))
                    {
                        projectile.Kill();
                    }
                }

                if (projectile.ai[0] == 0f)
                {
                    Vector2 velocity = projectile.velocity;
                    velocity.Normalize();
                    return;
                }
                Vector2 vector4 = projectile.Center - player.Center;
                vector4.Normalize();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 8, 8, 16, 0f, 0f, 0, new Color(255, 255, 255), 1f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.ai[0] == 1f)
                crit = true;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
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
                new Rectangle(0, (texture.Height / 4) * projectile.frame, texture.Width, texture.Height/4),
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
