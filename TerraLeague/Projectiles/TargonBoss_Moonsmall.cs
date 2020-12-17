using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TargonBoss_Moonsmall : ModProjectile
    {
        int effectRadius = 16 * 5;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonfall");
        }

        public override void SetDefaults()
        {
            effectRadius = 16 * 10;
            projectile.width = effectRadius*2;
            projectile.height = effectRadius * 2;
            projectile.timeLeft = (150);
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            TerraLeague.DustBorderRing((int)(effectRadius), projectile.Center, 263, TargonBoss.DianaColor, 1);

            if (projectile.timeLeft % 30 == 1)
            {
                float rad = effectRadius - (effectRadius * projectile.timeLeft / 150f);
                //TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 15, 0f - (0.05f * projectile.timeLeft / 30));
                TerraLeague.DustElipce(rad, rad, 0, projectile.Center, 263, TargonBoss.DianaColor, 1.5f, 180, true);
            }

            if (projectile.timeLeft == 1)
                projectile.hostile = true;
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustElipce(2, 2, 0, projectile.Center, 263, TargonBoss.DianaColor, 1f, 180, true, 10);
            Main.PlaySound(new LegacySoundStyle(2, 74), projectile.Center);

            base.Kill(timeLeft);
        }

        public override bool CanHitPlayer(Player target)
        {
            if (!TerraLeague.IsHitboxWithinRange(projectile.Center, target.Hitbox, effectRadius))
                return false;
            return base.CanHitPlayer(target);
        }
    }
}
