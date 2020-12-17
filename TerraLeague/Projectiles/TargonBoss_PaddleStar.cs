using Microsoft.Xna.Framework;
using TerraLeague.Dusts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using TerraLeague.NPCs;

namespace TerraLeague.Projectiles
{
    class TargonBoss_PaddleStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Paddle Star");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.netImportant = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, TargonBoss.ZoeColor.ToVector3());
            if (Main.rand.Next(0, 2) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 1, TargonBoss.ZoeColor, 2);
                dust.velocity *= 0f;
                dust.noGravity = true;
            }

            projectile.rotation += 0.02f * projectile.velocity.Length();

            base.AI();
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            //if (Main.rand.Next(0, Main.expertMode ? 4 : 5) == 0)
            //    target.AddBuff(BuffID.Confused, 60);
            base.ModifyHitPlayer(target, ref damage, ref crit);
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
                projectile.velocity.Y = -projectile.oldVelocity.Y;
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustType<Smoke>(), 0f, 0f, 150, new Color(255, 50, 255));
            //    dust.velocity *= 1f;
            //}
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, projectile.velocity.X, projectile.velocity.Y, 1, TargonBoss.ZoeColor);
                dust.noGravity = true;
                base.Kill(timeLeft);
            }
        }

    }
}
