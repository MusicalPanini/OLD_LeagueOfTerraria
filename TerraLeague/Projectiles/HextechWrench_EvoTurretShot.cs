using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HextechWrench_EvoTurretShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evolution Turret");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 255;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;

        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 159, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            Lighting.AddLight(projectile.position, 0.5f, 0.5f, 0f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 159, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 50, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
