using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;


namespace TerraLeague.Buffs
{
    public class FeastStack1 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Small Feast");
            Description.SetDefault("Gain 20 Life");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().feast1 = true;
            player.statLifeMax2 += 20;
        }
    }
}
