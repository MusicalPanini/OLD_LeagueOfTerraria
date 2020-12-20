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

    public class EnableDebugModeCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "debugMode"; }
        }

        public override string Usage
        {
            get { return "/debugMode"; }
        }

        public override string Description
        {
            get { return "Enables a bunch of development tools for testing"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (TerraLeague.instance.debugMode)
            {
                TerraLeague.instance.debugMode = false;
                Main.NewText("Debug mode disabled");
            }
            else
            {
                TerraLeague.instance.debugMode = true;
                Main.NewText("Debug mode enabled");
            }
        }
    }

    public class StartHarrowing : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "startHarrowing"; }
        }

        public override string Usage
        {
            get { return "/startHarrowing"; }
        }

        public override string Description
        {
            get { return "Force starts the Harrowing"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.dayTime)
            {
                Main.dayTime = false;
                Main.time = 0;
            }

            TerraLeagueWORLDGLOBAL.BlackMistEvent = true;
        }
    }
}
