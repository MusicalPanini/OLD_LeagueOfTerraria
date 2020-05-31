using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_RuptureControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rupture Control");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 16 * 24;
            projectile.magic = true;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.ai[1]++;

            if (projectile.ai[1] == 16)
            {
                Projectile.NewProjectileDirect(projectile.Center, new Vector2(0, 1000), ModContent.ProjectileType<TerrorOfTheVoid_RuptureSpike>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(0, 2) == 0 ? -1 : 1, (int)projectile.ai[0]);
                projectile.ai[1] = 0;
            }

        }
    }
}
