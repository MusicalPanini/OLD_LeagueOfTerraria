using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class DamnedSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 3));
            ItemID.Sets.ItemNoGravity[item.type] = true;
            DisplayName.SetDefault("Damned Soul");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 28;
            item.height = 34;
            item.uniqueStack = false;
            item.rare = 2;
            item.value = 1000;
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - item.Center;
            Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2));
            item.velocity = item.velocity + movement;
            return true;
        }

        public override void PostUpdate()
        {
            ItemID.Sets.ItemIconPulse[item.type] = false;

            Lighting.AddLight(item.Center, Color.DarkSeaGreen.ToVector3() * 0.55f * Main.essScale);
        }
    }

    public class DamnedSoulGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (!Main.dayTime && Main.moonPhase == 4 && npc.lifeMax > 5 && !npc.SpawnedFromStatue && Main.rand.Next(0, 4) == 0)
                Item.NewItem(npc.getRect(), ItemType<DamnedSoul>());

            base.NPCLoot(npc);
        }
    }
}
