using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class BurningVengance_Flame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burning Vengance");
        }

        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 15;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 250;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 30;
            projectile.magic = true;
            projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            if (projectile.wet)
            {
                projectile.Kill();
                return;
            }

            if (projectile.timeLeft <= 27)
            {
                   

                if (Main.rand.Next(0,1) == 0)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 0, 0, default(Color), 4f);
                    dust.noGravity = true;

                    Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 3, 0, default(Color), 1f);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 1200);

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

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
