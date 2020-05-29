using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class EyeofGod_TentacleHitbox : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tentacle Smash");
        }

        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 144;
            projectile.alpha = 255;
            projectile.timeLeft = 1;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.minion = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Dust dust;
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.Center.Y + 48), projectile.width, 2, 31, 0f, -3f, 100, default(Color), 1f);
                dust.velocity *= 0.5f;
            }
            for (int i = 0; i < 10; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.Center.Y + 48), projectile.width, 2, 59, 0f, -6f, 200, new Color(0, 255, 201), 2f);
                dust.noGravity = true;
                dust.velocity.Y -= 3f;
                dust.noLight = true;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.Center.Y + 48), projectile.width, 2, 59, 0f, -6f, 200, new Color(0, 255, 201), 3f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }
    }
}
