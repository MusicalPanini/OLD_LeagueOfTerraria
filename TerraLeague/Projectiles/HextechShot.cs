﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class HextechShot : ModProjectile
    {
        bool split = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Bullet");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 50;
            projectile.ranged = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Left, 0.09f, 0.40f, 0.60f);


            if (projectile.alpha > 0)
            {
                projectile.alpha -= 15;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            if (projectile.timeLeft == 1)
            {
                Split(-1);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Split(target.whoAmI);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(0, 192, 255), 0.5f);
            }

            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            
            base.Kill(timeLeft);
        }

        public void Split(int num = -1)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                Main.dust[dustIndex].velocity *= 0.5f;

            }


            if (!split)
            {
                if (num == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectileDirect(projectile.position, projectile.velocity.RotatedByRandom(MathHelper.TwoPi)/12, ProjectileType<HextechShotSplit>(), projectile.damage / 4, 0, projectile.owner, num == -1 ? 255 : num);
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectileDirect(projectile.position, projectile.velocity.RotatedBy(MathHelper.ToRadians(-15 + (15 * i)))/ 12, ProjectileType<HextechShotSplit>(), projectile.damage / 4, 0, projectile.owner, num == -1 ? 255 : num);
                    }
                }
                

                split = true;
            }
        }
    }
}
