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
    class TheFallenCelestialsDarkMagic_TormentedShadow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tormented Shadow");
        }

        public override void SetDefaults()
        {
            projectile.width = 200;
            projectile.height = 200;
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 150;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, Color.Purple.ToVector3());

            Dust dust;
            if (projectile.timeLeft == 300)
            {
                for (int i = 0; i < 80; i++)
                {
                    dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 248, 0, 0, 0, new Color(159, 0, 255), 2);
                    dust.noGravity = true;
                }
            }
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 5;
            }
            int num = Main.rand.Next(0, 2);
            if (num == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 248, 0, -1, 150);
                Main.dust[dustIndex].velocity.X *= 0.3f;
                Main.dust[dustIndex].color = new Color(159, 0, 255);
                Main.dust[dustIndex].noGravity = false;
            }
            if (projectile.timeLeft < 15)
                projectile.alpha += 5;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (int)(damage * (1 - (target.life / (float)target.lifeMax)));

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
