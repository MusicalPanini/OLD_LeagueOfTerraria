using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class Lift : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lift");
            Description.SetDefault("You've got wings of magic!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.wingTimeMax = GetInstance<LiftRune>().GetFlightTime();
            player.wings = 2;
            player.wingsLogic = 21;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {

        }
    }
}
