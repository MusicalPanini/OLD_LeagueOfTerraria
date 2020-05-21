using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class RawMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 12));
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            DisplayName.SetDefault("Raw Magic");
            
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.material = true;
            item.rare = ItemRarityID.Green;
            item.width = 32;
            item.height = 32;
            base.SetDefaults();
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
        }
    }

    public class RawMagicGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.lifeMax > 5 && !npc.SpawnedFromStatue && Main.rand.Next(0, 8) == 0 && !npc.townNPC)
                Item.NewItem(npc.getRect(), ItemType<RawMagic>());

            base.NPCLoot(npc);
        }
    }
}
