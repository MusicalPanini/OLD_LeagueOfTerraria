using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Scuttlegeist : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scuttlegeist");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.AnomuraFungus];
        }
        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 34;
            npc.damage = 10;
            npc.defense = 14;
            npc.lifeMax = 70;
            npc.aiStyle = 3;
            npc.HitSound = SoundID.NPCHit33;
            npc.DeathSound = SoundID.NPCDeath36;
            npc.knockBackResist = 0.2f;
            npc.value = 100f;
            aiType = NPCID.AnomuraFungus;
            animationType = NPCID.AnomuraFungus;
            base.SetDefaults();
            npc.scale = 1f;
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
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
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
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);

            base.NPCLoot();
        }
    }
}
