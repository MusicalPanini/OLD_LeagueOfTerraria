using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class HexCoreStaff_ChaosStormZap : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Storm");
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.alpha = 255;
            projectile.timeLeft = 3;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            //projectile.extraUpdates = 100;
        }

        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 dustPos = projectile.position;
                dustPos -= projectile.velocity * ((float)i * 0.25f);
                Vector2 position125 = dustPos;
                Dust dust = Dust.NewDustDirect(position125, 1, 1, 160, 0f, 0f, 0, default(Color), (float)Main.rand.Next(70, 110) * 0.013f);
                dust.position = dustPos;
                dust.position.X += (float)(projectile.width / 2);
                dust.position.Y += (float)(projectile.height / 2);
                dust.velocity *= 0.2f;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return (target.whoAmI == (int)projectile.ai[0]);
        }
    }
}
