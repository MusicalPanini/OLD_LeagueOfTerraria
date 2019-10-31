using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TrueIceFlailP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Flail");
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.width = 22;
            projectile.height = 22;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.scale = 1.25f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/TrueIceFlailChain");

            Vector2 position = projectile.Bottom.RotatedBy(projectile.rotation, projectile.Center);
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
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
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
                }
            }

            return true;
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
                projectile.alpha -= 20;

            for (int i = 0; i < 1; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color),1.5f);
                dustIndex.noGravity = true;
                dustIndex.velocity *= 0.3f;
            }

            Player player = Main.player[projectile.owner];

            player.itemAnimation = 5;
            player.itemTime = 5;

            if (projectile.ai[0] == 0f)
            {
                projectile.ai[1] += 1f;


                if (projectile.ai[1] >= 10f)
                {
                    projectile.velocity.Y = projectile.velocity.Y + 0.5f;
                    projectile.velocity.X = projectile.velocity.X * 0.95f;
                    if (projectile.velocity.Y > 16f)
                    {
                        projectile.velocity.Y = 16f;
                    }
                }
                else if (projectile.ai[1] >= 20f)
                {
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                projectile.tileCollide = false;
                float maxReturnSpeed = 9f;
                float ReturnAccel = 0.4f;
                maxReturnSpeed = 24f;
                ReturnAccel = 6f;

                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num44 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector2.X;
                float num45 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector2.Y;
                float num46 = (float)Math.Sqrt((double)(num44 * num44 + num45 * num45));
                if (num46 > 3000f)
                {
                    projectile.Kill();
                }
                num46 = maxReturnSpeed / num46;
                num44 *= num46;
                num45 *= num46;

                Vector2 vector3 = new Vector2(num44, num45) - projectile.velocity;
                if (vector3 != Vector2.Zero)
                {
                    Vector2 value = vector3;
                    value.Normalize();
                    projectile.velocity += value * Math.Min(ReturnAccel, vector3.Length());
                }

                if (Main.myPlayer == projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    Rectangle value2 = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
                    if (rectangle.Intersects(value2))
                    {
                        projectile.Kill();
                    }
                }
            }

            if (projectile.ai[0] == 0f)
            {
                Vector2 velocity = projectile.velocity;
                velocity.Normalize();
                projectile.rotation = (float)Math.Atan2((double)velocity.Y, (double)velocity.X) + 1.57f;
                return;
            }
            Vector2 vector4 = projectile.Center - Main.player[projectile.owner].Center;
            vector4.Normalize();
            projectile.rotation = (float)Math.Atan2((double)vector4.Y, (double)vector4.X) + 1.57f;
            return;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.Next(0, 4) == 0)
            {
                if (target.HasBuff(BuffType<Slowed>()))
                {
                    target.AddBuff(BuffType<Frozen>(), 180);
                    target.DelBuff(target.FindBuffIndex(BuffType<Slowed>()));
                }
                else
                {
                    target.AddBuff(BuffType<Slowed>(), 180);
                }
            }
            else if (target.HasBuff(BuffType<Slowed>()))
            {
                target.AddBuff(BuffType<Slowed>(), 180);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(0, projectile.Center);
            projectile.ai[0] = 1;
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
