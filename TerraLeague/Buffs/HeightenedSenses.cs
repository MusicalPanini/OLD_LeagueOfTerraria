using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class HeightenedSenses : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Heightened Senses");
            Description.SetDefault("You have sight of enemies, traps and treasure");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.dangerSense = true;
            player.findTreasure = true;
            player.detectCreature = true;
        }
    }
}
