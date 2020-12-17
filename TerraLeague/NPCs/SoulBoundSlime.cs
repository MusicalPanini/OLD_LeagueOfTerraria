using Microsoft.Xna.Framework;
using TerraLeague.Items;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class SoulBoundSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soulbound Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 32;
            npc.aiStyle = 1;
            npc.damage = 18;
            npc.defense = 6;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.05f;
            npc.value = 25f;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
            base.SetDefaults();
            npc.scale = 1f;
            banner = npc.type;
            bannerItem = ItemType<SoulBoundSlimeBanner>();
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
                return SpawnCondition.OverworldNightMonster.Chance * 0.5f;
            return 0;
        }

        

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 60; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 4, hitDirection, -2, 150, new Color(5, 245, 150), 1f);
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);
            int item = Item.NewItem(npc.position, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1,4));
            Main.item[item].color = new Color(67,248,175);
            base.NPCLoot();
        }
    }
}
