﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class VoidArrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Warped Arrow");
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 1;
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.6f, 0, 0.6f);

            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, 0f, 0f, 0, default(Color), 1f);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.damage += projectile.damage / 4;

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(0, projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 27, projectile.velocity.X / 5, projectile.velocity.Y / 5, 100, default(Color), 0.7f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            // For going through platforms and such, javelins use a tad smaller size
            //width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

    }
}
