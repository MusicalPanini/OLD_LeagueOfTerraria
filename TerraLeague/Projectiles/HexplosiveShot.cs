using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HexplosiveShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosive");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 1000;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            //Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 130);
            //dust.noGravity = true;

            projectile.velocity.Y += 0.3f;

            Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);
            projectile.rotation += projectile.velocity.X * 0.01f;

            Vector2 dustPos = projectile.position.RotatedBy(MathHelper.PiOver2 + projectile.rotation, projectile.Center);

            Dust dust = Dust.NewDustPerfect(dustPos, DustID.Smoke);
            dust.noGravity = true;
            dust.scale = 0.75f;

            dust = Dust.NewDustPerfect(dustPos, DustID.Fire);
            dust.noGravity = true;
            dust.velocity *= 0;
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
                // Smoke Dust spawn
                for (int i = 0; i < 20; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndex].velocity *= 0.5f;

                }
                // Fire Dust spawn
                for (int i = 0; i < 30; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 3f;
                    Main.dust[dustIndex].color = new Color(255, 0, 220);

                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].color = new Color(255, 0, 220);
                    Main.dust[dustIndex].noGravity = true;

                }
            }
            
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 64;
            projectile.height = 64;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}
