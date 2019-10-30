using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class StormNade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CH-2 Electron Storm Grenade");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().summonAbility = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 600)
                projectile.rotation = Main.rand.Next(0, 360);

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0,0,0, new Color(0,255, 255));

            projectile.rotation += projectile.velocity.X * 0.05f;//(int)(Math.Tan(projectile.velocity.X / -projectile.velocity.Y) * 180 / Math.PI);

            projectile.velocity.Y += 0.4f;
            if(projectile.velocity.X > 8)
            {
                projectile.velocity.X = 8;

            }
            else if(projectile.velocity.X < -8)
            {
                projectile.velocity.X = -8;

            }


            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Stunned>(), 60);
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }


        public override void Kill(int timeLeft)
        {
            //Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), ProjectileType("ExplosionCollision"), (int)(projectile.damage), 10, Main.player[projectile.owner].whoAmI);
            // Play explosion sound
            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);
            Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 255, 0, 150), 1f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 255, 0, 150), 1f);
                dust2.velocity *= 3f;
                dust2.color = new Color(0, 220, 220);
            }
            // reset size to normal width and height.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            base.Kill(timeLeft);
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 115;
            projectile.height = 115;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }

    }
}
