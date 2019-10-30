using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;
using Terraria.Audio;

namespace TerraLeague.Buffs
{
    public class Revived : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Revived");
            Description.SetDefault("You've been sparked with new energy!" +
                "\nMovement speed and defence have been increased!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
            player.moveSpeed += 0.1f;
        }
    }
}
