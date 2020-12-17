using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.IO;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class MistDevourer_Head : WormClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mist Devourer");
        }
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.SeekerHead);
            npc.lifeMax = 625;
            npc.aiStyle = -1;
            minLength = 24;
            maxLength = 30;
            headType = NPCType<MistDevourer_Head>();
            bodyType = NPCType<MistDevourer_Body>();
            tailType = NPCType<MistDevourer_Tail>();
            speed = 10f;
            turnSpeed = 0.06f;

            head = true;

            banner = npc.type;
            bannerItem = ItemType<MistDevourerBanner>();
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void CustomBehavior()
        {
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && spawnInfo.player.ZoneCorrupt && Main.hardMode)
                return SpawnCondition.Corruption.Chance * 0.5f;
            else if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && Main.ActiveWorldFileData.HasCorruption && Main.hardMode && NPC.downedMechBossAny)
                return SpawnCondition.OverworldNightMonster.Chance * 0.1f;
            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 4) == 0)
                target.AddBuff(BuffID.Confused, 5*60);
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
                    Dust dust;
                    if (i > 10)
                        dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 54, 0f, 0f, 50, default(Color), 1.5f);
                    else
                        dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
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
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), Main.rand.Next(1,3));

            Item.NewItem(npc.position, npc.width, npc.height, ItemID.CursedFlame, Main.rand.Next(2,6));

            base.NPCLoot();
        }
    }
}
