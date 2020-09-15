using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class AssassinsKunai_ShroudSmoke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Shroud");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.minion = true;
            projectile.scale = 2f;
            base.SetDefaults();
        }

        public override void AI()
        {
            projectile.tileCollide = false;
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 7*60f)
            {
                projectile.alpha += 10;
            }
            if (projectile.alpha >= 255)
            {
                projectile.Kill();
                projectile.alpha = 255;
            }

            projectile.rotation += projectile.velocity.X * 0.1f;
            projectile.rotation += (float)projectile.direction * 0.003f;
            projectile.velocity *= 0.96f;
            Rectangle projectileHitBox = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            for (int i = 0; i < 1000; i++)
            {
                if (i != projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].type == projectile.type)
                {
                    Rectangle targetHitBox = new Rectangle((int)Main.projectile[i].position.X, (int)Main.projectile[i].position.Y, Main.projectile[i].width, Main.projectile[i].height);
                    if (projectileHitBox.Intersects(targetHitBox))
                    {
                        Vector2 vector77 = Main.projectile[i].Center - projectile.Center;
                        if (vector77.X == 0f && vector77.Y == 0f)
                        {
                            if (i < projectile.whoAmI)
                            {
                                vector77.X = -1f;
                                vector77.Y = 1f;
                            }
                            else
                            {
                                vector77.X = 1f;
                                vector77.Y = -1f;
                            }
                        }
                        vector77.Normalize();
                        vector77 *= 0.005f;
                        projectile.velocity -= vector77;
                        Projectile projectile2 = Main.projectile[i];
                        projectile2.velocity += vector77;
                    }
                }
            }

            Player player = Main.player[projectile.owner];
            if (projectile.Hitbox.Intersects(player.Hitbox))
            {
                player.AddBuff(BuffType<Shrouded>(), 2);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
