using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Crescendum_Sentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum Sentry");
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 64;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.minion = true;
            projectile.sentry = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Main.player[projectile.owner].ownedProjectileCounts[projectile.type] != 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
                    {
                        if (Main.projectile[i].timeLeft > projectile.timeLeft)
                        {
                            projectile.Kill();
                        }
                    }
                }
            }

            if ((int)projectile.ai[0] == -1)
            {
                projectile.ai[0] = TerraLeague.GetClosestNPC(projectile.Center, 700, projectile.position, 24, 24, -1, Main.player[projectile.owner].MinionAttackTargetNPC);
            }

            if ((int)projectile.ai[0] != -1)
            {
                NPC npc = Main.npc[(int)projectile.ai[0]];

                if (!npc.active || !Collision.CanHitLine(projectile.position, 24, 24, npc.position, npc.width, npc.height))
                {
                    projectile.ai[0] = -1;
                    return;
                }

                Player player = Main.player[projectile.owner];

                if (projectile.ai[1] > 5 && player.ownedProjectileCounts[ModContent.ProjectileType<Crescendum_SentryProj>()] <= player.maxMinions + 5)
                {
                    Projectile.NewProjectileDirect(projectile.Center, (projectile.Center - npc.Center).SafeNormalize(-Vector2.UnitY) * -16, ModContent.ProjectileType<Crescendum_SentryProj>(), projectile.damage, projectile.knockBack, projectile.owner);
                    projectile.ai[1] = 0;
                }
                else
                {
                    projectile.ai[1]++;
                }

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
    }
}
