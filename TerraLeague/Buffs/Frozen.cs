using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Frozen : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Frozen");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
                npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().frozen = true;
            if (!npc.boss)
            {
                npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().stunned = true;
            }
        }
    }
}
