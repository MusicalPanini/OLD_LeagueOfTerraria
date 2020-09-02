using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeofGod_TentacleAttack : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle");
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 108;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 100;
            projectile.scale = 1f;
            projectile.timeLeft = 6000;
            projectile.minion = true;
            projectile.sentry = true;
            projectile.tileCollide = true;
            projectile.friendly = false;
            drawOffsetX = -24;
        }

        public override void AI()
        {
            if (projectile.ai[1] >= 0)
            {
                projectile.timeLeft = (int)projectile.ai[1];
                projectile.ai[1] = -1;
            }

            if ((int)projectile.ai[0] == 1)
                drawOffsetX = -24;
            else
                drawOffsetX = -116;
            projectile.spriteDirection = (int)projectile.ai[0];
            Lighting.AddLight(projectile.Center, 0, 0.1f, 0.05f);

            Dust dust2 = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.Bottom.Y - 4), projectile.width, 4, 257, 0f, 0, 100, new Color(0, 255, 201), 1f);
            dust2.fadeIn = 1.3f;
            dust2.noGravity = true;
            dust2.velocity.Y *= 0.2f;

            AnimateProjectile();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            int frameDelay;

            switch (projectile.frame)
            {
                // Preparing
                case 0:
                case 1:
                case 2:
                    frameDelay = 4;
                    break;
                // Swinging
                case 3:
                case 4:
                    frameDelay = 2;
                    break;
                // The Slam
                case 5:
                    frameDelay = 13;
                    break;
                // Reseting
                case 6:
                case 7:
                    frameDelay = 6;
                    break;
                default:
                    frameDelay = 0;
                    break;
            }

            if (projectile.frameCounter >= frameDelay)
            {
                projectile.frame++;
                if (projectile.frame == 8)
                {
                    projectile.ai[1] = projectile.timeLeft;
                    projectile.Kill();
                    Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<EyeofGod_Tentacle>(), projectile.damage, projectile.knockBack, projectile.owner, 60 * Main.player[projectile.owner].meleeSpeed, projectile.ai[1]);
                }
                projectile.frame %= 8;
                projectile.frameCounter = 0;

                if (projectile.frame == 5)
                {
                    Projectile.NewProjectileDirect(new Vector2(projectile.Center.X + (72 * (int)projectile.ai[0]), projectile.Center.Y), Vector2.Zero, ModContent.ProjectileType<EyeofGod_TentacleHitbox>(), projectile.damage, projectile.knockBack, projectile.owner);
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 14, -0.25f);
                }
            }
        }
    }
}
