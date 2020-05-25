using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class StarfireSpellblades_DivineJudgement : ModProjectile
    {
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

            AnimateProjectile();
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[(int)projectile.ai[0]];
            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectileDirect(new Vector2(player.Center.X - 345 + (115 * i), player.position.Y - (Main.screenHeight / 2)), new Vector2(0, 25), ProjectileType<StarfireSpellblades_DivineJudgementSword>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
            base.Kill(timeLeft);
        }

        public void AnimateProjectile() 
        {
            projectile.friendly = false;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5) 
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }
    }
}
