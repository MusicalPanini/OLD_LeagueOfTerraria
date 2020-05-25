using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Bullet_HextechShotSplit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Hextech Bolt");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 3;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 900;
            projectile.ranged = true;
            projectile.extraUpdates = 16;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Left, 0.09f, 0.40f, 0.60f);

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            Dust dust = Dust.NewDustPerfect(projectile.position, 111, Vector2.Zero, 0, default(Color), 0.5f);
            dust.noGravity = true;
            dust.alpha = 100;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
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

        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.ai[0] != target.whoAmI && !target.friendly)
                return true;
            else
                return false;
        }
    }
}
