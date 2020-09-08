using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EchoingFlameCannon_EchoingFlames : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoing Flames");
        }

        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 15;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 45;
            projectile.ranged = true;
            projectile.extraUpdates = 4;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 74));
            }
            projectile.soundDelay = 100;

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75, 0, 0, 0, default(Color), 4f);
            dust.noGravity = true;
            dust.noLight = true;

            if (Main.rand.Next(0, 3) == 0)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75, 0, 3, 0, default(Color), 1f);
                //dust.noLight = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 1200);

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
            return base.CanHitNPC(target);
        }
    }
}
