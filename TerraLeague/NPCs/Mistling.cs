using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Mistling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mistling");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DungeonSpirit];
        }
        public override void SetDefaults()
        {
            npc.width = 20;
            npc.height = 20;
            npc.aiStyle = 56;
            npc.damage = 35;
            npc.defense = 10;
            npc.lifeMax = 50;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCHit52;
            animationType = NPCID.DungeonSpirit;
            npc.value = 100;
            npc.knockBackResist = 0.2f;
            npc.scale = 1f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
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
            base.NPCLoot();
        }
    }
}
