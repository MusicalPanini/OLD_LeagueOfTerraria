using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class ShelledHorror : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shelled Horror");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GiantTortoise];
        }
        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 32;
            npc.aiStyle = 39;
            npc.damage = 100;
            npc.defense = 40;
            npc.lifeMax = 1000;
            npc.knockBackResist = 0.15f;
            npc.HitSound = SoundID.NPCHit24;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 1000f;
            aiType = NPCID.GiantTortoise;
            animationType = NPCID.GiantTortoise;
            base.SetDefaults();
            npc.npcSlots = 3;
            npc.scale = 1f;
            banner = npc.type;
            bannerItem = ItemType<ShelledHorrorBanner>();
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && spawnInfo.player.ZoneJungle && Main.hardMode)
                return SpawnCondition.SurfaceJungle.Chance * 0.5f;
            else if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && Main.hardMode && NPC.downedMechBossAny)
                return SpawnCondition.OverworldNightMonster.Chance * 0.1f;
            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 4) == 0)
                target.AddBuff(BuffID.BrokenArmor, 5*60);
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

            if (Main.rand.NextFloat(0, 1) <= 0.0588f)
            {
                int item = Item.NewItem(npc.position, npc.width, npc.height, ItemID.TurtleShell, 1);
                Main.item[item].color = new Color(100, 200, 150);
            }
                

            base.NPCLoot();
        }
    }
}
