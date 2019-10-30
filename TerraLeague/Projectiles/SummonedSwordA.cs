﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{

    public class SummonedSwordA : ModProjectile
    {
        Vector2 lastCenter = Vector2.Zero;

        const int ai_initialSpawn = 0;
        const int ai_spin = 1;
        const int ai_homing = 2;
        const int ai_fadeout = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            DisplayName.SetDefault("Summoned Sword");
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 42;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.scale = 1.5f;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().summonAbility = true;
        }

        public override void AI()
        {
            projectile.netUpdate = true;

            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                Player player = Main.player[projectile.owner];

                if ((int)projectile.ai[1] != -1)
                {
                    NPC targetNPC = Main.npc[(int)projectile.ai[1]];
                    lastCenter = targetNPC.Center;
                    projectile.ai[1] = -1;
                }



                projectile.rotation = projectile.ai[0] + MathHelper.ToRadians(135);

                if (projectile.timeLeft < 1170)
                    projectile.localAI[1] += (1200 - projectile.timeLeft) / 25f;

                Vector2 offset = new Vector2(100 + projectile.localAI[1], 0);

                projectile.ai[0] += .06f;
                projectile.Center = lastCenter + offset.RotatedBy(projectile.ai[0]);

                if (projectile.localAI[1] > 700)
                {
                    if (projectile.alpha > 250)
                        projectile.Kill();
                    projectile.alpha += 12;
                }
            }



            int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 231, 0f, 0f, 100, default(Color), 1);
            Main.dust[dustIndex].noGravity = true;
            Lighting.AddLight(projectile.position, 1f, 0.5f, 0.05f);
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 6f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            crit = false;
            target.immune[projectile.owner] = 10;
            base.OnHitNPC(target, damage, knockback, false);
        }

        public override void Kill(int timeLeft)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 80, 0f, 0f, 100, default(Color), 0.7f);

            //}
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            return true;
        }
    }
}
