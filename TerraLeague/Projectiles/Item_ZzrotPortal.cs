﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_ZzrotPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zz'Rot Portal");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.alpha = 0;
            projectile.timeLeft = 60 * 5 + 5;
            projectile.minion = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.60f, 0f, 0.60f);

            if (projectile.timeLeft == 60 * 5 + 5)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 27, 0f, 0, 0, default(Color), 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 27, 0f, 0, 0, default(Color), 2f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 3;
                }
            }

            if (projectile.ai[1] >= 60)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 103), projectile.Center);
                if (sound != null)
                    sound.Pitch = -0.25f;

                for (int i = 0; i < (int)projectile.ai[0] + Main.player[projectile.owner].maxMinions; i++)
                {
                    Projectile.NewProjectile(projectile.Center, new Vector2(Main.rand.NextFloat(-4, 4), -6), ModContent.ProjectileType<VoidProphetsStaff_Zzrot>(), projectile.damage, projectile.knockBack, projectile.owner);
                }

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 27, 0, -3);
                }

                projectile.ai[1] = 0;
            }
            else
            {
                projectile.ai[1]++;
            }

            if (Main.rand.Next(0, 30) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 27, 0, 0, 0, default(Color), 1.5f);
                dust.fadeIn = 1;
                dust.velocity *= 0.1f;
            }

            Animation();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 27, 0f, 0, 0, default(Color), 3f);
                dust.noGravity = true;
                dust.velocity.Y -= 2;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 16), projectile.width, projectile.height, 27, 0f, 0, 0, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity.Y -= 3;
            }
            base.Kill(timeLeft);
        }

        void Animation()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 6)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }
    }
}
