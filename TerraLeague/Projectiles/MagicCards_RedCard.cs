﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MagicCards_RedCard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Card");
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.aiStyle = 2;
            projectile.friendly = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 0, 0));
            dust.noGravity = true;
            dust.scale = 1.4f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Dust dust;
            for (int i = 0; i < 24; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255, 0, 220);

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
            }

            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14, Terraria.Audio.SoundType.Sound), projectile.position);
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 115;
            projectile.height = 115;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }
    }
}
