using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class AtlasGauntlets_ExcessiveForce : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Excessive Force");
        }

        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 15;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 3;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 60;
            projectile.extraUpdates = 4;
            //projectile.usesLocalNPCImmunity = true;
            //projectile.localNPCHitCooldown = -1;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Dust dust;
            if (projectile.soundDelay == 0)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), projectile.Center);
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Smoke, 0, 0, 0, default(Color), 1f);
                dust.noGravity = true;
                dust.noLight = true;
            }
            projectile.soundDelay = 100;

            dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Smoke, 0, 0, 0, default(Color), 2f);
            dust.noGravity = true;
            dust.noLight = true;

            if (Main.rand.Next(0, 3) == 0)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Fire, 0, 3, 0, default(Color), 1f);
                //dust.noLight = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return false;
            return base.CanHitNPC(target);
        }
    }
}
