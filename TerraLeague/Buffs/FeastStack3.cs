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
                "\nYou melee attacks now deal 10% max life On Hit" +
                "\nGain the ability 'Rupture'");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().feast3 = true;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit = player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep / 10;
            player.statLifeMax2 += 100;
        }
    }
}
