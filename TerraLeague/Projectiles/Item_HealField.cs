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
    class Item_HealField : ModProjectile
    {
        int effectRadius = 16 * 20;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Item_HealField");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.timeLeft = (150);
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
            if (projectile.soundDelay == 0)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 8, 0);
                for (int j = 0; j < 40; j++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 100, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                    dust.noGravity = true;
                }
            }
            projectile.soundDelay = 10;

            TerraLeague.DustBorderRing((int)(effectRadius), projectile.Center, 261, new Color(0, 255, 100), 1);

            if (projectile.timeLeft % 30 == 1)
            {
                float rad = effectRadius - (effectRadius * projectile.timeLeft / 150f);
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 15, 0f - (0.05f * projectile.timeLeft / 30));
                TerraLeague.DustElipce(rad, rad, 0, projectile.Center, 261, new Color(0, 255, 100), 1.5f, 180, true);
            }
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustElipce(2, 2, 0, projectile.Center, 261, new Color(0, 255, 100), 1f, 180, true, 10);
            Main.PlaySound(new LegacySoundStyle(2, 29), projectile.Center);

            Player player = Main.player[projectile.owner];
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                var players = TerraLeague.GetAllPlayersInRange(projectile.Center, effectRadius, -1, player.team);

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] == projectile.owner)
                        player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += projectile.damage;
                    else
                        player.GetModPlayer<PLAYERGLOBAL>().SendHealPacket(projectile.damage, players[i], -1, player.whoAmI);
                }

                var npcs = TerraLeague.GetAllNPCsInRange(projectile.Center, effectRadius, true, true);
                for (int i = 0; i < npcs.Count; i++)
                {
                    player.ApplyDamageToNPC(Main.npc[npcs[i]], projectile.damage * 2, 0, 0, false);
                }
            }

            base.Kill(timeLeft);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int alpha = 255;
            if (projectile.timeLeft % 30 < 15)
            {
                alpha = (int)(255 * (projectile.timeLeft % 30) / 15f);
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
        }
    }
}
