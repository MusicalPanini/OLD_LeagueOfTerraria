using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;


namespace TerraLeague.Buffs
{
    public class FeastStack3 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Large Feast");
            Description.SetDefault("Gain 100 Life" +
                "\nYou melee and magic attacks now deal 5% max life On Hit" +
                "\nTerror of the Void will now launch Vorpal Spikes into the air");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().feast3 = true;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit = player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep / 20;
            player.GetModPlayer<PLAYERGLOBAL>().magicOnHit = player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep / 20;
            player.statLifeMax2 += 100;
        }
    }
}
