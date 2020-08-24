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
    class EmperoroftheSands_EmperorsDivide : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emperor's Divide");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 238;
            projectile.timeLeft = 90;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (projectile.timeLeft == 90)
                projectile.friendly = true;
            if(projectile.velocity.Length() > 0 && projectile.timeLeft <= 75)
            {
                projectile.velocity.X *= .8f;
                projectile.velocity.Y *= .8f;
            }

            projectile.knockBack = 30 * (projectile.velocity.Length() / 16);
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 32, 0, 0, projectile.alpha);
            }
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
        }
    }
}
