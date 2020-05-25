using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class PowPow_Bullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pow Pow");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if(projectile.timeLeft == 90)
                Main.PlaySound(SoundID.Item11, projectile.position);

            Lighting.AddLight(projectile.position, 1f, 0.34f, 0.9f);

            if (projectile.alpha > 0)
                projectile.alpha -= 15;

            if (projectile.alpha < 0)
                projectile.alpha = 0;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
                projectile.velocity.Y = 16f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
                Main.player[projectile.owner].AddBuff(BuffType<PowPowExcited>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 119, 0f, 0f, 100, default(Color), 0.7f);
            }
        }
    }
}
