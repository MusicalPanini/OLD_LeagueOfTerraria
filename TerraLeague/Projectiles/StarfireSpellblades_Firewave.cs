using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class StarfireSpellblades_Firewave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
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
            projectile.melee = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            Dust dust;
            if (projectile.soundDelay == 0 && (int)projectile.ai[0] == 1)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.Center);
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 34), projectile.Center);
            }
            projectile.soundDelay = 100;

            dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 87, 0, 0, 0, default(Color), 2f);
            dust.noGravity = true;
            dust.velocity = projectile.velocity * 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 60);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 8, 1f);
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
