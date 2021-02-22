using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class IcebornSubjugation : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Iceborn Subjugation");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().icebornSubjugation = true;
        }
    }
}
