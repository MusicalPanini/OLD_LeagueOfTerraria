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
    public class DarksteelDagger_Dagger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Dagger");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.timeLeft = 180;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            if (projectile.ai[0] > 0)
                projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;

            projectile.spriteDirection = projectile.direction;

            if (projectile.timeLeft < 150 && (int)projectile.ai[1] == 0)
            {
                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.97f;
                projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;
            }
            else if ((int)projectile.ai[1] > 0)
            {
                projectile.velocity.Y += 0.4f;
                projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;

            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            
            if ((int)projectile.ai[1] == 2)
            {
                    projectile.velocity = new Vector2(projectile.velocity.X * 0.2f, -6);

                projectile.ai[1] = 1;
            }

            if (projectile.velocity.Y > 16)
                projectile.velocity.Y = 16;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] == 0)
            {
                projectile.netUpdate = true;
                projectile.ai[1] = 2;
                projectile.timeLeft += 30;
            }
            else
            {
                target.immune[projectile.owner] = 2;
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            for (int i = 0; i < 6; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 8, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
