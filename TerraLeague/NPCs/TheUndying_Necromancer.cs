using Microsoft.Xna.Framework;
using System;
using TerraLeague.Items;
using TerraLeague.Items.Accessories;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class TheUndying_Necromancer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Undying Necromancer");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Necromancer];
        }
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.damage = 10;
            npc.defense = 6;
            npc.lifeMax = 60;
            npc.aiStyle = 0;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.knockBackResist = 0.55f;
            npc.npcSlots = 4;
            npc.value = 100f;
            animationType = NPCID.Necromancer;
            npc.scale = 1f;
            banner = npc.type;
            bannerItem = ItemType<UndyingNecromancerBanner>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());
            return base.PreAI();
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            npc.velocity.X *= 0.93f;
            if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            if (npc.ai[0] == 0f)
            {
                npc.ai[0] = 500f;
            }
            if (npc.ai[2] != 0f && npc.ai[3] != 0f)
            {
                Main.PlaySound(SoundID.Item8, npc.position);
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 160, 0f, 0f, 100, new Color(5, 245, 150), 3);
                    dust.velocity *= 3f;
                    dust.fadeIn = 3;
                    dust.noGravity = true;
                }
                npc.position.X = npc.ai[2] * 16f - (float)(npc.width / 2) + 8f;
                npc.position.Y = npc.ai[3] * 16f - (float)npc.height;
                npc.velocity.X = 0f;
                npc.velocity.Y = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                Main.PlaySound(SoundID.Item8, npc.position);
                for (int i = 0; i < 50; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 160, 0f, 0f, 100, new Color(5, 245, 150), 3);
                    dust.velocity *= 3f;
                    dust.fadeIn = 3;
                    dust.noGravity = true;
                }
            }
            npc.ai[0] += 1f;
            if (npc.ai[0] == 50f || npc.ai[0] == 100f || npc.ai[0] == 150f)
            {
                npc.ai[1] = 30f;
                npc.netUpdate = true;
            }

            if (npc.ai[0] >= 350 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[0] = 1f;
                int playerBlockX = (int)Main.player[npc.target].position.X / 16;
                int playerBlockY = (int)Main.player[npc.target].position.Y / 16;
                int npcBlockX = (int)npc.position.X / 16;
                int npcBlockY = (int)npc.position.Y / 16;
                int extraDistance = 20;
                int loops = 0;
                bool flag4 = false;
                if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
                {
                    loops = 100;
                    flag4 = true;
                }
                while (!flag4 && loops < 100)
                {
                    loops++;
                    int num89 = Main.rand.Next(playerBlockX - extraDistance, playerBlockX + extraDistance);
                    int num90 = Main.rand.Next(playerBlockY - extraDistance, playerBlockY + extraDistance);
                    int num2;
                    for (int num91 = num90; num91 < playerBlockY + extraDistance; num2 = num91, num91 = num2 + 1)
                    {
                        bool flag5;
                        if ((num91 < playerBlockY - 4 || num91 > playerBlockY + 4 || num89 < playerBlockX - 4 || num89 > playerBlockX + 4) && (num91 < npcBlockY - 1 || num91 > npcBlockY + 1 || num89 < npcBlockX - 1 || num89 > npcBlockX + 1) && Main.tile[num89, num91].nactive())
                        {
                            flag5 = true;
                            if (Main.tile[num89, num91 - 1].lava())
                            {
                                flag5 = false;
                            }

                            if (flag5 && Main.tileSolid[Main.tile[num89, num91].type] && !Collision.SolidTiles(num89 - 1, num89 + 1, num91 - 4, num91 - 1))
                            {
                                npc.ai[1] = 20f;
                                npc.ai[2] = (float)num89;
                                npc.ai[3] = (float)num91;
                                flag4 = true;
                                break;
                            }
                        }
                        
                    }
                }
                npc.netUpdate = true;
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1] -= 1f;

                if (npc.ai[1] == 25f)
                {
                    npc.netUpdate = true;
                        spawnBoy();


                }
            }

            if (Main.rand.Next(2) == 0)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 160, 0f, 0f, 0, new Color(5, 245, 150), 1f);
                dust.velocity.X *= 0.5f;
                dust.velocity.Y *= 0.5f;
                dust.fadeIn = 3;
                dust.noGravity = true;
            }


            base.AI();
        }

        void spawnBoy()
        {
            int NpcBlockX = (int)Main.player[npc.target].position.X / 16;
            int NpcBlockY = (int)Main.player[npc.target].position.Y / 16;
            int boyBlockX = (int)npc.position.X / 16;
            int boyBlockY = (int)npc.position.Y / 16;
            int extraDistance = 20;
            int loops = 0;

            while (loops < 100)
            {
                loops++;
                int randX = Main.rand.Next(NpcBlockX - extraDistance, NpcBlockX + extraDistance);
                int randY = Main.rand.Next(NpcBlockY - extraDistance, NpcBlockY + extraDistance);
                int num2;
                for (int i = randY; i < NpcBlockY + extraDistance; num2 = i, i = num2 + 1)
                {
                    bool flag5;
                    if ((i < NpcBlockY - 4 || i > NpcBlockY + 4 || randX < NpcBlockX - 4 || randX > NpcBlockX + 4) && (i < boyBlockY - 1 || i > boyBlockY + 1 || randX < boyBlockX - 1 || randX > boyBlockX + 1) && Main.tile[randX, i].nactive())
                    {
                        flag5 = true;
                        if (Main.tile[randX, i - 1].lava())
                        {
                            flag5 = false;
                        }

                        if (flag5 && Main.tileSolid[Main.tile[randX, i].type] && !Collision.SolidTiles(randX - 1, randX + 1, i - 4, i - 1))
                        {
                            Vector2 boyPos = new Vector2(((randX * 16f) - (float)(npc.width / 2) + 8f), (int)(i * 16f));

                            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(NPCType<Ghoul>()) < 12)
                            {
                                NPC.NewNPC((int)boyPos.X, (int)boyPos.Y, NPCType<Ghoul>());
                            }
                            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 113), boyPos);
                            boyPos.Y -= 40;
                            

                            return;
                        }
                    }

                }
            }
        }

        public override void PostAI()
        {
            base.PostAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && (spawnInfo.player.ZoneBeach || NPC.downedBoss3))
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            npc.ai[0] = 200;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 20; i++)
            {
                int num620;
                if (i > 10)
                    num620 = Dust.NewDust(npc.position, npc.width, npc.height, 54, 0f, 0f, 50, default(Color), 1.5f);
                else
                    num620 = Dust.NewDust(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                Dust dust = Main.dust[num620];
                dust.velocity *= 2f;
                Main.dust[num620].noGravity = true;
            }
            int num621 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), 99, npc.scale);
            Gore gore = Main.gore[num621];
            gore.velocity *= 0.3f;
            num621 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
            gore = Main.gore[num621];
            gore.velocity *= 0.3f;
            num621 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), 99, npc.scale);
            gore = Main.gore[num621];
            gore.velocity *= 0.3f;
            gore.velocity *= 0.3f;
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);

            float rnd = Main.rand.NextFloat();
            if (rnd <= 0.0067f || (Main.expertMode && rnd <= 0.0133f))
                Item.NewItem(npc.position, npc.width, npc.height, ItemType<Nightbloom>(), 1);

            rnd = Main.rand.NextFloat();
            if (rnd <= 0.0133 || (Main.expertMode && rnd <= 0.0266f))
                Item.NewItem(npc.position, npc.width, npc.height, ItemType<PossessedSkull>(), 1);

            base.NPCLoot();
        }
    }
}
