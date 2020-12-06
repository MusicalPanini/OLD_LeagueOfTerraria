using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Summoner_SyphonVisuals : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Syphon Visuals");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 3;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 pos = projectile.Center + new Vector2(projectile.timeLeft * 1.5f).RotatedBy((projectile.timeLeft * 0.03f) + (MathHelper.TwoPi * i / 6));

                Dust dust = Dust.NewDustPerfect(pos, 263, Vector2.Zero, 0, new Color(255, 0, 120), 2);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
        }
    }
}
