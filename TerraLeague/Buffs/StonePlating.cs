using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class StonePlating : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stone Skin");
            Description.SetDefault("Your armor and resist have been increased");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().stonePlating = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
