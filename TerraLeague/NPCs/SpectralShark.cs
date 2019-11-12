using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class SpectralShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectral Shark");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SandShark];
        }
        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 24;
            npc.aiStyle = 0;
            npc.damage = 50;
            npc.defense = 20;
            npc.lifeMax = 360;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            aiType = NPCID.Shark;
            animationType = NPCID.SandShark;
            npc.value = 400;
            npc.knockBackResist = 0.9f;
            npc.scale = 1f;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            if (npc.direction == 0)
            {
                npc.TargetClosest(true);
            }

            bool attackTarget = false;
            if (npc.type != 55)
            {
                npc.TargetClosest(false);
                if (!Main.player[npc.target].dead)
                {
                    attackTarget = true;
                }
            }
            if (!attackTarget)
            {
                if (npc.collideX)
                {
                    npc.velocity.X *= -1f;
                    npc.direction *= -1;
                    npc.netUpdate = true;
                }
                if (npc.collideY)
                {
                    npc.netUpdate = true;
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
                        npc.directionY = -1;
                        npc.ai[0] = -1f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y = Math.Abs(npc.velocity.Y);
                        npc.directionY = 1;
                        npc.ai[0] = 1f;
                    }
                }
            }
            if (attackTarget)
            {
                npc.TargetClosest(true);
                npc.velocity.X += (float)npc.direction * 0.3f;
                npc.velocity.Y += (float)npc.directionY * 0.15f;
                if (npc.velocity.X > 10f)
                {
                    npc.velocity.X = 10f;
                }
                if (npc.velocity.X < -10f)
                {
                    npc.velocity.X = -10f;
                }
                if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
                if (npc.velocity.Y < -3f)
                {
                    npc.velocity.Y = -3f;
                }
            }
            else
            {
                npc.velocity.X += (float)npc.direction * 0.1f;
                if (npc.velocity.X < -1f || npc.velocity.X > 1f)
                {
                    npc.velocity.X *= 0.95f;
                }
                if (npc.ai[0] == -1f)
                {
                    npc.velocity.Y -= 0.01f;
                    if ((double)npc.velocity.Y < -0.3)
                    {
                        npc.ai[0] = 1f;
                    }
                }
                else
                {
                    npc.velocity.Y += 0.01f;
                    if ((double)npc.velocity.Y > 0.3)
                    {
                        npc.ai[0] = -1f;
                    }
                }

                int num250 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                int num251 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
                if (Main.tile[num250, num251 - 1] == null)
                {
                    Tile[,] tile3 = Main.tile;
                    int num252 = num250;
                    int num253 = num251 - 1;
                    Tile tile4 = new Tile();
                    tile3[num252, num253] = tile4;
                }
                if (Main.tile[num250, num251 + 1] == null)
                {
                    Tile[,] tile5 = Main.tile;
                    int num254 = num250;
                    int num255 = num251 + 1;
                    Tile tile6 = new Tile();
                    tile5[num254, num255] = tile6;
                }
                if (Main.tile[num250, num251 + 2] == null)
                {
                    Tile[,] tile7 = Main.tile;
                    int num256 = num250;
                    int num257 = num251 + 2;
                    Tile tile8 = new Tile();
                    tile7[num256, num257] = tile8;
                }
                if (Main.tile[num250, num251 - 1].liquid > 128)
                {
                    if (Main.tile[num250, num251 + 1].active())
                    {
                        npc.ai[0] = -1f;
                    }
                    else if (Main.tile[num250, num251 + 2].active())
                    {
                        npc.ai[0] = -1f;
                    }
                }
                if (npc.type != 157 && ((double)npc.velocity.Y > 0.4 || (double)npc.velocity.Y < -0.4))
                {
                    npc.velocity.Y *= 0.95f;
                }

            }
            npc.rotation = npc.direction * npc.velocity.Y * 0.1f;

            base.AI();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }
    }
}
