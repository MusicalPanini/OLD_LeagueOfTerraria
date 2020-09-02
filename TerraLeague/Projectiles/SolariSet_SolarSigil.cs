using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_SolarSigil : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Sigil");
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 257;
            projectile.magic = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 1f, 1f, 0f);

            if (projectile.timeLeft > 17)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 15;
                }
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }

                if (projectile.timeLeft <= 240 && projectile.timeLeft % 4 == 0)
                {
                    if (projectile.timeLeft % 16 == 0)
                    {
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 9, 0.5f);
                    }

                    float x = Main.rand.NextFloat(projectile.position.X + 12, projectile.position.X + 34);
                    float y = Main.rand.NextFloat(projectile.position.Y + 12, projectile.position.Y + 34);
                    Projectile proj = Projectile.NewProjectileDirect(new Vector2(x, y - 8), new Vector2(0, 16), ModContent.ProjectileType<SolariSet_SolarFlare>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
            else
            {
                projectile.alpha += 15;
            }

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
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
