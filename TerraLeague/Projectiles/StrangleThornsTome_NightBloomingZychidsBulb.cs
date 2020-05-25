using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StrangleThornsTome_NightBloomingZychidsBulb : ModProjectile
    {
        bool grounded = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Night Blooming Zychids Bulb");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 34;
            projectile.timeLeft = 3600;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.minion = true;
            projectile.scale = 0;
        }

        public override void AI()
        {
            projectile.rotation = 0;
            AnimateProjectile();
            if (!grounded)
            {
                grounded = true;
                projectile.velocity.Y = 0f;
            }
            else
            {
                if (projectile.scale < 1)
                {
                    projectile.scale += 0.05f;
                    projectile.position.Y -= 34 * 0.05f;
                }

                projectile.velocity = Vector2.Zero;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;

            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.Y <= 0)
            {
                grounded = true;
            }

            return false;
        }

        
        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 20)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }
    }
}
