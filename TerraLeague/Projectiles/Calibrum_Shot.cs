using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Calibrum_Shot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Calibrum");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.timeLeft = 900;
            projectile.ranged = true;
            projectile.extraUpdates = 90;
        }

        public override void AI()
        {
            if (projectile.timeLeft < 896)
            {
                Dust dust = Dust.NewDustPerfect(projectile.position, 111, Vector2.Zero, 0, default(Color), projectile.timeLeft <= 575 ? 1.5f : 1);
                dust.noGravity = true;
                dust.alpha = 100;
            }

            if (projectile.timeLeft == 575)
            {
                projectile.damage *= 2;
                for (int i = 0; i < 36; i++)
                {
                    float XRad = 10;
                    float YRad = 20;
                    float rotation = projectile.velocity.ToRotation();
                    float time = MathHelper.TwoPi * i / 36;

                    double X = XRad * System.Math.Cos(time) * System.Math.Cos(rotation) - YRad * System.Math.Sin(time) * System.Math.Sin(rotation);
                    double Y = XRad * System.Math.Cos(time) * System.Math.Sin(rotation) + YRad * System.Math.Sin(time) * System.Math.Cos(rotation);

                    Vector2 pos = new Vector2((float)X, (float)Y) + projectile.Center;

                    Dust dust = Dust.NewDustPerfect(pos, 111, Vector2.Zero, 0, default(Color), 1.5f);
                    dust.noGravity = true;
                }
            }

            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().SyncProjectileKill(projectile);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 1)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 122), projectile.Center);
                for (int i = 0; i < 8; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 8, 8, 112, 0, 0, 0, new Color(59, 0, 255), 1f);
                }
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
