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
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
            {
                projectile.tileCollide = true;
            }

            if (projectile.soundDelay == 0)
                Main.PlaySound(SoundID.Item8, projectile.position);
            projectile.soundDelay = 10;

            //if (projectile.timeLeft < 590)
            //{
            //    projectile.friendly = true;
            //}

            if (projectile.velocity.X > 12)
                projectile.velocity.X = 12;
            else if (projectile.velocity.X < -12)
                projectile.velocity.X = -12;

            //for (int i = 0; i < 2; i++)
            //{
            //    Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 124, default(Color), 2.5f);
            //    dust2.noGravity = true;
            //    dust2.noLight = true;
            //    dust2.velocity *= 0.6f;
            //}

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 12, projectile.position.Y + 12);
                int dustBoxWidth = projectile.width - 24;
                int dustBoxHeight = projectile.height - 24;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 113, 0f, 0f, 124, default(Color), 2.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
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

            TerraLeague.DustBorderRing(projectile.width / 2, projectile.Center, 113, default, 3f);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 0, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;

                //dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, -4, 0, default(Color), 3f);
                //dust.velocity *= 1f;
                //dust.noGravity = true;
                //dust.color = new Color(0, 220, 220);
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
