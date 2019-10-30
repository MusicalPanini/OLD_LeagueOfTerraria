using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class BlueBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Crest of Insight");
            Description.SetDefault("Greatly increased Max Mana" +
                "\n+10% Cooldown Reduction");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.statManaMax2 += 120;
            player.manaRegenBonus += 100;
        }
    }
}
