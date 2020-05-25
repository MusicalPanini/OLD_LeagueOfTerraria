using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TideCallerStaff_WaterShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Pellet");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;

            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 137, 0f, 0f, 100, default(Color), 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 172, 0f, 0f, 100, default(Color), 0.5f);
                dust.velocity *= 0.25f;
                dust.velocity += projectile.velocity * 0.5f;
            }
            if (projectile.timeLeft < 30)
                projectile.alpha += 9;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 17; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 211, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, default(Color), 1f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}