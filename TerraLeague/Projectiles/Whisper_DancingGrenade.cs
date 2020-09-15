using Microsoft.Xna.Framework;
using TerraLeague.Dusts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    class Whisper_DancingGrenade : ModProjectile
    {
        int bounces = 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dancing Grenade");
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 14;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.netImportant = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                projectile.rotation = Main.rand.NextFloat(0, 6.282f);
            projectile.soundDelay = 100;

            Lighting.AddLight(projectile.position, 1f * projectile.ai[0], 0.5f * projectile.ai[0], 0.9f * projectile.ai[0]);
            Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustType<Smoke>(),0,0,(int)(255 - (255 * projectile.ai[0])), new Color(255,50,255));

            projectile.rotation += projectile.velocity.X * 0.05f;

            projectile.velocity.Y += 0.4f;

            if(projectile.velocity.X > 8)
                projectile.velocity.X = 8;
            else if(projectile.velocity.X < -8)
                projectile.velocity.X = -8;

            base.AI();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (target.statLife <= 0)
            {
                projectile.ai[0] += 0.3f;
                projectile.damage = (int)(projectile.damage * 1.44f);
                projectile.netUpdate = true;
            }
            base.OnHitPlayer(target, damage, crit);
        }
        
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //if (target.life <= 0)
            {
                projectile.ai[0] += 0.3f;
                projectile.damage = (int)(projectile.damage * 1.44f);
                projectile.netUpdate = true;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Rebound();

            Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
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
                    projectile.velocity.Y = -8;
                    bounces--;
                }
                else
                {
                    projectile.velocity.Y = 8;
                }
            }

            if (bounces == 0)
            {
                projectile.Kill();
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustType<Smoke>(), 0f, 0f, 150, new Color(255, 50, 255));
                    dust.velocity *= 1f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustType<Smoke>(), 0f, 0f, 100, new Color(255, 50, 255));
            dust.velocity *= 1f;
            base.Kill(timeLeft);
        }

    }
}
