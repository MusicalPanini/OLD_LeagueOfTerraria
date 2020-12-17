using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Accessories;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class TheUndying_1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Undying");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueArmoredBonesMace];
        }
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 3;
            npc.damage = 28;
            npc.defense = 9;
            npc.lifeMax = 40;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 100f;
            aiType = NPCID.BlueArmoredBonesMace;
            animationType = NPCID.BlueArmoredBonesMace;
            npc.scale = 1f;
            base.SetDefaults();
            banner = npc.type;
            bannerItem = ItemType<UndyingBanner>();
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
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist)
                return SpawnCondition.OverworldNightMonster.Chance;
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

                Gore.NewGore(npc.Center, npc.velocity/2, mod.GetGoreSlot("Gores/TheUndying_1_1"), 1f);
                Gore.NewGore(npc.Top, npc.velocity / 2, mod.GetGoreSlot("Gores/TheUndying_1_2"), 1f);
                Gore.NewGore(npc.Bottom, npc.velocity / 2, mod.GetGoreSlot("Gores/TheUndying_1_3"), 1f);

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
