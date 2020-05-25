using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Bullet_HextechChloroShot : ModProjectile
    {
        bool split = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech CH-300 Balistic");
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
            Lighting.AddLight(projectile.Left, Color.LightGreen.ToVector3());

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
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X/2, projectile.velocity.Y/2, 100, new Color(0, 255, 0), 0.5f);
            }

            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public void Split(int num = -1)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 0.5f;
            }

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75);
                dust.noGravity = true;
            }

            if (!split)
            {
                if (num == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            Projectile.NewProjectileDirect(projectile.position, projectile.velocity.RotatedByRandom(MathHelper.TwoPi) / 12, ProjectileType<Bullet_HextechChloroShotSplit>(), projectile.damage / 2, 0, projectile.owner, num == -1 ? 255 : num);
                        }
                        else
                        {
                            Projectile.NewProjectileDirect(projectile.position, projectile.velocity.RotatedByRandom(MathHelper.TwoPi) / 12, ProjectileType<Bullet_HextechShotSplit>(), projectile.damage / 4, 0, projectile.owner, num == -1 ? 255 : num);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            Projectile.NewProjectileDirect(projectile.position, projectile.velocity.RotatedBy(MathHelper.ToRadians(-15 + (15 * i))) / 12, ProjectileType<Bullet_HextechChloroShotSplit>(), projectile.damage / 2, 0, projectile.owner, num == -1 ? 255 : num);

                        }
                        else
                        {
                            Projectile.NewProjectileDirect(projectile.position, projectile.velocity.RotatedBy(MathHelper.ToRadians(-15 + (15 * i))) / 12, ProjectileType<Bullet_HextechShotSplit>(), projectile.damage / 4, 0, projectile.owner, num == -1 ? 255 : num);

                        }
                    }
                }

                split = true;
            }
        }
    }
}
