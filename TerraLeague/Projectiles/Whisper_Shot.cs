using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Whisper_Shot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whisper");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 360;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.extraUpdates = 24;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Custom/WhisperShot"), projectile.position);
            projectile.soundDelay = 100;

            Lighting.AddLight(projectile.Left, 1f, 0.5f, 0.01f);

            if (projectile.timeLeft < 354)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 75, Vector2.Zero, 0, new Color(255, 0, 0));
                dust.noGravity = true;
                dust.velocity *= 0;
                dust = Dust.NewDustPerfect(projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero), 75, Vector2.Zero, 0, new Color(255, 0, 0));
                dust.noGravity = true;
                dust.velocity *= 0;
            }

            //if (projectile.alpha > 0)
            //    projectile.alpha -= 15;
            //if (projectile.alpha < 0)
            //    projectile.alpha = 0;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
                projectile.velocity.Y = 16f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 232, projectile.velocity.X / 5, projectile.velocity.Y / 5, 100, default(Color), 0.7f);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                projectile.rotation,
                texture.Size() * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
