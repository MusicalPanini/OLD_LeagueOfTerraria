using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;


namespace TerraLeague.Buffs
{
    public class LastBreath3 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Gathering Storm");
            Description.SetDefault("Winds are building up around you sword!");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().gathering3 = true;
        }
    }
}
