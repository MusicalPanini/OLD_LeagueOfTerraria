using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkinBow_DarkinArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Arrow");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.timeLeft = 60;
            projectile.ranged = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 1f - (projectile.alpha/255f), 0f, 0f);
            projectile.scale = 1 + projectile.ai[0];
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.timeLeft < 10)
            {
                projectile.alpha += 26;
            }

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Blood, 0, 0, 0, default(Color), projectile.scale + 0.2f);
            dust.noGravity = true;
            dust.velocity.Y *= 0.1f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
