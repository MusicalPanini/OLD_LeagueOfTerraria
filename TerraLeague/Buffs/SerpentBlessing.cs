using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class SerpentBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Serpent");
            Description.SetDefault("10% decreased mana costs");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.manaCost -= 0.1f;
            if (modPlayer.bottleOfStardust)
                player.manaCost -= 0.05f;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "15% decreased mana costs";
            else
                tip = "10% decreased mana costs";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
