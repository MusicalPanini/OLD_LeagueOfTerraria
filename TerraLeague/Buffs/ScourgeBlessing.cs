using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class ScourgeBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Scourge");
            Description.SetDefault("Your attacks will apply 'Cursed Inferno'");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.scourgeBlessing = true;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "Your attacks will apply 'Cursed Inferno' and 'Grievous Wounds'";
            else
                tip = "Your attacks will apply 'Cursed Inferno'";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
