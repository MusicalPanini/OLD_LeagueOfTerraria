using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class Ablaze : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ablaze");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff(BuffID.OnFire))
                npc.DelBuff(npc.FindBuffIndex(BuffID.OnFire));
            npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().ablaze = true;
        }
    }
}
