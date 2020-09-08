using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class HextechWrench_StormGrenade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CH-2 Electron Storm Grenade");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                projectile.rotation = Main.rand.NextFloat(0, 6.282f);
            projectile.soundDelay = 100;

            Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0,0,0, new Color(0,255, 255));

            projectile.rotation += projectile.velocity.X * 0.05f;

            projectile.velocity.Y += 0.4f;

            if(projectile.velocity.X > 8)
                projectile.velocity.X = 8;
            else if(projectile.velocity.X < -8)
                projectile.velocity.X = -8;

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
            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);
            Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);

            Dust dust;
            for (int i = 0; i < 10; i++)
            {
                dust = Dust.NewDustDirect(projectile.Center, 1, 1, 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity *= 2f;
            }
            for (int i = 0; i < 40; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(0, 220, 220), 1f);
                dust.noGravity = true;
                dust.velocity = (dust.position - projectile.Center) * 0.1f;
            }
            int effectRadius = 75/2;
            for (int i = 0; i < effectRadius / 2; i++)
            {
                Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 2f)))) + projectile.Center;

                Dust dustR = Dust.NewDustPerfect(pos, 261, Vector2.Zero, 0, new Color(0, 220, 220), 2);
                dustR.noGravity = true;
                dustR.velocity = (dustR.position - projectile.Center) * 0.1f;
            }

            for (int i = 0; i < 30; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0, 0, 0, new Color(0, 220, 220), 0.6f);
                dust.velocity = (dust.position - projectile.Center) * 0.075f;
                dust.velocity.Y -= 1f;
                dust.noLight = true;
            }

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
            projectile.alpha = 255;
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
