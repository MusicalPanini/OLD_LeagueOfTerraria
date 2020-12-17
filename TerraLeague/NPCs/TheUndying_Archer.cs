using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using TerraLeague.Gores;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Accessories;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class TheUndying_Archer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Undying Archer");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SkeletonArcher];
        }
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 3;
            npc.damage = 20;
            npc.defense = 4;
            npc.lifeMax = 55;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 150f;
            if (Main.hardMode)
                aiType = NPCID.SkeletonArcher;
            else
                aiType = NPCID.GoblinArcher;
            animationType = NPCID.SkeletonArcher;
            npc.scale = 1f;
            base.SetDefaults();
            banner = npc.type;
            bannerItem = ItemType<UndyingArcherBanner>();
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
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && (spawnInfo.player.ZoneBeach || NPC.downedBoss3))
                return SpawnCondition.OverworldNightMonster.Chance * 0.5f;
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

                Gore.NewGore(npc.Top, npc.velocity / 2, mod.GetGoreSlot("Gores/TheUndying_Archer_1"), 1f);

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

            float rnd = Main.rand.NextFloat();
            if (rnd <= 0.0067f || (Main.expertMode && rnd <= 0.0133f))
                Item.NewItem(npc.position, npc.width, npc.height, ItemType<Nightbloom>(), 1);

            base.NPCLoot();
        }
    }
}
