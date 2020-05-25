﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class NezuksGauntlet_ArcaneShift : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Arcane Shift");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }

        public virtual string GetName()
        {
            return "Echo";
        }

        public override void AI()
        {
            if ((int)projectile.ai[0] == -1)
            {
                projectile.ai[0] = FindNewTarget();

                if ((int)projectile.ai[0] == -1)
                {
                    projectile.Kill();
                    return;
                }
            }
            else
            {

                projectile.friendly = true;

                if (projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref projectile.velocity);
                    projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 400;

                NPC npc = Main.npc[(int)projectile.ai[0]];

                Vector2 newMove = npc.Center - projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    move = newMove;
                    distance = distanceTo;
                }

                AdjustMagnitude(ref move);
                projectile.velocity = move;
            }
            if (projectile.timeLeft < 15)
            {
                projectile.alpha += 10;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 133, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }

        private int FindNewTarget()
        {
            projectile.netUpdate = true;

            NPC closest = null;
            float cDistance = 99999;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (!target.townNPC && !target.immortal)
                {
                    float shootToX = target.Center.X - projectile.Center.X;
                    float shootToY = target.Center.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    if (distance < 400 && !target.friendly && target.active && distance < cDistance)
                    {
                        closest = target;
                        cDistance = distance;
                    }
                }
            }
            if (closest != null)
            {
                closest.immune[projectile.owner] = 0;
                return closest.whoAmI;
            }
            else
            {
                return -1;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 113, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1.2f);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (target.HasBuff(BuffType<Buffs.EssenceFluxDebuff>()))
            {
                Items.Weapons.NezuksGauntlet gaunt = new Items.Weapons.NezuksGauntlet();
                modPlayer.magicFlatDamage += (int)(player.HeldItem.damage + gaunt.GetAbilityScalingDamage(player, AbilityType.W, DamageType.RNG) + gaunt.GetAbilityScalingDamage(player, AbilityType.W, DamageType.MAG));

                SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 12), target.position);
                if (sound != null)
                    sound.Pitch = 0.5f;

                projectile.magic = true;
                player.ManaEffect(40);
                player.statMana += 40;
            }

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)projectile.ai[0])
                return true;
            else
                return false;
        }
    }
    public class ArcaneShiftGlobalNPC : GlobalNPC
    {
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.HasBuff(BuffType<Buffs.EssenceFluxDebuff>()))
            {
                if (projectile.type == ProjectileType<NezuksGauntlet_ArcaneShift>())
                    npc.DelBuff(npc.FindBuffIndex(BuffType<Buffs.EssenceFluxDebuff>()));
            }
        }
    }
}
