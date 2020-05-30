using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class Hexplosives_BouncingBomb : ModProjectile
    {
        int bounces = 3;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncing Bomb");
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.magic = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.rotation += projectile.velocity.X * 0.05f;
            Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);
            projectile.velocity.Y += 0.4f;

            Vector2 dustPos = projectile.position.RotatedBy(MathHelper.Pi + projectile.rotation, projectile.Center);

            Dust dust = Dust.NewDustPerfect(dustPos, DustID.Smoke);
            dust.noGravity = true;

            dust = Dust.NewDustPerfect(dustPos, DustID.Fire);
            dust.noGravity = true;
            dust.velocity *= 0;

            base.AI();
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Rebound();

            if (bounces <= 0)
                Prime();
            else
                Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);

            return false;
        }

        public void Rebound()
        {
            if (projectile.velocity.X != projectile.oldVelocity.X)
            {
                projectile.velocity.X = -projectile.oldVelocity.X;
            }
            else if (projectile.velocity.Y != projectile.oldVelocity.Y)
            {
                if (projectile.oldVelocity.Y > 0)
                {
                    //if (projectile.oldVelocity.Y < 6)
                        projectile.velocity.Y = -6;

                    if (projectile.oldVelocity.X > 6)
                        projectile.velocity.X = 6;
                    if (projectile.oldVelocity.X < -6)
                        projectile.velocity.X = -6;
                    //else if (projectile.oldVelocity.Y > 10)
                    //    projectile.velocity.Y = -10;
                    //else
                    //    projectile.velocity.Y *= -1f;

                    bounces--;
                }
                else
                {
                    projectile.velocity.Y = 6;
                }

                
            }
            if (bounces == 0)
            {
                Prime();
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.penetrate == 1)
            {
                Prime();
            }
            else
            {
                Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);

                Dust dust;
                for (int i = 0; i < 20; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                    dust.velocity *= 0.5f;

                }
                for (int i = 0; i < 50; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    dust.noGravity = true;
                    dust.velocity *= 3f;
                    dust.color = new Color(255, 0, 220);

                    dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    dust.color = new Color(255, 0, 220);
                    dust.noGravity = true;

                }
            }

        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 128;
            projectile.height = 128;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}
