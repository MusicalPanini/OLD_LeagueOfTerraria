using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class DemacianStandard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demacian Standard");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 64;
            projectile.timeLeft = 60*8;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.75f, 0.75f, 0.75f);

            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
            {
                projectile.tileCollide = true;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 229, 0, 3, 0, new Color(0, 0, 255), 1f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            for (int i = 0; i < Main.player.Length; i++)
            {
                Player target = Main.player[i];

                if (projectile.Distance(target.Center) < 500 && target.active)
                {
                    target.AddBuff(BuffType<ForDemacia>(), 2);
                }
            }

            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            projectile.friendly = false;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 4; 
                projectile.frameCounter = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.friendly = false;
            return false;
        }
    }
}
