using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class StrangleThornsTome_Seed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorn Seed");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.minion = true;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.4f;

            if (projectile.velocity.Y > 16)
            {
                projectile.velocity.Y = 16;
            }

            Dust dust = Dust.NewDustPerfect(projectile.Center, 18);
            dust.noGravity = true;
            dust.velocity *= 0;

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != projectile.oldVelocity.X)
            {
                projectile.velocity.X = -projectile.oldVelocity.X * 0.3f;
            }
            else if (projectile.velocity.Y != projectile.oldVelocity.Y && projectile.oldVelocity.Y > 0)
            {
                return true;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 pos = projectile.Center.ToTileCoordinates16().ToWorldCoordinates();
            pos.Y += 12;
            pos.X -= 16;
            Projectile.NewProjectileDirect(pos, Vector2.Zero, ProjectileType<StrangleThornsTome_NightBloomingZychidsBulb>(), projectile.damage, projectile.knockBack, projectile.owner);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;

            width = height = 8;
            return true;
        }
    }
}
