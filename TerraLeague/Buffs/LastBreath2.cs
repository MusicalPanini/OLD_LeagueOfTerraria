using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;


namespace TerraLeague.Buffs
{
    public class LastBreath2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Gathering Storm");
            Description.SetDefault("Unleash a tornado!");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().gathering2 = true;
        }
    }
}
