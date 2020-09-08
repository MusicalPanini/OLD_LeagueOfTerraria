using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_FrostBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Bolt");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 60;
            projectile.magic = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            Dust dust;

            dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 137, projectile.velocity.X, projectile.velocity.Y, 100, default(Color), 1.5f);
            dust.noGravity = true;
            dust.noLight = true;
            dust.velocity *= 0.1f;

            for (int i = 0; i < 2; i++)
            {
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 229, projectile.velocity.X, projectile.velocity.Y, 124, default(Color), 1.25f);
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.velocity *= 0.6f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Slowed>(), 5 * 60);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 8, 1f);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            var sound = TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 10, 0);
            if (sound != null)
            {
                sound.Volume *= 0.5f;
            }
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
