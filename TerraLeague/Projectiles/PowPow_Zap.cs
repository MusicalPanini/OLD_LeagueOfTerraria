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
    public class PowPow_Zap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("ZAP!");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1;
            projectile.timeLeft = 900;
            projectile.ranged = true;
            projectile.extraUpdates = 32;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 11), projectile.position);
            }

            projectile.soundDelay = 2;
            Lighting.AddLight(projectile.Left, 0.09f, 0.40f, 0.60f);

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, i < 3 ? new Color(0, 255, 255, 150) : new Color(255, 0, 226), 1f);
                dust.velocity *= Main.rand.Next(6) == 0 ? 2 : 0.3f;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Slowed>(), 240);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, i < 5 ? new Color(0, 255, 255, 150) : new Color(255, 0, 226), 1f);
                dust.velocity *= 3;
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
