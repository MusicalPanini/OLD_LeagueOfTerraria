using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class SpectralBitter : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectral Bitter");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SandShark];
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 18;
            npc.alpha = 110;
            npc.aiStyle = 0;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 50;
            npc.behindTiles = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCHit52;
            aiType = NPCID.Piranha;
            animationType = NPCID.SandShark;
            npc.value = 75;
            npc.knockBackResist = 0.9f;
            npc.scale = 1f;
            base.SetDefaults();
            banner = npc.type;
            bannerItem = ItemType<MistBitterBanner>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
             if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && spawnInfo.player.ZoneJungle)
                return SpawnCondition.OverworldNightMonster.Chance;
            else if(spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && NPC.downedBoss3)
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 100, new Color(5, 245, 150), 1f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += npc.velocity * 0.1f;
                dust.position.X -= npc.velocity.X / 3f * (float)i;
                dust.position.Y -= npc.velocity.Y / 3f * (float)i;
            }


            return base.PreAI();
        }

        public override void AI()
        {
            if (npc.direction == 0)
            {
                npc.TargetClosest(true);
            }

            bool attackTarget = false;
            if (npc.type != NPCID.Goldfish)
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
                if (npc.velocity.X > 2.5f)
                {
                    npc.velocity.X = 2.5f;
                }
                if (npc.velocity.X < -2.5f)
                {
                    npc.velocity.X = -2.5f;
                }
                if (npc.velocity.Y > 2.5f)
                {
                    npc.velocity.Y = 2.5f;
                }
                if (npc.velocity.Y < -2.5f)
                {
                    npc.velocity.Y = -2.5f;
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
                if (npc.type != NPCID.Arapaima && ((double)npc.velocity.Y > 0.4 || (double)npc.velocity.Y < -0.4))
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
            if (npc.life > 0)
            {
                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                Gore gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_2"), npc.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_3"), npc.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);
            base.NPCLoot();
        }
    }
}
