using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeofGod_TestofSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test of Spirit");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.alpha = 0;
            projectile.timeLeft = 360;
            projectile.tileCollide = true;
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
                if (Distance > 400)
                {
                    projectile.ai[0] = 1f;
                }
                projectile.rotation = (float)Math.Atan2((double)(player.MountedCenter - (projectile.Top + new Vector2(0, 6))).Y, (double)(player.MountedCenter - (projectile.Top + new Vector2(0, 6))).X) - 1.57f;
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
            else if (projectile.ai[0] >= 1f)
            {
                if ((int)projectile.ai[0] == 2f)
                {
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 73, 0.5f);
                    projectile.ai[0] = 1f;
                }

                projectile.tileCollide = false;
                projectile.friendly = false;
                projectile.rotation = (float)Math.Atan2((double)num168, (double)num167) - 1.57f;
                float num170 = 16f;
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.ai[0] = 1f;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.friendly = false;
            projectile.ai[0] = 2f;
            projectile.netUpdate = true;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vessel || target.immortal)
                return false;

            return base.CanHitNPC(target);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/EyeofGod_TestofSpiritChain");

            Vector2 position = projectile.Top + new Vector2(0, 6);
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
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);

                    if (Main.rand.Next(0, 6) == 0)
                    {
                        Dust dust2 = Dust.NewDustPerfect(position, 59, null, 100, new Color(0, 255, 201), 1f);
                        dust2.fadeIn = 0.5f;
                        dust2.noGravity = true;
                    }
                }
            }

            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
