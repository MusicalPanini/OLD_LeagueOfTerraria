using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;


namespace TerraLeague.Buffs
{
    public class FeastStack2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Medium Feast");
            Description.SetDefault("Gain 50 Life" +
                "\nYou melee attacks now deal 10% max life On Hit");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().feast2 = true;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit = player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep/10;
            player.statLifeMax2 += 50;
        }
    }
}
