using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class TwilightBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Trickster");
            Description.SetDefault("50 increased max mana" +
                "\n100% increased mana regen");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statManaMax2 += 50;
            if (player.GetModPlayer<PLAYERGLOBAL>().bottleOfStardust)
                player.statManaMax2 += 50;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5f;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "100 increased max mana" +
                "\n100% increased mana regen";
            else
                tip = "50 increased max mana" +
                "\n100% increased mana regen";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
