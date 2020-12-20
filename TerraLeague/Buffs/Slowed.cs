using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class Slowed : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Slowed");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff(BuffType<Frozen>()))
            {
                npc.DelBuff(buffIndex);
            }
            if (!npc.boss)
            {
                if (initial)
                {
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().initialSpeed = npc.velocity.X;
                }
                npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().slowed = true;
                initial = false;
            }
        }
    }
}
