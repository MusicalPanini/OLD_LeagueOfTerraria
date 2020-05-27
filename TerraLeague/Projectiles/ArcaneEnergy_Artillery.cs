using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class ArcaneEnergy_Artillery : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Right of the Arcane");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft < 590)
            {
                projectile.friendly = true;
            }

            if (projectile.velocity.X > 12)
                projectile.velocity.X = 12;
            else if (projectile.velocity.X < -12)
                projectile.velocity.X = -12;

            if (projectile.timeLeft == 600)
                Main.PlaySound(SoundID.Item8, projectile.position);

            for (int i = 0; i < 2; i++)
            {
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 124, default(Color), 2.5f);
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.velocity *= 0.6f;
            }

            

            projectile.velocity.Y += 0.4f;


            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 89), projectile.position);

            Dust dust;
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 0, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 0, default(Color), 3f);
                dust.velocity *= 1f;
                dust.noGravity = true;
                dust.color = new Color(0, 220, 220);
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.friendly = true;

            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 200;
            projectile.height = 200;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 1;
        }
    }
}
