using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TargonBoss_SoulShackles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Shackles");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 1000;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.hostile = true;
            projectile.netImportant = true;
            projectile.hide = true;
            projectile.ai[1] = -1;
        }

        public override void AI()
        {
            NPC npc = Main.npc[(int)projectile.ai[0]];

            if ((int)projectile.ai[1] == -2)
            {
                if (projectile.soundDelay == 0)
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 4, -0.5f);
                projectile.soundDelay = 10;
            }

            if (!npc.active)
            {
                ChainBreak(npc.Center);
                projectile.Kill();
            }

            if ((int)projectile.ai[1] >= 0)
            {
                Player player = Main.player[(int)projectile.ai[1]];
                projectile.Center = player.MountedCenter;
            }

            if (-1 != (int)projectile.ai[1])
                DustChain(npc, (int)projectile.Distance(npc.Center) / 16, 1f);

            if (Main.netMode == NetmodeID.Server)
            {
                if ((int)projectile.ai[1] == -1)
                {
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        if (Main.player[i].active)
                        {
                            if (!Main.player[i].dead && projectile.Hitbox.Intersects(Main.player[i].Hitbox))
                            {
                                projectile.ai[1] = i;
                                projectile.timeLeft = 180;
                                projectile.netUpdate = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            if (target.whoAmI == (int)projectile.ai[1])
                return false;
            return base.CanHitPlayer(target);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if ((int)projectile.ai[1] == -1)
            {
                projectile.ai[1] = target.whoAmI;
                projectile.timeLeft = 180;
                target.AddBuff(BuffID.Slow, 180);
                projectile.netUpdate = true;
            }

            base.OnHitPlayer(target, damage, crit);
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            base.ModifyHitPlayer(target, ref damage, ref crit);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (-1 != (int)projectile.ai[1])
            {
                NPC npc = Main.npc[(int)projectile.ai[0]];
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), npc.Center,
                    projectile.Center, 12, ref point);
            }

            return base.Colliding(projHitbox, targetHitbox);
        }

        void DustChain(NPC npc, int loops, float scale)
        {
            //Vector2 ChainLine = projectile.Center - npc.Center;
            //ChainLine.Normalize();

            //for (int i = 0; i < loops; i++)
            //{
            //    int distance = Main.rand.Next((int)projectile.Distance(npc.Center));
            //    Vector2 dustPoint = ChainLine * distance;

            //    Dust dust = Dust.NewDustDirect(dustPoint + npc.Center, 1, 1, 248, 0, 0, 100, new Color(159, 0, 255), scale);
            //    dust.noGravity = true;
            //}
        }

        public void ChainBreak(Vector2 source)
        {
            Vector2 ChainLine = projectile.position - source;
            ChainLine.Normalize();
            int links = (int)projectile.Distance(source) / 64;

            for (int i = 0; i < links; i++)
            {
                int distance = 64 * i;
                Vector2 gorePoint = ChainLine * distance;

                int gore = Gore.NewGore(gorePoint + source, Vector2.Zero, mod.GetGoreSlot("Gores/SoulShackleGoreA"), 2f);

                Main.gore[gore].timeLeft /= 10;

                gorePoint = ChainLine * (distance + 16);
                gore = Gore.NewGore(gorePoint + source, Vector2.Zero, mod.GetGoreSlot("Gores/SoulShackleGoreB"), 2f);
                Main.gore[gore].timeLeft /= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            NPC npc = Main.npc[(int)projectile.ai[0]];
            if ((int)projectile.ai[1] == -2)
                ChainBreak(npc.Center);
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)projectile.ai[1] == -1)
            {
                projectile.timeLeft = 120;
                projectile.netUpdate = true;
                projectile.velocity *= 0;
                projectile.position += projectile.oldVelocity;
                projectile.ai[1] = -2;
            }
            
            return false;
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/TargonBoss_SoulShackles_Chain");

            Vector2 mountedCenter = Main.npc[(int)projectile.ai[0]].Center;
            Vector2 position = projectile.Center + new Vector2(projectile.width, 0).RotatedBy(TerraLeague.CalcAngle(projectile.Center, mountedCenter));
            //mountedCenter.Y += 4;
            Rectangle? sourceRectangle = new Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    Lighting.AddLight(position, 178 / 255f, 0, 1);
                    vector2_4 = mountedCenter - position;
                    Color color2 = Color.White;
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation + MathHelper.Pi, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }

            return true;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCs.Add(index);

            base.DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
