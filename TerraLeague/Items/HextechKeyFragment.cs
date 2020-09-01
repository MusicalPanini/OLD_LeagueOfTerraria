using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class HextechKeyFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Key Fragment");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 12;
            item.height = 20;
            item.rare = ItemRarityID.LightRed;
            item.value = 10000;
        }
    }

    public class KeyFragGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.lifeMax > 5 && !npc.SpawnedFromStatue && Main.rand.Next(0, 15) == 0 && !npc.townNPC && Main.hardMode)
                Item.NewItem(npc.getRect(), ItemType<HextechKeyFragment>());

            base.NPCLoot(npc);
        }
    }
}
