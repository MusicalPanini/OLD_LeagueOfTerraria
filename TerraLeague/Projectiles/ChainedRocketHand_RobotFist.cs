using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ChainedRocketHand_RobotFist : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chained Rocket Hand");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GolemFist);
            projectile.aiStyle = 0;
            projectile.alpha = 255;
        }
        public override void AI()
        {
            if (projectile.alpha > 0)
                projectile.alpha -= 20;
            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            player.itemRotation = player.AngleTo(projectile.Center);
            if (player.itemRotation > MathHelper.PiOver2)
            {
                player.itemRotation -= MathHelper.Pi;
            }
            else if (player.itemRotation <= -MathHelper.PiOver2)
            {
                player.itemRotation += MathHelper.Pi;
            }

            if (player.dead)
            {
                projectile.Kill();
                return;
            }
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
            Vector2 vector15 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num167 = player.position.X + (float)(player.width / 2) - vector15.X;
            float num168 = player.position.Y + (float)(player.height / 2) - vector15.Y;
            float Distance = (float)Math.Sqrt((double)(num167 * num167 + num168 * num168));
            if (projectile.ai[0] == 0f)
            {
                if (Distance > 200)
                {
                    projectile.ai[0] = 1f;
                }
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                projectile.ai[1] += 1f;
                if (projectile.ai[1] > 8f)
                {
                    projectile.ai[1] = 8f;
                }
                if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = -1;

                }
                else
                {
                    projectile.spriteDirection = 1;

                }
            }
            else if (projectile.ai[0] == 1f)
            {
                projectile.tileCollide = false;
                projectile.rotation = (float)Math.Atan2((double)num168, (double)num167) - 1.57f;
                float num170 = 7f;
                if (Distance < 50f)
                {
                    projectile.Kill();
                }
                Distance = num170 / Distance;
                num167 *= Distance;
                num168 *= Distance;
                projectile.velocity.X = num167;
                projectile.velocity.Y = num168;
                if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = 1;

                }
                else
                {
                    projectile.spriteDirection = -1;
                }
            }
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.ai[0] = 1f;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDustDirect(projectile.Center, 8, 8, 51,0,0,0,default(Color), 0.75f);
            }
            projectile.ai[0] = 1f;
            return false;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/ChainedRocketHand_RobotChain");

            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;

            mountedCenter.Y += 4;
            Rectangle? sourceRectangle = new Rectangle?();
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
                    spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
            Texture2D robotBase = mod.GetTexture("Projectiles/ChainedRocketHand_RobotBase");
            origin = new Vector2((float)robotBase.Width * 0.5f, (float)robotBase.Height * 0.5f);
            Color color3 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
            spriteBatch.Draw(robotBase, mountedCenter - Main.screenPosition, sourceRectangle, color3, rotation, new Vector2((float)robotBase.Width * 0.5f, robotBase.Height), 1f, SpriteEffects.None, 0f);

            return true;
        }
    }
}
