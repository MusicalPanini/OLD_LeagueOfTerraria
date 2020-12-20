using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class RadiantStaff_FinalSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Final Spark");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.timeLeft = 1000;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.hide = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().illuminated)
                damage *= 2;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI()
        {
            if ((int)projectile.ai[1] == 0)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 4;
                }
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }

                if (projectile.timeLeft == 1000)
                {
                    projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 13, -1f);
                }

                projectile.velocity = Vector2.Zero;
                projectile.Center = Main.player[projectile.owner].Center;

                if (projectile.timeLeft == 940)
                {
                    projectile.ai[1] = 1;
                    projectile.velocity = new Vector2(0, -10).RotatedBy(projectile.rotation);
                    projectile.friendly = true;
                    projectile.extraUpdates = 40;
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 72, -1f);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center - (Vector2.One * 8), 16, 16, 228, 0, 0, 0, default(Color), 3f - (3f* (projectile.alpha/255f)));
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
    }

    public class FinalSparkGlobalNPC : GlobalNPC
    {
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.HasBuff(BuffType<Buffs.Illuminated>()))
            {
                if (projectile.type == ProjectileType<RadiantStaff_FinalSpark>())
                    npc.DelBuff(npc.FindBuffIndex(BuffType<Buffs.Illuminated>()));
            }
        }
    }
}
