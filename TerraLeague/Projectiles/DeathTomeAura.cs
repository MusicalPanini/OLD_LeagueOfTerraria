using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class DeathTomeAura : ModProjectile
    {
        int framecount2 = 29;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Defile");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 400;
            projectile.height = 400;
            projectile.timeLeft = 180;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 180;
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].Center;
                Lighting.AddLight(projectile.Center, 0f, 0.75f, 0.3f);

            int num = Main.rand.Next(0, 3);
            if (num == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                Main.dust[dustIndex].velocity.X *= 0.3f;
                Main.dust[dustIndex].color = new Color(0, 255, 150);
                Main.dust[dustIndex].noGravity = false;
            }
            else if (num == 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                Main.dust[dustIndex].velocity.X *= 0.3f;
                Main.dust[dustIndex].color = new Color(0, 255, 0);
                Main.dust[dustIndex].noGravity = false;
            }
            //Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 16);
            if (projectile.timeLeft < 15)
            {
                projectile.alpha += 5;
            }
            AnimateProjectile();
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            projectile.friendly = false;
            projectile.frameCounter++;
            framecount2++;
            if (projectile.frameCounter >= 5) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                projectile.frame++;
                projectile.frame %= 4; // Will reset to the first frame if you've gone through them all.
                projectile.frameCounter = 0;
            }
            if (framecount2 >= 20)
            {
                projectile.friendly = true;
                framecount2 = 0;
            }
        }
    }
}
