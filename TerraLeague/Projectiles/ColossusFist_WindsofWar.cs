using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ColossusFist_WindsofWar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winds of War");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 162;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 180;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 100)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 10;
                }
                if (projectile.alpha < 100)
                {
                    projectile.alpha = 100;
                }
            }
            else if (projectile.timeLeft < 155/10f)
            {
                projectile.alpha += 10;

                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                }
            }
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0, -3, projectile.alpha/2, default(Color), 0.5f);
            if (projectile.owner == Main.LocalPlayer.whoAmI && (int)projectile.ai[0] > 0 && projectile.timeLeft == 175)
            {
                Projectile proj = Projectile.NewProjectileDirect(projectile.Center + new Vector2(0, -42), Vector2.Zero, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, (int)projectile.ai[0] - 1);
                proj.tileCollide = false;
            }

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
            if (projectile.frameCounter >= 4)
            {
                projectile.frame++;
                projectile.frame %= 6;
                projectile.frameCounter = 0;
            }
        }
    }
}
