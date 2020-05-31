using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_VorpalSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vorpal Spike");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 600;
            projectile.magic = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Left, 0.60f, 0.32f, 0f);

            if (projectile.alpha > 0)
                projectile.alpha -= 15;
            if (projectile.alpha < 0)
                projectile.alpha = 0;

            if (projectile.timeLeft < 600 - 15)
                projectile.velocity.Y += 0.4f;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 97, 0f, 0f, projectile.alpha, default(Color));
            dustIndex.noGravity = true;

            if (projectile.velocity.Y < -16f)
                projectile.velocity.Y = -16f;
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
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 97, projectile.velocity.X / 2, projectile.velocity.Y / 2);
            }

            base.Kill(timeLeft);
        }
    }
}
