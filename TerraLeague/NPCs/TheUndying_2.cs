using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class TheUndying_2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Undying");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueArmoredBonesMace];
        }
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 3;
            npc.damage = 12;
            npc.defense = 9;
            npc.lifeMax = 55;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.knockBackResist = 0.1f;
            npc.value = 100f;
            aiType = NPCID.BlueArmoredBonesMace;
            animationType = NPCID.BlueArmoredBonesMace;
            npc.scale = 1f;
            base.SetDefaults();
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
