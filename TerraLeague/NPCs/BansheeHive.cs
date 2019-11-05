using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class BansheeHive : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banshee Hive");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Ghost];
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 24;
            npc.height = 44;
            npc.aiStyle = 22;
            npc.damage = 12;
            npc.defense = 9;
            npc.lifeMax = 55;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 100f;
            aiType = NPCID.Ghost;
            animationType = NPCID.Ghost;
            base.SetDefaults();
            npc.scale = 1f;
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
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), Main.rand.Next(1,3));

            base.NPCLoot();
        }
    }
}
