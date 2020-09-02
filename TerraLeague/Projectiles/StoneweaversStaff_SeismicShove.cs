using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StoneweaversStaff_SeismicShove : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seismic Shove");
        }

        public override void SetDefaults()
        {
            projectile.width = 48*3;
            projectile.height = 48*3;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 90;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.rotation = MathHelper.PiOver4;
        }

        public override void AI()
        {

            if (projectile.timeLeft > 31)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, 16, 4, 0, -1, 100, new Color(255, 125, 0));

                if (projectile.soundDelay == 0 && projectile.type != 383)
                {
                    projectile.soundDelay = 20;
                    Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), projectile.position);
                }
                projectile.velocity = Vector2.Zero;

            }
            else if (projectile.timeLeft == 31)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 14, -0.5f);
            }
            else if (projectile.timeLeft <= 31 && projectile.timeLeft >= 29)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, projectile.oldVelocity.X * 0.25f, projectile.oldVelocity.Y * 0.25f, 0, new Color(255, 125, 0));
                }

                projectile.alpha = 0;

                projectile.friendly = true;

                projectile.velocity = new Vector2(Main.player[projectile.owner].position.X < projectile.position.X ? 20 : -20, -20);
            }
            else if (projectile.timeLeft <= 29)
            {


                projectile.friendly = false;

                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.velocity = Vector2.Zero;

            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.velocity = projectile.velocity/2;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;

            return true;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, projectile.velocity.X / 1.5f, projectile.velocity.Y / 1.5f, 100, default(Color), 1.5f);
                dustIndex.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
