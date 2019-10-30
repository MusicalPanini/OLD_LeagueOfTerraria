﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class StrangleThornsEnd : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorns");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.minion = true;
        }

        public override void AI()
        {
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].type == ProjectileType<NightBloomingZychidsBulb>())
                    {
                        Projectile bulb = Main.projectile[i];

                        if (bulb.Hitbox.Intersects(new Rectangle((int)projectile.Center.X - 75, (int)projectile.Center.Y - 75, 150, 150)))
                        {
                            Projectile.NewProjectileDirect(bulb.position, Vector2.Zero, ProjectileType<EvolutionTurret>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
                            bulb.Kill();
                        }
                    }
                }
            }
            projectile.velocity = Vector2.Zero;

            if (projectile.ai[0] == 0f)
            {
                projectile.alpha -= 75;

                if (projectile.alpha <= 0)
                {
                    projectile.alpha = 0;
                    projectile.ai[0] = 1f;
                    if (projectile.ai[1] == 0f)
                    {
                        projectile.ai[1] += 1f;
                        projectile.position += projectile.velocity * 1f;
                    }
                }
            }
            else
            {
                if (projectile.alpha < 170 && projectile.alpha + 5 >= 170)
                {
                    int num3;
                    for (int num58 = 0; num58 < 3; num58 = num3 + 1)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, 18, projectile.velocity.X * 0.025f, projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
                        num3 = num58;
                    }
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 14, 0f, 0f, 170, default(Color), 1.1f);
                }

                projectile.alpha += 7;

                if (projectile.alpha >= 255)
                {
                    projectile.Kill();
                    return;
                }
            }

            

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Buffs.Seeded>(), 5 * 60);
        }
    }
}
