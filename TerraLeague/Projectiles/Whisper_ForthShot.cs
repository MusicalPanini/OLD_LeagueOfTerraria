using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Whisper_ForthShot : ModProjectile
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
            projectile.extraUpdates = 48;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                SoundEffectInstance sound = Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Custom/WhisperShot"), projectile.position);
                if (sound != null)
                    sound.Pitch = -0.5f;
            }
            projectile.soundDelay = 100;

            if (projectile.timeLeft < 354)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 182, Vector2.Zero);
                dust.noGravity = true;
                dust.velocity *= 0;
                dust = Dust.NewDustPerfect(projectile.Center - projectile.velocity.SafeNormalize(Vector2.Zero), 182, Vector2.Zero);
                dust.noGravity = true;
                dust.velocity *= 0;
            }

            Lighting.AddLight(projectile.position, 1f, 0.0f, 0.0f);
            if (projectile.alpha > 0)
                projectile.alpha -= 15;
            if (projectile.alpha < 0)
                projectile.alpha = 0;

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
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 225, projectile.velocity.X / 5, projectile.velocity.Y / 5, 0, default(Color), 0.7f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = true;
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
