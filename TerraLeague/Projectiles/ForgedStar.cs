using Microsoft.Xna.Framework;
using System.Linq;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
	public class ForgedStar : ModProjectile
	{
        Vector2 offset = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forged Star");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 3;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.netImportant = true;
            projectile.netUpdate = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 3)
            {
                if ((int)projectile.ai[0] != 1)
                {
                    projectile.ai[1] = Main.projectile.Where(x => (int)x.ai[0] == 1 && x.owner == projectile.owner).First().whoAmI;
                }
                else
                {
                    projectile.ai[1] = 0;
                }
            }

            Player player = Main.player[projectile.owner];
            if (player.HasBuff(BuffType<CenterOfTheUniverse>()))
            {
                projectile.timeLeft = 2;
            }

            if (player.HasBuff(BuffType<CelestialExpansion>()) && offset.X < 300)
            {
                offset.X += 5;
            }
            else if (!player.HasBuff(BuffType<CelestialExpansion>()) && offset.X > 150)
            {
                offset.X -= 5;
            }
            else if (!player.HasBuff(BuffType<CelestialExpansion>()) && offset.X < 150)
            {
                offset.X += 5;
            }

            if ((int)projectile.ai[0] != 1)
            {
                float angle = Main.projectile[(int)projectile.ai[1]].ai[1];
                int numOfProj = Main.player[projectile.owner].ownedProjectileCounts[ProjectileType<ForgedStar>()];
                projectile.Center = player.Center + offset.RotatedBy(angle + ((MathHelper.TwoPi * ((int)projectile.ai[0] - 1)) /numOfProj));
            }
            else
            {
                projectile.ai[1] += .035f;
                projectile.Center = player.Center + offset.RotatedBy(projectile.ai[1]);
            }

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 111, projectile.velocity.X, projectile.velocity.Y, 200, default(Color), 1.5f);
            dust.noGravity = true;
            dust.noLight = true;
            dust.velocity *= 0.1f;

            for (int i = 0; i < 2; i++)
            {
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 162, projectile.velocity.X, projectile.velocity.Y, 124, default(Color), 2.5f);
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.velocity *= 0.6f;
            }


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
           
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Main.dust[Dust.NewDust(projectile.position, 16, 16, 92, 0, 0, 50, new Color(255, 180, 0), 1.2f)];
                dust.noGravity = true;
                dust.noLight = true;
            }

            if ((int)projectile.ai[0] == 1)
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.projectile[i].type == projectile.type && Main.projectile[i].owner == projectile.owner && projectile.whoAmI != i)
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
        }
    }
}
