using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    class DeathTomeShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Death Singer's Tome");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 75;
            projectile.height = 75;
            projectile.timeLeft = 42;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            projectile.Center = new Vector2(projectile.ai[0], projectile.ai[1]);

            int num = Main.rand.Next(0, 3);
            if (num == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                Main.dust[dustIndex].velocity.X *= 0.3f;
                Main.dust[dustIndex].color = new Color(0, 255, 150);
                Main.dust[dustIndex].alpha = projectile.alpha;
                Main.dust[dustIndex].noGravity = false;
            }
            else if (num == 2)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                Main.dust[dustIndex].velocity *= 0.3f;
                Main.dust[dustIndex].alpha = projectile.alpha;
                Main.dust[dustIndex].color = new Color(0, 255, 0);
                Main.dust[dustIndex].noGravity = false;
            }

            Lighting.AddLight(projectile.Center, 0f, 0.75f, 0.3f);
            if (projectile.timeLeft > 12)
            {
                projectile.alpha -= 1;
            }
            if (projectile.timeLeft == 12)
            {
                projectile.friendly = true;
                projectile.frame++;
                projectile.alpha = 20;
                SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 54).WithPitchVariance(1f), projectile.Center);
                if (sound != null)
                    sound.Pitch = -0.6f;

                for (int i = 0; i < 15; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                    Main.dust[dustIndex].color = new Color(0, 255, 150);
                    Main.dust[dustIndex].noGravity = false;
                }

                int totalHit = 0;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];

                    if (projectile.Hitbox.Intersects(npc.Hitbox))
                    {
                        totalHit++;
                        if (totalHit > 1)
                            break;
                    }
                }

                if (totalHit == 1)
                    projectile.damage *= 2;
            }
            if (projectile.timeLeft <= 12)
            {
                AnimateProjectile();
            }
            if (projectile.timeLeft == 11)
            {
                projectile.friendly = false;
            }
        }

        public void AnimateProjectile() // Call this every frame, for example in the AI method.
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3) // This will change the sprite every 8 frames (0.13 seconds). Feel free to experiment.
            {
                projectile.frame++;
                projectile.frame %= 5; // Will reset to the first frame if you've gone through them all.
                projectile.frameCounter = 0;
            }
        }
    }
}
