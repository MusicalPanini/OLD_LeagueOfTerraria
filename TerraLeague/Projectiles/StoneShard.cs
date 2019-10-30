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
    class StoneShard : ModProjectile
    {
        Vector2[] stonePos = { new Vector2(-24,-24), new Vector2(24,-16), new Vector2(0,-8), new Vector2(16, -32), new Vector2(-8,-32) };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Threaded Volley");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.alpha = 150;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if ((int)projectile.ai[1] == 0)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 10;
                }
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }

                if (projectile.timeLeft == 1000)
                    projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

                projectile.velocity = Vector2.Zero;

                projectile.Center = new Vector2(Main.player[projectile.owner].Center.X + stonePos[(int)projectile.ai[0]].X, Main.player[projectile.owner].Center.Y + stonePos[(int)projectile.ai[0]].Y + (16 * (projectile.alpha / 255f)));

                if (projectile.timeLeft == 970 - ((int)projectile.ai[0] * 20))
                {
                    projectile.ai[1] = 1;
                    projectile.velocity = new Vector2(0, -20).RotatedBy(projectile.rotation);
                    projectile.friendly = true;
                    projectile.tileCollide = true;
                    SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound), projectile.Center);
                    if (sound != null)
                        sound.Pitch = -1;

                }
            }
            else
            {
                    Dust dust = Terraria.Dust.NewDustDirect(projectile.position, 16, 16, 4, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);

            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 4, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
                Main.dust[dustIndex].velocity *= 1.5f;

                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 4, 0f, 0f, 100, new Color(255, 125, 0), 1f);

            }

            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();

            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.timeLeft = 3;
            projectile.velocity = Vector2.Zero;
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 60;
            projectile.height = 60;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }
    }
}
