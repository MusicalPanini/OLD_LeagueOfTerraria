using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class EtherealRemitter : ModNPC
    {
        int effectRadius = 500;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ethereal Remitter");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Wraith];
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 34;
            npc.height = 50;
            npc.damage = 10;
            npc.defense = 8;
            npc.lifeMax = 60;
            npc.aiStyle = 22;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 100f;
            npc.npcSlots = 2;
            aiType = NPCID.Wraith;
            animationType = NPCID.Wraith;
            npc.scale = 1f;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());
            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
        }

        public override void PostAI()
        {
            npc.ai[3]++;

            if (npc.ai[3] > 240)
            {
                if (Main.netMode != 1)
                {
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        NPC healTarget = Main.npc[i];

                        if (healTarget.active && !healTarget.immortal && !healTarget.friendly && !healTarget.townNPC && healTarget.lifeMax > 5)
                        {
                            if (npc.Distance(healTarget.Center) < effectRadius && healTarget.active && i != npc.whoAmI)
                            {
                                int heal = (int)((healTarget.lifeMax - healTarget.life) * 0.3);

                                if (heal == 0 && healTarget.lifeMax != healTarget.life)
                                    heal = 1;


                                if (heal > 0)
                                {
                                    healTarget.life += heal;
                                    if (healTarget.life > healTarget.lifeMax)
                                        healTarget.life = healTarget.lifeMax;
                                    healTarget.netUpdate = true;

                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendData(81, -1, -1, null, (int)Color.DarkGreen.PackedValue, healTarget.position.X, healTarget.position.Y, (float)heal, 0, 0, 0);
                                    }
                                    else
                                    {
                                        CombatText.NewText(healTarget.Hitbox, Color.DarkGreen, heal);
                                    }
                                }
                            }
                        }
                    }
                }

                TerraLeague.DustRing(261, npc, new Color(0, 255, 0, 0));
                //Main.PlaySound(new LegacySoundStyle(2, 29), user.Center);


                for (int i = 0; i < effectRadius / 5; i++)
                {
                    Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + npc.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 267, Vector2.Zero, 0, new Color(0, 255, 0, 0), 2);
                    dustR.noGravity = true;
                }

                npc.ai[3] = 0;
            }

            base.PostAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && (spawnInfo.player.ZoneBeach || NPC.downedBoss3))
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
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
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 54, 0f, 0f, 50, default(Color), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 54, 0f, 0f, 50, default(Color), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                Gore gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), 99, npc.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), 99, npc.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), 99, npc.scale);
                gore.velocity *= 0.3f;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);

            base.NPCLoot();
        }
    }
}
