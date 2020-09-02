using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_SpinningAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Throwing Axe");
        }

        public override void SetDefaults()
        {
            projectile.width = 78;
            projectile.height = 78;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.aiStyle = 0;
            projectile.friendly = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 12;
                Main.PlaySound(SoundID.Item7, projectile.position);
            }

            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;

            projectile.rotation += 0.5f * projectile.spriteDirection;

            Lighting.AddLight(projectile.position, 0.75f, 0, 0);
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 211, 0, 0, 0, new Color(255, 0, 0), 1.4f);
            dust.noGravity = true;

            if (projectile.timeLeft < 270 && projectile.velocity.Y < 15)
                projectile.velocity.Y += 0.8f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float distance = Main.player[projectile.owner].position.X - projectile.position.X;
            Projectile.NewProjectileDirect(projectile.oldPosition, new Vector2((distance * 0.013f) + (Main.player[projectile.owner].velocity.X * 0.6f), -12), ProjectileType<DarksteelThrowingAxe_CatchingAxe>(), 0, 0, projectile.owner, projectile.spriteDirection);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 8, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 8, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 4, -0.5f);
            return true;
        }
    }
}
