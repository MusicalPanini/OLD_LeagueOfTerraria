using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class LightPet_Flame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(3) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 0, new Color(0, 255, 180), 2f);
                dust.velocity *= 0.1f;
                dust.velocity.X = projectile.velocity.X;
                dust.fadeIn = 0.1f;
                dust.noGravity = true;
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 0, new Color(0, 255, 180), 1.5f);
                dust.velocity.X = projectile.velocity.X;
                dust.velocity.Y = Main.rand.NextFloat(-2, 0);
                dust.fadeIn = 0.5f;
                dust.noGravity = true;
            }
            Lighting.AddLight(projectile.Center, 0, 1, 0.6f);
            projectile.velocity *= 0.99f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
