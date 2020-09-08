using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Infernum_FlameSpread : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernum");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 60;
            projectile.ranged = true;
            projectile.extraUpdates = 4;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, 0.5f);
            }
            projectile.soundDelay = 100;

            if (Main.rand.Next(0, 1) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 88, 0, 0, 0, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;

                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 88, 0, 0, 0, default(Color), 0.75f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<InfernumMark>(), 60 * 5);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((int)projectile.ai[1] == 1)
                crit = true;
            else
                crit = false;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
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
            if (target.whoAmI == (int)projectile.ai[0])
                return false;
            return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
