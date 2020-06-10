using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ArcaneEnergy_ShockingOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shocking Orb");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.localAI[0] = 1 - projectile.timeLeft / 90f;

            for (int i = 0; i < 2; i++)
            {
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, 0, 0, 124, default(Color), 2.5f);
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.velocity *= 0.6f;
            }
            if (projectile.timeLeft == 2)
                Prime();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            target.AddBuff(BuffType<Stunned>(), 60 + (int)(180 * projectile.localAI[0]));
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 53), projectile.position);

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
            projectile.width = 50 + (int)(250 * projectile.localAI[0]);
            projectile.height = 50 + (int)(250 * projectile.localAI[0]);
            
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}