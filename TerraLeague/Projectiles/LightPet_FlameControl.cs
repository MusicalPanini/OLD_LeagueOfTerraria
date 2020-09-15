using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class LightPet_FlameControl : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.timeLeft *= 5;
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.Center = player.MountedCenter;
            if (!player.active)
            {
                projectile.active = false;
                return;
            }
            if (player.HasBuff(ModContent.BuffType<CandlePet>()))
            {
                projectile.timeLeft = 2;
            }

            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                if (projectile.soundDelay == 0)
                {
                    Projectile.NewProjectileDirect(projectile.Center, new Vector2(Main.rand.NextFloat(0, 5f), 0).RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<LightPet_Flame>(), 0, 0, player.whoAmI);
                    projectile.soundDelay = Main.rand.Next(150, 300);
                }
            }
        }
    }
}
