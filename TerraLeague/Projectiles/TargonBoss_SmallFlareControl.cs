using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_SmallFlareControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 150;
            projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            if (projectile.timeLeft <= 90)
            {
                if (projectile.timeLeft % 4 == 0)
                {
                    if (projectile.timeLeft % 16 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 34, 0f);
                    }

                    float x = Main.rand.NextFloat(projectile.position.X + 4, projectile.position.X + 12);
                    float y = Main.rand.NextFloat(projectile.position.Y + 4, projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(new Vector2(x, y), new Vector2(0, 16), ModContent.ProjectileType<TargonBoss_SmallFlare>(), projectile.damage, projectile.knockBack, projectile.owner);

                    x = Main.rand.NextFloat(projectile.position.X + 4, projectile.position.X + 12);
                    y = Main.rand.NextFloat(projectile.position.Y + 4, projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(new Vector2(x, y), new Vector2(0, -16), ModContent.ProjectileType<TargonBoss_SmallFlare>(), projectile.damage, projectile.knockBack, projectile.owner);

                    x = Main.rand.NextFloat(projectile.position.X + 4, projectile.position.X + 12);
                    y = Main.rand.NextFloat(projectile.position.Y + 4, projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(new Vector2(x, y), new Vector2(16, 0), ModContent.ProjectileType<TargonBoss_SmallFlare>(), projectile.damage, projectile.knockBack, projectile.owner);

                    x = Main.rand.NextFloat(projectile.position.X + 4, projectile.position.X + 12);
                    y = Main.rand.NextFloat(projectile.position.Y + 4, projectile.position.Y + 12);
                    Projectile.NewProjectileDirect(new Vector2(x, y), new Vector2(-16, 0), ModContent.ProjectileType<TargonBoss_SmallFlare>(), projectile.damage, projectile.knockBack, projectile.owner);

                }
            }
            else
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt, 0, 0, 150, default(Color), 2f);
                dust.velocity *= 0;
                dust.noGravity = true;
                dust.fadeIn = 0;

                for (int i = 0; i < 4; i++)
                {
                    Vector2 vel = new Vector2(8, 0).RotatedBy(MathHelper.PiOver2 * i);

                    dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt, 0, 0, 150, default(Color), 1f);
                    dust.velocity = vel;
                    dust.noGravity = true;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 192, projectile.velocity.X / 2, projectile.velocity.Y / 2, 100, new Color(255, 192, 0), 0.5f);
            //}

            base.Kill(timeLeft);
        }
    }
}
