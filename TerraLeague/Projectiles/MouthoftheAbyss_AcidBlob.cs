using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MouthoftheAbyss_AcidBlob : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caustic Spittle");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 185;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.scale = 0.75f;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 95), projectile.Center);
            }
            projectile.soundDelay = 100;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.timeLeft < 185 - 30)
            {
                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.97f;

            }

            if (projectile.velocity.Y > 16)
                projectile.velocity.Y = 16;

            if (Main.rand.Next(0, 3) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 167, 0, 0, 50);
                dustIndex.velocity *= 0.3f;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Ichor, 120);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(3, 13), projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 167, projectile.velocity.X * 0.35f, projectile.velocity.Y * 0.35f, 50);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
