using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class FinalSparkChannel : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Channeling Final Spark");
            Description.SetDefault("No more holding back!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
            player.jump = 0;
            player.wingTime = 0;
            player.noItems = true;
            player.silence = true;
            player.GetModPlayer<PLAYERGLOBAL>().finalsparkChannel = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            
        }
    }
}
