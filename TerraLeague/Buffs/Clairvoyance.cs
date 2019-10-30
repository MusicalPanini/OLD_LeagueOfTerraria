using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class Clairvoyance : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Clairvoyance");
            Description.SetDefault("You see all");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.dangerSense = true;
            player.nightVision = true;
            player.findTreasure = true;
            player.detectCreature = true;
        }
    }
}