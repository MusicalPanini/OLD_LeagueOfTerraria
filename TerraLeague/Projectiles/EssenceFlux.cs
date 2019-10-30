using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class EssenceFlux : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Essence Flux");
        }

        public override void SetDefaults()
        {
            projectile.width = 21;
            projectile.height = 21;
            projectile.alpha = 30;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
        }

        public override void AI()
        {
            projectile.damage = 1;

            if (projectile.velocity.X < 0)
            {
                projectile.spriteDirection = -1;
            }
            projectile.rotation += (float)projectile.direction * 0.6f;

            Lighting.AddLight(projectile.position, 0.75f, 0.75f, 0);
            for (int i = 0; i < 3; i++)
            {
                int num345 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 159, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1.2f);
                Main.dust[num345].noGravity = true;
                Main.dust[num345].velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<EssenceFluxDebuff>(), 240);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int num345 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 159, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 50, default(Color), 2f);
                Main.dust[num345].noGravity = true;
                Main.dust[num345].velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            // For going through platforms and such, javelins use a tad smaller size
            width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return true;
        }
    }
}

