using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Tools
{
    public class FadingMemories : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Faded Mirror");
            Tooltip.SetDefault("Return to where you last died" +
                "\n10 minute cooldown");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 6);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.lastDeathPostion != Vector2.Zero && !player.HasBuff(BuffType<Faded>()))
            {
                player.Teleport(player.lastDeathPostion);
                player.AddBuff(BuffType<Faded>(), 60 * 60 * 10);
                return true;
            }

            return false;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }

    public class FadedMirrorGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            float rnd = Main.rand.NextFloat();
            if (NPCType<TheUndying_1>() == npc.type || NPCType<TheUndying_2>() == npc.type || NPCType<TheUndying_Archer>() == npc.type || NPCType<TheUndying_Necromancer>() == npc.type || NPCType<Mistwraith>() == npc.type || NPCType<UnleashedSpirit>() == npc.type)
                if (rnd <= 0.0033f || (Main.expertMode && rnd <= 0.0067f))
                    Item.NewItem(npc.getRect(), ItemType<FadingMemories>());

            base.NPCLoot(npc);
        }
    }
}
