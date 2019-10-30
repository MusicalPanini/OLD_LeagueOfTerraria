using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Surge : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Surge");
            Description.SetDefault("The magic gives you strength!" +
                "\nIncreases damage by 1.1x");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.meleeStatScaling += 0.1f;
            modPlayer.rangedStatScaling += 0.1f;
            modPlayer.magicStatScaling += 0.1f;
            modPlayer.minionStatScaling += 0.1f;
            player.kbBuff = true;

            modPlayer.surge = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
