using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class HeartoftheTempest_LightningRush : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Rush");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.timeLeft = 1000;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (player.active && modPlayer.lightningRush || projectile.timeLeft > 990)
                projectile.Center = player.MountedCenter;
            else
                projectile.Kill();

            Lighting.AddLight(projectile.Center, 0f, 1f, 1f);

            Dust dust = Dust.NewDustPerfect(projectile.Center, 226, null, 0, default(Color), 2);
            dust.noGravity = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
