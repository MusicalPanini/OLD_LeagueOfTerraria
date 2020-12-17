using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class InTargonArena : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Arena");
            Description.SetDefault("Prove your worth");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().targonArena = true;
            player.noBuilding = true;
        }
    }
}
