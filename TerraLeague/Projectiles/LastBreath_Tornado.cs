using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class LastBreath_Tornado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tornado");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.timeLeft = 90;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1.5f;
            aiType = 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.Y = -12;
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss)
            {
                target.velocity.Y = -12;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if(projectile.velocity.Length() > 0)
            {
                projectile.velocity.X *= .98f;
                projectile.velocity.Y *= .98f;
            }
            

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 16);
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frame++;
                projectile.frame %= 6; 
                projectile.frameCounter = 0;
            }
        }
    }
}
