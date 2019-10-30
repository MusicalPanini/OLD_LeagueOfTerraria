using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class SpinningAxe : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spinning Axe");
            Description.SetDefault("Your next Darksteel Throwing Axe will deal additional damage" +
                "\nIt will also bounce back for you to catch, refreshing this buff");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().spinningAxe = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
