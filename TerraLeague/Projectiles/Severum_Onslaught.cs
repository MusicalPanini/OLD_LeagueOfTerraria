using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Severum_Onslaught : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Onslaught");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.timeLeft = 240;
            projectile.melee = true;
            projectile.extraUpdates = 60;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if ((int)projectile.ai[1] == 1)
            {
                NPC target = Main.npc[(int)projectile.ai[0]];
                if (!projectile.Hitbox.Intersects(target.Hitbox))
                {
                    projectile.Kill();
                }
            }

            Dust dust = Dust.NewDustPerfect(projectile.Center, 235/*182*/, Vector2.Zero);
            dust.noGravity = true;
            dust.velocity *= 0;
            dust.noLight = true;
            dust = Dust.NewDustPerfect(projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero), 235, Vector2.Zero);
            dust.noGravity = true;
            dust.velocity *= 0;
            dust.noLight = true;

            //Lighting.AddLight(projectile.Center, 1f, 0.0f, 0f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.ai[1] = 1;
            projectile.penetrate = 2;
            Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += 1;
            //Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0, projectile.owner, projectile.owner, 1);
            projectile.friendly = false;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)projectile.ai[0])
            {
                return base.CanHitNPC(target);
            }
            return false;

        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
