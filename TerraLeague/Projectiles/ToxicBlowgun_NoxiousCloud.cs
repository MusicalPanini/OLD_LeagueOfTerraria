using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class ToxicBlowgun_NoxiousCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noxious Trap");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.scale = 1.5f;
            base.SetDefaults();
        }

        public override void AI()
        {
            projectile.tileCollide = false;
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 60f)
            {
                projectile.ai[0] += 10f;
            }
            if (projectile.ai[0] > 255f)
            {
                projectile.Kill();
                projectile.ai[0] = 255f;
            }
            projectile.alpha = (int)(100.0 + (double)projectile.ai[0] * 0.7);
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
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 240);
            base.OnHitNPC(target, damage, knockback, crit);
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
