using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MicroRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Micro-Rocket");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().summonAbility = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 130);
            dust.noGravity = true;
            Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);
            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 14), projectile.position);
            
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                Main.dust[dustIndex].velocity *= 0.5f;

            }
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3f;
                Main.dust[dustIndex].color = new Color(255, 0, 220);

                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].color = new Color(255, 0, 220);
                Main.dust[dustIndex].noGravity = true;

            }
        }
    }
}
