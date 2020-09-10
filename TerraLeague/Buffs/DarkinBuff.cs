using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class DarkinBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Darkin");
            Description.SetDefault("LIFESTEAL OR SOMETHING");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 3;

            player.wingTimeMax = 120;
            player.wings = 36;
            player.wingsLogic = 29;
        }
    }
}
