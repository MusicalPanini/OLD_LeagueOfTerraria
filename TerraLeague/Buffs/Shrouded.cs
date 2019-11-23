using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Shrouded : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Twilight Shroud");
            Description.SetDefault("You are hidden in the shroud");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.itemAnimation <= 0)
            {
                player.GetModPlayer<PLAYERGLOBAL>().projectileDodge = true;
                player.GetModPlayer<PLAYERGLOBAL>().trueInvis = true;
                player.invis = true;
                player.aggro = -750;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
