using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class SolariSet_SolarFlare : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = 3;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 600;
            projectile.magic = true;
            projectile.extraUpdates = 7;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 1f, 1f, 0f);

            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
            {
                projectile.tileCollide = true;
            }

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt, 0, 4, 150, default(Color), 3f);
            dust.velocity.X *= 0;
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Stunned>(), 60*6);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.AmberBolt, 0, -4, 150, default(Color), 2f);
                dust.velocity.X *= 3;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }
    }
}
