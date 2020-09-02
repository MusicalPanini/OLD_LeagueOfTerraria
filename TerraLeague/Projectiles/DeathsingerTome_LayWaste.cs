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
    class DeathsingerTome_LayWaste : ModProjectile
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
            projectile.penetrate = -1;
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
            Dust dust;
            if (num == 0)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 150);
                dust.noGravity = false;
                dust.alpha = projectile.alpha;
            }
            else if (num == 2)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                dust.velocity.X *= 0.3f;
                dust.color = new Color(0, 255, 0);
                dust.noGravity = false;
                dust.alpha = projectile.alpha;
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
                TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 54, -0.6f);

                for (int i = 0; i < 15; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0, -1, 150);
                    dust.color = new Color(0, 255, 150);
                    dust.noGravity = false;
                }

                int totalHit = 0;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (projectile.Hitbox.Intersects(npc.Hitbox) && npc.active && !npc.townNPC)
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

        public void AnimateProjectile() 
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frame++;
                projectile.frame %= 5;
                projectile.frameCounter = 0;
            }
        }
    }
}
