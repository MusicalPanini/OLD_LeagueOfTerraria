using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_Spear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyfall of Areion");
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 30;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.timeLeft = 240;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, TargonBoss.PanthColor.ToVector3() * (1 - (projectile.alpha / 255f)));

            if (projectile.ai[0] == 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                //projectile.ai[1]++;
                //if ((int)projectile.ai[1] == 90)
                //    Prime();

                if (projectile.timeLeft < 51)
                {
                    projectile.alpha += 5;
                }
            }

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            projectile.velocity *= 0;
            projectile.position += oldVelocity;
            projectile.ai[0] = 1;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            //TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 14, 0);

            //TerraLeague.DustBorderRing(projectile.width / 2, projectile.Center, 6, default, 2);
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 1280;
            projectile.height = 1280;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}
