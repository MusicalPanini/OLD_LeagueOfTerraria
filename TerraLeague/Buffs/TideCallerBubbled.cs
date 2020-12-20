using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class TideCallerBubbled : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bubbled");
            Description.SetDefault("Trapped in a giant bubble!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if(!npc.boss)
            {
                npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().bubbled = true;
            }
        }
    }
}
