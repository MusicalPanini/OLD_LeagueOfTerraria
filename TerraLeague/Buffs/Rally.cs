using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Rally : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rally");
            Description.SetDefault("Minions are stonger");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().rally = true;
        }
    }
}
