using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class LightPistol_Bullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Pistol");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 180;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 12, -0.25f);
            }
            projectile.soundDelay = 100;

            Lighting.AddLight(projectile.position, Color.White.ToVector3());

            for (int i = 0; i < 3; i++)
            {
                
                Dust dust = Dust.NewDustPerfect(projectile.position, 66);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 17; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 66, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, default(Color), 1f);
                dust.noGravity = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}