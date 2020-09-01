using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using TerraLeague.Items.SummonerSpells;

namespace TerraLeague.Buffs
{
    public class CommandProtect : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Command: Protect");
            Description.SetDefault("You are shielded and have increased armor and resist");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 15;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 15;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}