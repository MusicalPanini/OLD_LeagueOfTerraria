using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class MessengerBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Messenger");
            Description.SetDefault("You have access to a bunch of information");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;

            player.accFishFinder = true;
            player.accWeatherRadio = true;
            player.accCalendar = true;

            player.accThirdEye = true;
            player.accJarOfSouls = true;
            player.accCritterGuide = true;

            player.accStopwatch = true;
            player.accOreFinder = true;
            player.accDreamCatcher = true;

            if (modPlayer.bottleOfStardust)
            {
                player.dangerSense = true;
                player.nightVision = true;
                player.detectCreature = true;
            }
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "You have access to a bunch of information" +
                    "\nYour eyes can see more";
            else
                tip = "You have access to a bunch of information";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
