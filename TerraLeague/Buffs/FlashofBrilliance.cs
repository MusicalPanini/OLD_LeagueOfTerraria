using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class FlashofBrilliance : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Flash of Brilliance!");
            Description.SetDefault("Your next attack will cast a burst of random magic");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
