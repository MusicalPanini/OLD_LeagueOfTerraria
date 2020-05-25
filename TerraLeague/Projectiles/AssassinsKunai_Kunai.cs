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
    public class AssassinsKunai_Kunai : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Assassin's Kunai");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.timeLeft = 185;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.scale = 0.75f;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.timeLeft < 180)
            {
                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.97f;
            }

            if (projectile.velocity.Y > 16)
                projectile.velocity.Y = 16;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 8, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
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
