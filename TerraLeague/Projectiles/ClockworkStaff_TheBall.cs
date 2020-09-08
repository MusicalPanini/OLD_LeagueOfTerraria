using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.CodeDom;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class ClockworkStaff_TheBall : ModProjectile
    {
        bool attacking = false;

        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("The Ball");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.timeLeft = 2000;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 3;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 124, 0, 0, 0, default(Color), 2);
                    dust.noGravity = true;
                }
            }
            projectile.soundDelay = 100;

            Player player = Main.player[projectile.owner];

            if (player.whoAmI == Main.LocalPlayer.whoAmI && (int)projectile.localAI[0] > 0)
            {
                projectile.localAI[0]--;
            }
            if (!attacking && player.whoAmI == Main.LocalPlayer.whoAmI && (int)projectile.localAI[0] == 0)
            {
                projectile.ai[0] = (int)player.MountedCenter.X - (32 * player.direction);
                projectile.ai[1] = (int)player.MountedCenter.Y - 32;

                float distance = 700;

                if (player.MinionAttackTargetNPC != -1)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal && npc.whoAmI != (int)projectile.ai[0])
                    {
                        float distanceTo = projectile.Distance(npc.Center);
                        if (distanceTo < distance)
                        {
                            distance = distanceTo;
                            Vector2 predictedPosition = npc.Center + (npc.velocity * distance / 10f);
                            Vector2 offset = new Vector2(64, 0).RotatedBy(projectile.AngleTo(predictedPosition));
                            projectile.ai[0] = predictedPosition.X + offset.X;
                            projectile.ai[1] = predictedPosition.Y + offset.Y;
                            attacking = true;
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];

                        if (player.Distance(npc.Center) <= 1000 && npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal && npc.whoAmI != (int)projectile.ai[0] && k != (int)projectile.localAI[1] - 1)
                        {
                            float distanceTo = projectile.Distance(npc.Center);
                            if (distanceTo < distance && Collision.CanHit(player.position, projectile.width * 2, projectile.height * 2, npc.position, npc.width, npc.height))
                            {
                                distance = distanceTo;
                                Vector2 predictedPosition = npc.Center + (npc.velocity * distance / 10f);
                                Vector2 offset = new Vector2(64, 0).RotatedBy(projectile.AngleTo(predictedPosition));
                                projectile.ai[0] = predictedPosition.X + offset.X;
                                projectile.ai[1] = predictedPosition.Y + offset.Y;
                                attacking = true;
                            }
                        }
                    }
                }

                projectile.netUpdate = true;
            }

            float speed = projectile.Distance(new Vector2(projectile.ai[0], projectile.ai[1]));
            if (speed > 10)
                speed = 10;
            if (speed < 0.5f)
            {
                if (player.whoAmI == Main.LocalPlayer.whoAmI)
                {
                    if (projectile.Colliding(projectile.Hitbox, new Rectangle((int)projectile.ai[0], (int)projectile.ai[1], 1, 1)) && projectile.localAI[0] == 0)
                    {
                        projectile.localAI[0] = 20;

                        if (!attacking)
                        {
                            projectile.localAI[1] = 0;
                        }
                    }
                    if (attacking)
                    {
                        attacking = false;
                    }
                }
                projectile.velocity = Vector2.Zero;

            }
            else
            {
                if (!projectile.Colliding(projectile.Hitbox, new Rectangle((int)player.MountedCenter.X, (int)player.MountedCenter.Y - 32, 1, 1)) && attacking )
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.ai[0], projectile.ai[1]), 1, 1, 124, 0, 0, 0, default(Color), 2);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
                projectile.velocity = TerraLeague.CalcVelocityToPoint(projectile.Center, new Vector2(projectile.ai[0], projectile.ai[1]), speed);
            }

            if (projectile.Distance(player.MountedCenter) > 1000)
            {
                projectile.Center = new Vector2(player.MountedCenter.X - (32 * player.direction), player.MountedCenter.Y - 32);
                projectile.netUpdate = true;
            }
            else if (projectile.Distance(player.MountedCenter) > 700)
            {
                projectile.ai[0] = (int)player.MountedCenter.X - (32 * player.direction);
                projectile.ai[1] = (int)player.MountedCenter.Y - 32;
                projectile.netUpdate = true;
            }
            
            if (player.HasBuff(ModContent.BuffType<TheBall>()))
                projectile.timeLeft = 5;

            Lighting.AddLight(projectile.Center, 0, 0.5f, 0.75f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.whoAmI != (int)projectile.localAI[1] - 1)
            {
                //projectile.localAI[1] = target.whoAmI + 1;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.velocity.Length() >= 10)
                return base.CanHitNPC(target);
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //damage *= (int)projectile.minionSlots;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
