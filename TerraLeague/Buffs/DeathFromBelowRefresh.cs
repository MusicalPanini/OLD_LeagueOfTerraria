using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class DeathFromBelowRefresh : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Death From Below");
            Description.SetDefault("Can cast Death From Below again for free");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().deathFromBelowRefresh = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
