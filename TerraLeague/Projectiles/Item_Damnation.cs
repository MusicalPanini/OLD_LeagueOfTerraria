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
    class Item_Damnation : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Damnation");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 6;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            Vector2 move = player.Center - projectile.Center;

            AdjustMagnitude(ref move);
            projectile.velocity = (10 * projectile.velocity + move) / 11f;
            AdjustMagnitude(ref projectile.velocity);

            Dust dust = Dust.NewDustPerfect(projectile.position, 156, null, 50, default(Color), 1f);
            dust.noGravity = true;
            dust.velocity *= 0;

            if (projectile.Hitbox.Intersects(player.Hitbox))
            {
                HitPlayer(player);
            }

        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 3f)
            {
                vector *= 3f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        public void HitPlayer(Player player)
        {
            projectile.netUpdate = true;
            player.AddBuff(BuffID.Swiftness, 180);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 156, 0, -2, 0);
                dust.noGravity = true;
            }

            projectile.Kill();
        }



        public override void Kill(int timeLeft)
        {
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
