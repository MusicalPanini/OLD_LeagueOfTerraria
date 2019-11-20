using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Lantern : ModProjectile
    {
        int effectRadius = 200;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lantern");
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 44;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.timeLeft = 360;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            Lighting.AddLight(projectile.Left, Color.SeaGreen.ToVector3());

            if ((int)projectile.ai[1] == 0)
            {
                if (projectile.timeLeft == 360)
                {
                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    int shieldAmount = (int)(projectile.damage * modPlayer.healPower);

                    for (int i = 0; i < effectRadius / 5; i++)
                    {
                        Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + projectile.Center;

                        Dust dustR = Dust.NewDustPerfect(pos, 188, Vector2.Zero, 0, new Color(0, 255, 0, 0), 2);
                        dustR.noGravity = true;
                    }

                    for (int j = 0; j < 50; j++)
                    {
                        Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 188);
                        dust.noGravity = true;
                        dust.scale = 2;
                    }

                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        Player healTarget = Main.player[i];

                        if (projectile.Distance(healTarget.Center) < 300 && healTarget.active)
                        {
                            if (i == projectile.owner)
                            {
                                modPlayer.AddShield(shieldAmount, 240, Color.SeaGreen, ShieldType.Basic);
                            }
                            else if (Main.netMode == 1)
                            {
                                modPlayer.SendShieldPacket(shieldAmount, i, ShieldType.Basic, 240, -1, player.whoAmI, Color.SeaGreen);
                            }
                        }
                    }
                }

                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (projectile.Hitbox.Intersects(Main.player[i].Hitbox) && i != projectile.owner)
                    {
                        Main.player[i].position = player.position;
                        projectile.Kill();
                    }
                }
            }

            if (projectile.Distance(player.MountedCenter) > 1000)
            {
                projectile.Kill();
            }

            projectile.position.Y += (float)Math.Sin(projectile.timeLeft * 0.05) * 0.4f;

            Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 188, 0, -3);
            dust2.scale = 0.75f;
            dust2.alpha = 150;
            dust2.velocity /= 3;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);

            }

            base.Kill(timeLeft);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/ChainWardensScytheChain");

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
                }
            }

            return true;
        }
    }
}
