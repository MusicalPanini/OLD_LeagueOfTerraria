using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class OrbofDeception_FoxFire : ModProjectile
	{
        Vector2 offset = new Vector2(65, 0);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fox-Fire");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.ai[0] += .07f;
            projectile.Center = player.MountedCenter + offset.RotatedBy(projectile.ai[0]);

            Lighting.AddLight(projectile.position, 1f, 1f, 1f);
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 92, projectile.velocity.X, projectile.velocity.Y, 200, new Color(255, 255, 255), 1.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 16, 16, 92, 0, 0, 50, new Color(255, 255, 255), 1.2f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }
    }
}
