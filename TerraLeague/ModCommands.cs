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

    public class DisableModdedUICommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "toggleModUI"; }
        }

        public override string Usage
        {
            get { return "/toggleModUI"; }
        }

        public override string Description
        {
            get { return "Toggles between using Vanilla UI or League of Terraria's custom UI "; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (TerraLeague.instance.disableModUI)
            {
                TerraLeague.instance.disableModUI = false;
                Main.NewText("Mod UI Enabled");
            }
            else
            {
                TerraLeague.instance.disableModUI = true;
                Main.NewText("Mod UI Disabled");
            }
        }
    }
}
