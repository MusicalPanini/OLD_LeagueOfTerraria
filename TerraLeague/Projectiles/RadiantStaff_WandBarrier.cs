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
    public class RadiantStaff_WandBarrier : ModProjectile
    {
        bool[] hasHitPlayer = new bool[200];

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            DisplayName.SetDefault("Prismatic Barrier");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = false;
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

            Lighting.AddLight(projectile.position, 0.75f, 0.75f, 0.75f);
            for (int i = 0; i < 1; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 16, 16, 16, 0f, 0f, 0, new Color(255, 255, 255), 2f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            if (projectile.timeLeft > 210)
            {
                if (projectile.position.X + (float)(projectile.width / 2) > player.position.X + (float)(player.width / 2))
                    player.ChangeDir(1);
                else
                    player.ChangeDir(-1);
            }

            if (projectile.ai[0] == 0f)
            {
                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= 25f)
                {
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = 0f;
                    projectile.damage *= 2;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                float returnSpeed = 16f;
                float acceleration = 0.5f;

                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num44 = player.position.X + (float)(player.width / 2) - vector2.X;
                float num45 = player.position.Y + (float)(player.height / 2) - vector2.Y;
                float num46 = (float)Math.Sqrt((double)(num44 * num44 + num45 * num45));
                if (num46 > 3000f)
                {
                    projectile.Kill();
                }
                num46 = returnSpeed / num46;
                num44 *= num46;
                num45 *= num46;
                if (projectile.type == 383)
                {
                    Vector2 vector3 = new Vector2(num44, num45) - projectile.velocity;
                    if (vector3 != Vector2.Zero)
                    {
                        Vector2 value = vector3;
                        value.Normalize();
                        projectile.velocity += value * Math.Min(acceleration, vector3.Length());
                    }
                }
                else
                {
                    if (projectile.velocity.X < num44)
                    {
                        projectile.velocity.X = projectile.velocity.X + acceleration;
                        if (projectile.velocity.X < 0f && num44 > 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X + acceleration;
                        }
                    }
                    else if (projectile.velocity.X > num44)
                    {
                        projectile.velocity.X = projectile.velocity.X - acceleration;
                        if (projectile.velocity.X > 0f && num44 < 0f)
                        {
                            projectile.velocity.X = projectile.velocity.X - acceleration;
                        }
                    }
                    if (projectile.velocity.Y < num45)
                    {
                        projectile.velocity.Y = projectile.velocity.Y + acceleration;
                        if (projectile.velocity.Y < 0f && num45 > 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y + acceleration;
                        }
                    }
                    else if (projectile.velocity.Y > num45)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - acceleration;
                        if (projectile.velocity.Y > 0f && num45 < 0f)
                        {
                            projectile.velocity.Y = projectile.velocity.Y - acceleration;
                        }
                    }
                }
                if (Main.myPlayer == projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (rectangle.Intersects(value2))
                    {
                        Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().AddShield(projectile.damage, 240, Color.LightGoldenrodYellow, ShieldType.Basic);
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

            for (int i = 0; i < Main.player.Length; i++)
            {
                if (!hasHitPlayer[i] && projectile.Hitbox.Intersects(Main.player[i].Hitbox))
                {
                    hasHitPlayer[i] = true;
                    Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendShieldPacket(projectile.damage, i, ShieldType.Basic, 240, -1, projectile.owner, Color.LightGoldenrodYellow);
                }
            }
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
    }
}
