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
    class HeartoftheTempest_SlicingMaelstrom : ModProjectile
    {
        int framecount2 = 29;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slicing Maelstrom");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 150;
            projectile.height = 150;
            projectile.timeLeft = 150;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
        }

        public override void AI()
        {
            if (Main.projectile[(int)projectile.ai[0]].active)
                projectile.Center = Main.projectile[(int)projectile.ai[0]].Center;
            else
                projectile.Kill();

            Lighting.AddLight(projectile.Center, 0f, 1f, 1f);
            projectile.rotation += 0.05f;

            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0, 0, 0, new Color(0, 255, 255), 1f);
            dust.noGravity = true;

            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            framecount2++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frame++;
                projectile.frame %= 4; 
                projectile.frameCounter = 0;
            }
            if (framecount2 >= 30)
            {
                framecount2 = 0;
                TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 53, 0.25f);
            }
        }
    }
}
