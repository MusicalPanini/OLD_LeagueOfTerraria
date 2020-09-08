using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class DarkSovereignsStaff_DarkSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Sphere");
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.penetrate = -1;
            projectile.timeLeft = 1000;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 targetPosition = GetTarget();

            if (projectile.soundDelay == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 32, 32, 112, 0, 0, projectile.alpha, default(Color), 2);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.velocity *= 1.5f;
                }
            }
            projectile.soundDelay = 100;

            if (projectile.Distance(player.Center) > 1500)
            {
                projectile.Center = player.Top;
            }
            else if (projectile.Distance(player.Center) > 1000)
            {
                targetPosition = player.Top;
            }

            if (projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref projectile.velocity);
                projectile.localAI[0] = 1f;
            }

            Vector2 move = targetPosition - projectile.Center;

            if ((int)projectile.ai[0] != -1)
            {
                AdjustMagnitude(ref move, 12);
                projectile.velocity = (10f * projectile.velocity + move) / 10f;
                AdjustMagnitude(ref projectile.velocity);
            }
            else
            {
                AdjustMagnitude(ref move, 6);
                projectile.velocity = (10 * projectile.velocity + move) / 10f;
                AdjustMagnitude(ref projectile.velocity, 8);
            }

            Rectangle projectileHitBox = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            for (int i = 0; i < 1000; i++)
            {
                if (i != projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].type == projectile.type)
                {
                    Rectangle targetHitBox = new Rectangle((int)Main.projectile[i].position.X, (int)Main.projectile[i].position.Y, Main.projectile[i].width, Main.projectile[i].height);
                    if (projectileHitBox.Intersects(targetHitBox))
                    {
                        Vector2 vector77 = Main.projectile[i].Center - projectile.Center;
                        if (vector77.X == 0f && vector77.Y == 0f)
                        {
                            if (i < projectile.whoAmI)
                            {
                                vector77.X = -1f;
                                vector77.Y = 1f;
                            }
                            else
                            {
                                vector77.X = 1f;
                                vector77.Y = -1f;
                            }
                        }
                        vector77.Normalize();
                        vector77 *= 0.1f;
                        projectile.velocity -= vector77;
                        Projectile projectile2 = Main.projectile[i];
                        projectile2.velocity += vector77;
                    }
                }
            }

            if (Main.player[projectile.owner].HasBuff(ModContent.BuffType<DarkSphere>()))
                projectile.timeLeft = 3;
        }

        private void AdjustMagnitude(ref Vector2 vector, float num = 12)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > num)
            {
                vector *= num / magnitude;
            }
        }

        Vector2 GetTarget()
        {
            projectile.netUpdate = true;
            float distance = 500;
            NPC target = null;
            for (int k = 0; k < 200; k++)
            {
                NPC npcCheck = Main.npc[k];
                if (npcCheck.active && !npcCheck.friendly && !npcCheck.townNPC && npcCheck.lifeMax > 5 && !npcCheck.dontTakeDamage && !npcCheck.immortal && npcCheck.CanBeChasedBy())
                {
                    Vector2 newMove = Main.npc[k].Center - Main.player[projectile.owner].Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (npcCheck == projectile.OwnerMinionAttackTargetNPC && distanceTo < 900)
                    {
                        target = npcCheck;
                        break;
                    }
                    else if (distanceTo < distance)
                    {
                        distance = distanceTo;
                        target = npcCheck;
                    }
                }
            }

            projectile.ai[0] = target == null ? -1 : target.whoAmI;
            return target == null ? Main.player[projectile.owner].Top : target.Center;
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
                new Vector2(texture.Width, texture.Width) * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
            base.PostDraw(spriteBatch, lightColor);
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}
