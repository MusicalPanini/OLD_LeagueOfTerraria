using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class DivineJudgement : ModProjectile
    {
        int framecount2 = 29;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Judgement Shield");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.timeLeft = 120;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[(int)projectile.ai[0]];

            projectile.Center = player.Center;
            player.AddBuff(BuffType<DivineJudgementBuff>(), projectile.timeLeft);
            Lighting.AddLight(projectile.Center, new Color(255, 226, 82).ToVector3());


            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 87, 0, -4, 200, default(Color), 1f);
                dust.noGravity = true;
            }

            //int num = Main.rand.Next(0, 3);
            //if (num == 0)
            //{
            //    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
            //    Main.dust[dustIndex].velocity.X *= 0.3f;
            //    Main.dust[dustIndex].color = new Color(0, 255, 150);
            //    Main.dust[dustIndex].noGravity = false;
            //}
            //else if (num == 2)
            //{
            //    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
            //    Main.dust[dustIndex].velocity.X *= 0.3f;
            //    Main.dust[dustIndex].color = new Color(0, 255, 0);
            //    Main.dust[dustIndex].noGravity = false;
            //}
            //Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 16);

            AnimateProjectile();
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[(int)projectile.ai[0]];
            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectileDirect(new Vector2(player.Center.X - 345 + (115 * i), player.position.Y - (Main.screenHeight / 2)), new Vector2(0, 25), ProjectileType<DivineJudgementSword>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
            base.Kill(timeLeft);
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            projectile.friendly = false;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                projectile.frame++;
                projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                projectile.frameCounter = 0;
            }
        }
    }
}
