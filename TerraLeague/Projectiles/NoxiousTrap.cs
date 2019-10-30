using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class NoxiousTrap : ModProjectile
    {
        bool grounded = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noxious Trap");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 32;
            projectile.timeLeft = 18000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.minion = true;
            projectile.scale = 1.2f;
        }

        public override void AI()
        {
            projectile.rotation = 0;

            if (projectile.alpha >= 60)
            {
                Rectangle area = new Rectangle((int)projectile.Center.X - 45, (int)projectile.Center.Y - 45, projectile.width + 90, projectile.height + 90);

                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];

                    if (!npc.townNPC && npc.active)
                    {
                        if (npc.Hitbox.Intersects(area))
                        {
                            Prime();
                            break;
                        }
                    }
                }
            }

            if (!grounded)
            {
                projectile.velocity.Y += 0.3f;
            }
            else
            {
                if (projectile.alpha < 100)
                    projectile.alpha++;

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
            //Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), ProjectileType("ExplosionCollision"), (int)(projectile.damage), 10, Main.player[projectile.owner].whoAmI);
            // Play explosion sound
            var efx = Main.PlaySound(new LegacySoundStyle(2, 102), projectile.position);
            if (efx != null)
                efx.Pitch = -1;

            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].noGravity = true;

                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0f, 0f, 100, default(Color), 1f);
            }

            if (projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(20, 31);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<NoxiousCloud>(), projectile.damage, 0, projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            // reset size to normal width and height.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            base.Kill(timeLeft);
            base.Kill(timeLeft);
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
                grounded = true;

            return false;
        }

        public void Prime()
        {
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 150;
            projectile.height = 150;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            projectile.Kill();
        }
    }
}
