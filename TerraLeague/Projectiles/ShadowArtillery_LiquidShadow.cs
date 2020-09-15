using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ShadowArtillery_LiquidShadow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Liquid Shadow");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            projectile.tileCollide = true;
            projectile.localAI[1] = 0f;
            if (Main.myPlayer == projectile.owner && projectile.ai[0] == 0f)
            {
                projectile.tileCollide = false;
                if (Main.player[projectile.owner].channel)
                {
                    projectile.localAI[1] = -1f;
                    float num145 = 12f;
                    Vector2 vector13 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num146 = (float)Main.mouseX + Main.screenPosition.X - vector13.X;
                    float num147 = (float)Main.mouseY + Main.screenPosition.Y - vector13.Y;
                    if (Main.player[projectile.owner].gravDir == -1f)
                    {
                        num147 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector13.Y;
                    }
                    float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                    num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                    if (num148 > num145)
                    {
                        num148 = num145 / num148;
                        num146 *= num148;
                        num147 *= num148;
                        if (num146 != projectile.velocity.X || num147 != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num146;
                        projectile.velocity.Y = num147;
                    }
                    else
                    {
                        if (num146 != projectile.velocity.X || num147 != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num146;
                        projectile.velocity.Y = num147;
                    }
                }
                else
                {
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
            }
            if (projectile.ai[0] == 1f && projectile.type != 109)
            {
                if (projectile.type == 42 || projectile.type == 65 || projectile.type == 68 || projectile.type == 354)
                {
                    projectile.ai[1] += 1f;
                    if (projectile.ai[1] >= 60f)
                    {
                        projectile.ai[1] = 60f;
                        projectile.velocity.Y += 0.2f;
                    }
                }
                else
                {
                    projectile.velocity.Y += 0.41f;
                }
            }
            else if (projectile.ai[0] == 2f && projectile.type != 109)
            {
                projectile.velocity.Y += 0.2f;
                if ((double)projectile.velocity.X < -0.04)
                {
                    projectile.velocity.X += 0.04f;
                }
                else if ((double)projectile.velocity.X > 0.04)
                {
                    projectile.velocity.X -= 0.04f;
                }
                else
                {
                    projectile.velocity.X = 0f;
                }
            }
            projectile.rotation += 0.1f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            Dust dust;
            for (int i = 0; i < 3; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 63, 0f, 0f, 0, new Color(5, 245, 150), 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 54, 0, 0, 50, Color.DarkSeaGreen, 2f);
            dust.noGravity = true;
            dust.velocity /= 2f;


        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Blackout, 30);

            base.OnHitPlayer(target, damage, crit);
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
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 54, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, Color.DarkSeaGreen, 1f);
            }

            base.Kill(timeLeft);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
