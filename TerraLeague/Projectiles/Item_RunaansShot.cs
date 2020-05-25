using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_RunaansShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runaan's Hurricane");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 900;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 12;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.5f, 0.45f, 0.30f);

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            Dust dust = Dust.NewDustPerfect(projectile.position, 204, Vector2.Zero, 0, default(Color), 0.75f);
            dust.noGravity = true;
            dust.alpha = 100;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
        }
    }
}
