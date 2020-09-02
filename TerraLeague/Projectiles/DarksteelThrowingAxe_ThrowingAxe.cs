using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_ThrowingAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Throwing Axe");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;

            if (projectile.velocity.X > 0)
                projectile.rotation += 0.5f;
            else
                projectile.rotation += -0.5f;

            if (projectile.timeLeft < 270 && projectile.velocity.Y < 15)
                projectile.velocity.Y += 0.8f;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 8, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 8, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 4, -0.5f);
            return true;
        }
    }
}
