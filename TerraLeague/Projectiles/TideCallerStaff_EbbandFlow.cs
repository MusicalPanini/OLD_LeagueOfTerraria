using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class TideCallerStaff_EbbandFlow : ModProjectile
    {
        int damage = 0;
        int healing = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            DisplayName.SetDefault("Ebb and Flow");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 255;
            projectile.timeLeft = 91;
            projectile.penetrate = 3;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.netImportant = true;
            //projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                damage = (int)projectile.ai[0];
                healing = (int)projectile.ai[1];
                Main.PlaySound(new LegacySoundStyle(2, 21, Terraria.Audio.SoundType.Sound));
                projectile.ai[0] = 0;
                projectile.ai[1] = -1;
            }
            projectile.soundDelay = 100;

            if (projectile.timeLeft == 90)
            {
                if (projectile.friendly)
                    projectile.damage = damage;
                else
                    projectile.damage = healing;
            }

            if (projectile.timeLeft < 84)
            {
                if (projectile.friendly && projectile.ai[0] == 1)
                {
                    if (projectile.localAI[0] == 0f)
                    {
                        AdjustMagnitude(ref projectile.velocity);
                        projectile.localAI[0] = 1f;
                    }
                    Vector2 move = Vector2.Zero;
                    float distance = 400;
                    bool target = false;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];

                        if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal && k != projectile.ai[1])
                        {
                            Vector2 newMove = Main.npc[k].Center - projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                distance = distanceTo;
                                target = true;
                            }
                        }
                    }
                    if (target)
                    {
                        AdjustMagnitude(ref move);
                        projectile.velocity = (10 * projectile.velocity + move) / 11f;
                        AdjustMagnitude(ref projectile.velocity);
                    }
                }
                else if (projectile.ai[0] == 1)
                {
                    if (projectile.localAI[0] == 0f)
                    {
                        AdjustMagnitude(ref projectile.velocity);
                        projectile.localAI[0] = 1f;
                    }
                    Vector2 move = Vector2.Zero;
                    float distance = 400;
                    int targetPlayer = TerraLeague.GetClosestPlayer(projectile.Center, distance, -1, Main.player[projectile.owner].team);

                    if (targetPlayer != -1)
                    {
                        move = Main.player[targetPlayer].MountedCenter - projectile.Center;
                        AdjustMagnitude(ref move);
                        projectile.velocity = (10 * projectile.velocity + move) / 11f;
                        AdjustMagnitude(ref projectile.velocity);
                    }
                }
            }

            Dust dust;
            for (int i = 0; i < 5; i++)
            {
                Color color = default(Color);

                if (i == 4)
                {
                    if (projectile.friendly)
                        color = new Color(100, 100, 255);
                    else
                        color = new Color(0, 255, 100);
                }

                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 137, 0f, 0f, 100, color, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 172, 0f, 0f, 100, default(Color), 0.8f);
                dust.velocity *= 0.25f;
                dust.velocity += projectile.velocity * 0.5f;
            }
            if (!projectile.friendly && projectile.timeLeft < 84)
            {
                for (int i = 0; i < 200; i++)
                {
                    Player player = Main.player[i];
                    if (projectile.Hitbox.Intersects(player.Hitbox))
                    {
                        HitPlayer(player);
                        break;
                    }
                }
            }

            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;

            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().stunned || target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().bubbled)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 137, 0, 0, 50, new Color(100, 100, 255), 1.2f);
                    dust.noGravity = true;
                    Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 211, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, default(Color), 1f);
                }

                projectile.velocity.Y = -8;
                projectile.timeLeft = 90;
                projectile.ai[0] = 1;
                projectile.ai[1] = target.whoAmI;
                projectile.friendly = false;
                projectile.tileCollide = false;
            }
            else
            {
                projectile.Kill();
            }
        }

        public void HitPlayer(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 4, 0);
            Main.PlaySound(new LegacySoundStyle(2, 21), player.Center);

            projectile.netUpdate = true;
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (player.whoAmI != projectile.owner)
                    Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket(healing, player.whoAmI, -1, projectile.owner);
                if (player.whoAmI == Main.myPlayer)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += healing;
                }
            }
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 137, 0,0, 50, new Color(0, 255, 100), 1.2f);
                dust.noGravity = true;
                Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 211, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, default(Color), 1f);
            }

            projectile.velocity.Y = -8;
            projectile.timeLeft = 90;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate--;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 211, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 0, default(Color), 1f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}
