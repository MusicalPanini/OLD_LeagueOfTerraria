using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class PowPowExcited : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("EXCITED");
            Description.SetDefault("This party just got crazy!");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().excited = true;
            player.moveSpeed += 1f;
        }
    }
}
