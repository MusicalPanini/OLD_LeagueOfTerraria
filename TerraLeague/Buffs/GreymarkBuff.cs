using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using System.Linq;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class GreymarkBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Petricite Greymark");
            Description.SetDefault("Resist increased!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().greymarkBuff = true;
        }
    }
}