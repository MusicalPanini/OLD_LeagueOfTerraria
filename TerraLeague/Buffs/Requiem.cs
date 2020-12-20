using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Requiem : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Requiem");
            Description.SetDefault("This might hurt");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().requiem = true;
            player.GetModPlayer<PLAYERGLOBAL>().requiemTime = player.buffTime[buffIndex];
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().requiem = true;
            npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().requiemTime = npc.buffTime[buffIndex];
        }
    }
}
