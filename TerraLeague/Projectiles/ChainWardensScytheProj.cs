using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ChainWardensScytheProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chain Warden's Scythe");
        }

        public override void SetDefaults()
        {
            //projectile.aiStyle = 15;
            projectile.friendly = true;
            projectile.alpha = 0;
            projectile.width = 50;
            projectile.height = 50;
            projectile.melee = true;
            projectile.penetrate = -1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            // So set the correct path here to load the chain texture. 'YourModName' is of course the name of your mod.
            // Then into the Projectiles folder and take the texture that is called 'CustomFlailBall_Chain'.
            Texture2D texture = mod.GetTexture("Projectiles/ChainWardensScytheChain");
            Vector2 position;
            if (projectile.spriteDirection == 1)
                position = new Vector2(projectile.position.X + 8, projectile.position.Y + 7).RotatedBy(projectile.rotation, projectile.Center);
            else
                position = new Vector2(projectile.position.X + projectile.width - 8, projectile.position.Y + 7).RotatedBy(projectile.rotation, projectile.Center);

                //position.X += 4;
            //position;

            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;

            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.itemAnimation = 5;
            player.itemTime = 5;
            if (projectile.alpha == 0)
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
            projectile.spriteDirection = player.direction;


            float distance = projectile.Distance(player.Center);
            if (projectile.ai[0] == 0f)
            {
                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= 17f)
                {
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }

                if (projectile.velocity.X < 0f)
                {
                    //projectile.spriteDirection = -1;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI * (projectile.spriteDirection == 1 ? 5 : 5) / 4f)/*1.57f*/;
                }
                else
                {
                    projectile.rotation = projectile.velocity.ToRotation() - (float)(Math.PI * (projectile.spriteDirection == 1 ? 1 : 7) / 4f)/*1.57f*/;
                    //projectile.spriteDirection = 1;

                }
            }
            else
            {
                projectile.tileCollide = false;
                float returnSpeed = 15f;
                float acceleration = 0.5f;

                float xDif = player.Center.X - projectile.Center.X;
                float yDif = player.Center.Y - projectile.Center.Y;

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

                if (projectile.velocity.X < 0f)
                {
                    //projectile.spriteDirection = 1;
                    projectile.rotation = projectile.AngleTo(player.Center) - (float)(Math.PI * (projectile.spriteDirection == 1 ? 5 : 7) / 4f)/*1.57f*/;
                }
                else
                {
                    //projectile.spriteDirection = -1;
                    projectile.rotation = projectile.AngleTo(player.Center) - (float)(Math.PI * (projectile.spriteDirection == 1 ? 5 : 7)/ 4f)/*1.57f*/;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(0, projectile.Center);
            projectile.velocity *= 0;
            projectile.ai[0] = 1;
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Texture2D texture = GetTexture(GlowTexture);
            //spriteBatch.Draw
            //(

            //    texture,
            //    projectile.Center-Main.screenPosition,
            //    new Rectangle(0, 0, texture.Width, texture.Height),
            //    Color.White,
            //    projectile.rotation,
            //    texture.Size() * 0.5f,
            //    projectile.scale,
            //    projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
            //    0f
            //);
        }
    }
}
