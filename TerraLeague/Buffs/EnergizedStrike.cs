using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class EnergizedStrike : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Energized Strike");
            Description.SetDefault("Your next melee or ranged attack will have bonus effects!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().energized = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
