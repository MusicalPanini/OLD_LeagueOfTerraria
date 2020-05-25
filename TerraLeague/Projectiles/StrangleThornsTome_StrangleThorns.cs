using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Projectiles
{
    public class StrangleThornsTome_StrangleThorns : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strangle Thorns");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.minion = true;
        }

        public override void AI()
        {
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].type == ProjectileType<StrangleThornsTome_NightBloomingZychidsBulb>())
                    {
                        Projectile bulb = Main.projectile[i];

                        if (bulb.Hitbox.Intersects(new Rectangle((int)projectile.Center.X - 75, (int)projectile.Center.Y - 75, 150, 150)))
                        {
                            Projectile.NewProjectileDirect(bulb.position, Vector2.Zero, ProjectileType<HextechWrench_EvolutionTurret>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
                            bulb.Kill();
                        }
                    }
                }
            }
            projectile.velocity = Vector2.Zero;

            if (projectile.ai[0] == 0f)
            {
                projectile.alpha -= 75;

                if (projectile.alpha <= 0)
                {
                    projectile.alpha = 0;
                    projectile.ai[0] = 1f;
                    if (projectile.ai[1] == 0f)
                    {
                        projectile.ai[1] += 1f;
                        projectile.position += projectile.velocity * 1f;
                    }
                    if (projectile.type == ProjectileType<StrangleThornsTome_StrangleThorns>() && Main.myPlayer == projectile.owner)
                    {
                        int num49 = projectile.type;
                        if (projectile.ai[1] >= (float)(9 + Main.rand.Next(2)))
                        {
                            num49 = ProjectileType<StrangleThornsTome_StrangleThornsEnd>();
                        }
                        int number = Projectile.NewProjectile(projectile.Center.X + new Vector2(0, -32).RotatedBy(projectile.rotation).X, projectile.Center.Y + new Vector2(0, -32).RotatedBy(projectile.rotation).Y, projectile.velocity.X + new Vector2(0, -32).RotatedBy(projectile.rotation).X, projectile.velocity.Y + new Vector2(0, -32).RotatedBy(projectile.rotation).Y, num49, projectile.damage, projectile.knockBack, projectile.owner, 0f, projectile.ai[1] + 1f);
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, number, 0f, 0f, 0f, 0, 0, 0);
                        return;
                    }
                }
            }
            else
            {
                if (projectile.alpha < 170 && projectile.alpha + 5 >= 170)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDust(projectile.position, projectile.width, projectile.height, 18, projectile.velocity.X * 0.025f, projectile.velocity.Y * 0.025f, 170, default(Color), 1.2f);
                    }
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 14, 0f, 0f, 170, default(Color), 1.1f);
                }

                projectile.alpha += 7;

                if (projectile.alpha >= 255)
                {
                    projectile.Kill();
                    return;
                }
            }

            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Buffs.Seeded>(), 5 * 60);
        }
    }
}
