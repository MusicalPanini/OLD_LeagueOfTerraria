using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague
{
    public class ShowLogsCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "showLogs"; }
        }

        public override string Usage
        {
            get { return "/showLogs"; }
        }

        public override string Description
        {
            get { return "Toggle the display of logs"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            TerraLeague.instance.canLog = TerraLeague.instance.canLog == false ? true : false;
            Main.NewText("Toggled Logs");
        }
    }

    public class StackTearCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "stackTear"; }
        }

        public override string Usage
        {
            get { return "/stackTear"; }
        }

        public override string Description
        {
            get { return "Set MANA CHARGE stacks to 749"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Player.GetModPlayer<PLAYERGLOBAL>().manaChargeStacks = 749;
            Main.NewText("Tear stacked");
        }
    }
}
