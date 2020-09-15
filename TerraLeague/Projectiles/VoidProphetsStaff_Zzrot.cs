using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class VoidProphetsStaff_Zzrot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zz'Rot");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabySpider);
            aiType = ProjectileID.BabySpider;
            projectile.minion = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.60f, 0f, 0.60f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, 27);
                dust.velocity *= 0.2f;
            }
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
