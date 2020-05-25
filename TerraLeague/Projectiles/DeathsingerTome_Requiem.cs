﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DeathsingerTome_Requiem : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Requiem");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 8;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        public override void AI()
        {
            if(projectile.timeLeft == 301)
            {
                Main.PlaySound(new LegacySoundStyle(2, 88), projectile.Center);
                if (projectile.owner == Main.LocalPlayer.whoAmI)
                    projectile.netUpdate = true;
            }

            if (!Main.npc[(int)projectile.ai[0]].active)
            {
                projectile.Kill();
            }
            else if (projectile.Hitbox.Intersects(Main.npc[(int)projectile.ai[0]].Hitbox))
            {
                projectile.velocity = Vector2.Zero;
                projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
            }
            else
            {
                projectile.timeLeft = 300;

                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;

                NPC npc = Main.npc[(int)projectile.ai[0]];

                projectile.Center = new Vector2(npc.Center.X, projectile.Center.Y);

                Vector2 newMove = npc.Center - projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                move = newMove;
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 2f;
                AdjustMagnitude(ref projectile.velocity);

                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(24, 86, 69, 255), 1.5f);
                    dust.velocity *= 0f;
                    dust.noGravity = true;
                }
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(24, 86, 69, 255), 1.5f);
                dust2.noGravity = true;
                dust2.velocity *= 3f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            Main.PlaySound(new LegacySoundStyle(2, 92), projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(24, 86, 69, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return base.CanHitNPC(target);
            else
                return false;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }
    }
}
