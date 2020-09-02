using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeofGod_Tentacle : ModProjectile
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
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.minion = true;
            projectile.sentry = true;
            projectile.tileCollide = true;
            projectile.friendly = false;
        }

        public override void AI()
        {
            if (projectile.ai[1] >= 0)
            {
                projectile.timeLeft = (int)projectile.ai[1];
                projectile.ai[1] = -1;
            }
            if (projectile.timeLeft == Projectile.SentryLifeTime)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 59, 0f, -2f, 200, new Color(0, 255, 201), 5f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 59, 0f, -1f, 200, new Color(0, 255, 201), 4f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 3;
                }
            }

            Dust dust2 = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.Bottom.Y - 4), projectile.width, 4, 257, 0f, 0, 100, new Color(0, 255, 201), 1f);
            dust2.fadeIn = 1.3f;
            dust2.noGravity = true;
            dust2.velocity.Y *= 0.2f;

            if (projectile.ai[0] <= 0)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && !npc.immortal)
                    {
                        if (npc.Hitbox.Intersects(new Rectangle((int)projectile.Left.X - 128, (int)projectile.Top.Y + 32, projectile.width + 272, projectile.height)))
                        {
                            int direction = npc.Center.X > projectile.Center.X ? 1 : -1;

                            projectile.ai[1] = projectile.timeLeft;
                            projectile.Kill();
                            projectile.netUpdate = true;
                            Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<EyeofGod_TentacleAttack>(), projectile.damage, projectile.knockBack, projectile.owner, direction, projectile.ai[1]);
                            break;
                        }
                    }
                }
            }
            else
            {
                projectile.ai[0]--;
            }

            Lighting.AddLight(projectile.Center, 0, 0.1f, 0.05f);

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
            //if (projectile.frameCounter >= ((projectile.frame == 3 || projectile.frame == 7) ? 12 : 8))
            if (projectile.frameCounter >= 12)
            {
                projectile.frame++;
                projectile.frame %= 8;
                projectile.frameCounter = 0;
            }
        }
    }
}
