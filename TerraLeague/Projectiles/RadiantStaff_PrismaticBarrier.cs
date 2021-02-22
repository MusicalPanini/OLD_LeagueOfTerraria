using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class RadiantStaff_PrismaticBarrier : ModProjectile
    {
        List<int> hitPlayers = new List<int>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radiant Staff");
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.scale = 1f;
            projectile.tileCollide = false;
            projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.rotation += -0.3f;

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X, projectile.position.Y);
                int dustBoxWidth = projectile.width;
                int dustBoxHeight = projectile.height;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 228, 0f, 0f, 100, new Color(229, 242, 249), 1.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            //player.itemTime = 5;
            //if (projectile.timeLeft > 210)
            //{
            //    if (projectile.position.X + (float)(projectile.width / 2) > player.position.X + (float)(player.width / 2))
            //    {
            //        player.ChangeDir(1);
            //    }
            //    else
            //    {
            //        player.ChangeDir(-1);
            //    }
            //}

            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[1] += 1f;
                if (projectile.localAI[1] >= 25f)
                {
                    projectile.localAI[0] = 1f;
                    projectile.localAI[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                float returnSpeed = 12f;
                float acceleration = 0.3f;

                float xDif = player.Center.X - projectile.Center.X;
                float yDif = player.Center.Y - projectile.Center.Y;
                float distance = projectile.Distance(player.Center);

                if (distance > 3000f)
                {
                    projectile.Kill();
                }
                distance = returnSpeed / distance;
                xDif *= distance;
                yDif *= distance;

                if (projectile.velocity.X < xDif)
                {
                    projectile.velocity.X = projectile.velocity.X + acceleration;
                    if (projectile.velocity.X < 0f && xDif > 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X + acceleration;
                    }
                }
                else if (projectile.velocity.X > xDif)
                {
                    projectile.velocity.X = projectile.velocity.X - acceleration;
                    if (projectile.velocity.X > 0f && xDif < 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X - acceleration;
                    }
                }

                if (projectile.velocity.Y < yDif)
                {
                    projectile.velocity.Y = projectile.velocity.Y + acceleration;
                    if (projectile.velocity.Y < 0f && yDif > 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y + acceleration;
                    }
                }
                else if (projectile.velocity.Y > yDif)
                {
                    projectile.velocity.Y = projectile.velocity.Y - acceleration;
                    if (projectile.velocity.Y > 0f && yDif < 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - acceleration;
                    }
                }

                if (Main.myPlayer == projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                    Rectangle value2 = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                    if (rectangle.Intersects(value2))
                    {
                        projectile.Kill();
                    }
                }

                if (projectile.localAI[0] == 0f)
                {
                    Vector2 velocity = projectile.velocity;
                    velocity.Normalize();
                    return;
                }
                Vector2 vector4 = projectile.Center - player.Center;
                vector4.Normalize();
            }

            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (!hitPlayers.Contains(i) && i != projectile.owner)
                    {
                        if (projectile.Hitbox.Intersects(Main.player[i].Hitbox))
                        {
                            if (Main.player[i].team == player.team && Main.player[i].active)
                            {
                                player.GetModPlayer<PLAYERGLOBAL>().SendShieldPacket((int)projectile.ai[0], i, ShieldType.Basic, 120, -1, player.whoAmI, Color.Yellow);
                                hitPlayers.Add(i);
                            }
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.localAI[0] = 1f;
            if (target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().illuminated)
                Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().magicOnHit += 40 + (int)(Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().MAG * 0.2);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(4) == 0)
                target.AddBuff(BuffType<Illuminated>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}
