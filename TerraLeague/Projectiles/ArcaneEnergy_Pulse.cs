using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_Pulse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Arcanopulse");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1;
            projectile.timeLeft = 900;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 32;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                projectile.timeLeft = (int)projectile.ai[0] / 2;
                projectile.extraUpdates = projectile.timeLeft - 1;
            }
            projectile.soundDelay = 1000;

            Lighting.AddLight(projectile.Left, 0.09f, 0.40f, 0.60f);

            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 0, default(Color), 4f);
                dust.velocity *= 0;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
