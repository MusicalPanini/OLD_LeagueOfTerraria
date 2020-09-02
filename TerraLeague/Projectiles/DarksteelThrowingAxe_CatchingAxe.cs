using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarksteelThrowingAxe_CatchingAxe : ModProjectile
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
            projectile.penetrate = -1;
            projectile.aiStyle = 0;
            projectile.ranged = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 8;
                Main.PlaySound(SoundID.Item7, projectile.position);
            }

                projectile.spriteDirection = (int)projectile.ai[0];

                projectile.rotation += 0.5f * (int)projectile.ai[0];

            if (projectile.velocity.Y < 15)
            {
                projectile.velocity.Y += 0.3f;
            }
            Lighting.AddLight(projectile.position, 0.75f, 0, 0);
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 211, 0, 0, 0, new Color(255, 0, 0));
            dust.noGravity = true;
            dust.scale = 1.4f;

            if (projectile.timeLeft % 30 == 0)
            {
                Projectile.NewProjectile(projectile.Center, projectile.velocity, ProjectileType<DarksteelThrowingAxe_PathMarker>(), 0, 0, projectile.owner);
            }

            if (new Rectangle((int)projectile.position.X - 30, (int)projectile.position.Y - 30, 102, 102).Intersects(Main.player[projectile.owner].Hitbox) && projectile.timeLeft < 270)
            {
                Main.player[projectile.owner].AddBuff(BuffType<Buffs.SpinningAxe>(), 240);
                Main.PlaySound(SoundID.Grab, projectile.position);
                projectile.Kill();
            }

            base.AI();
        }


        public override void Kill(int timeLeft)
        {
            

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 24;
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
