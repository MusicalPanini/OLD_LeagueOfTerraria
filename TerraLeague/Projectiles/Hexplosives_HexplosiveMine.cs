using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Hexplosives_HexplosiveMine : ModProjectile
    {
        bool grounded = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexplosive Mine");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 60 * 10;
            projectile.penetrate = 100;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.velocity.Y < 0)
                projectile.tileCollide = false;
            else
                projectile.tileCollide = true;

            if (!grounded)
            {
                projectile.velocity.Y += 0.3f;
                projectile.friendly = false;
            }
            else
            {
                projectile.velocity = Vector2.Zero;
                projectile.friendly = true;
            }

            Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);
            projectile.rotation += projectile.velocity.X * 0.01f;

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
                for (int i = 0; i < 30; i++)
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.Y <= 0)
                grounded = true;
            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
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
