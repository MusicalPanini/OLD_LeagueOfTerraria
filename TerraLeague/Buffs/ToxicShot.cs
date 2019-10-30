using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class ToxicShot : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Toxic Shot");
            Description.SetDefault("Your ranged attacks are toxic");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().toxicShot = true;
        }
    }
}
