using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class NezuksGauntlet_EssenceFlux : ModProjectile
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
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 159, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<EssenceFluxDebuff>(), 240);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 159, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 50, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}

