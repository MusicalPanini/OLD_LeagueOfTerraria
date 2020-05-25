using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_PathMarker : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("AxePathMarker");
        }

        public override void SetDefaults()
        {
            projectile.width = 78;
            projectile.height = 78;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 32;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.velocity.Y < 15)
            {
                projectile.velocity.Y += 0.3f;
            }

            if (projectile.timeLeft % 8 == 0 && Main.myPlayer == projectile.owner)
            {
                Dust dust2 = Dust.NewDustPerfect(projectile.Center, 211, null, 0, new Color(255, 0, 0), 2);
                dust2.noGravity = true;
                dust2.velocity *= 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == projectile.owner)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustPerfect(new Vector2(projectile.position.X + (int)((projectile.width / 19.0) * i), projectile.position.Y + projectile.height - 24), 6, null, 0, new Color(255, 125, 0), 4f);
                    dust.velocity *= 0f;
                    dust.noGravity = true;
                }
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 24;
            return true;
        }
    }
}
