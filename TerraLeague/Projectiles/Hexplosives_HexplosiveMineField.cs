using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Hexplosives_HexplosiveMineField : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mine Clump");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.alpha = 0;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.3f;

            Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);
            projectile.rotation += projectile.velocity.X * 0.01f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 0.5f;
            }

            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.Bottom.Y - 10), new Vector2(6 - (i * 2), -5), ProjectileType<Hexplosives_HexplosiveMine>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != projectile.oldVelocity.X)
                projectile.velocity.X = -projectile.oldVelocity.X * 0.3f;
            else
                Prime();

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

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
