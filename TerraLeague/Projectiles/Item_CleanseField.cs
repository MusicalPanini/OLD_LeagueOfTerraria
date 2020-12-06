using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class Item_CleanseField : ModProjectile
    {
        int effectRadius = 16 * 16;
        int nodeFrames = 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cleanse Field");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.timeLeft = (60 * 10) + 2;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            if (projectile.ai[0] < nodeFrames)
                projectile.ai[0]++;

            if (projectile.soundDelay == 0)
            {
                Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), projectile.Center);
                for (int j = 0; j < 40; j++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                    dust.noGravity = true;
                }
            }
            projectile.soundDelay = 10;

            TerraLeague.DustBorderRing((int)(effectRadius * projectile.ai[0] / nodeFrames), projectile.Center, 261, new Color(0, 255, 255), 1);

            if (projectile.timeLeft % 120 == 1)
            {
                float rad = 2;

                TerraLeague.DustElipce(rad, rad / 4f, 0, projectile.Center, 111, new Color(0, 255, 255), 1.5f, 180, true, 10);
                TerraLeague.DustElipce(rad / 4f, rad, 0, projectile.Center, 111, new Color(0, 255, 255), 1.5f, 180, true, 10);
            }

            if (projectile.timeLeft % 15 == 0)
            {
                List<int> playersInRange = TerraLeague.GetAllPlayersInRange(projectile.Center, effectRadius * projectile.ai[0] / nodeFrames, -1, Main.player[projectile.owner].team);

                for (int i = 0; i < playersInRange.Count; i++)
                {
                    Main.player[i].AddBuff(BuffType<GeneralCleanse>(), 15);
                }
            }

        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int alpha = 255;
            if (projectile.timeLeft % 120 < 15)
            {
                alpha = (int)(255 * (projectile.timeLeft % 120) / 15f);
            }
            Color color = new Color(255, 255, 255, alpha);



            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
                    (projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f) + (float)System.Math.Sin(Main.time * 0.1) * 3
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                color,
                projectile.rotation,
                new Vector2(texture.Width, texture.Width) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );

            Vector2 nodeOffset = new Vector2(-effectRadius * projectile.ai[0] / nodeFrames, 0).RotatedBy(MathHelper.PiOver4 + MathHelper.ToRadians(projectile.timeLeft));

            texture = GetTexture("TerraLeague/Projectiles/Item_CleanseField_Node");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f + nodeOffset.X,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                projectile.rotation + MathHelper.ToRadians(projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );

            nodeOffset = nodeOffset.RotatedBy(MathHelper.PiOver2);
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f + nodeOffset.X,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                projectile.rotation + MathHelper.ToRadians(projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                projectile.scale,
                SpriteEffects.FlipHorizontally,
                0f
            );

            nodeOffset = nodeOffset.RotatedBy(MathHelper.PiOver2);
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f + nodeOffset.X,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                projectile.rotation + MathHelper.ToRadians(projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );

            nodeOffset = nodeOffset.RotatedBy(MathHelper.PiOver2);
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f + nodeOffset.X,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height * 0.5f + nodeOffset.Y
                ),
                new Rectangle(0, 0, 18, 18),
                color,
                projectile.rotation + MathHelper.ToRadians(projectile.timeLeft),
                new Vector2(18, 18) * 0.5f,
                projectile.scale,
                SpriteEffects.FlipHorizontally,
                0f
            );
        }
    }
}
