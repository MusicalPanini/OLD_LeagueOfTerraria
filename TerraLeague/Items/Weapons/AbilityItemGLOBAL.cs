using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System.Linq;
using TerraLeague.Items.Armor;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.Items.Weapons
{
    public class AbilityItemGLOBAL : GlobalItem
    {
        internal AbilitiesPacketHandler PacketHandler = new AbilitiesPacketHandler(7);

        //public Ability[] Abilities = new Ability[Enum.GetNames(typeof(AbilityType)).Length];
        Ability AbilityQ = null;
        Ability AbilityW = null;
        Ability AbilityE = null;
        Ability AbilityR = null;


        static string AbilityNameColor = "5cd65c";
        static string AbilityButtonColor = "2eb82e";
        static string QuoteColor = "cc9900";
        public string ChampQuote = "";
        public delegate string GetWeaponTooltip();
        public GetWeaponTooltip getWeaponTooltip;
        public bool IsAbilityItem = false;

        public AbilityItemGLOBAL()
        {
            getWeaponTooltip = DefaultWeaponTooltip;
            AbilityQ = null;
            AbilityW = null;
            AbilityE = null;
            AbilityR = null;
            //Abilities = new Ability[Enum.GetNames(typeof(AbilityType)).Length];
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }

        public override void SetDefaults(Item item)
        {
            
            base.SetDefaults(item);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (IsAbilityItem)
            {
                TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
                if (tt != null)
                {
                    int pos = tooltips.IndexOf(tt);

                    string text = getWeaponTooltip() != "" ? "\n" + getWeaponTooltip() : "";
                    if (GetIfAbilityExists(AbilityType.Q))
                    {
                        text += "\n[c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityNameColor)).Hex3() + ":Ability " + TerraLeague.ConvertKeyString(TerraLeague.QAbility) + ":] [c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityButtonColor)).Hex3() + ":" + GetAbility(AbilityType.Q).GetAbilityName() + "]" +
                            "\n" + GetAbility(AbilityType.Q).GetTooltip();
                    }
                    if (GetIfAbilityExists(AbilityType.W))
                    {
                        text += "\n[c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityNameColor)).Hex3() + ":Ability " + TerraLeague.ConvertKeyString(TerraLeague.WAbility) + ":] [c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityButtonColor)).Hex3() + ":" + GetAbility(AbilityType.W).GetAbilityName() + "]" +
                            "\n" + GetAbility(AbilityType.W).GetTooltip();
                    }
                    if (GetIfAbilityExists(AbilityType.E))
                    {
                        text += "\n[c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityNameColor)).Hex3() + ":Ability " + TerraLeague.ConvertKeyString(TerraLeague.EAbility) + ":] [c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityButtonColor)).Hex3() + ":" + GetAbility(AbilityType.E).GetAbilityName() + "]" +
                            "\n" + GetAbility(AbilityType.E).GetTooltip();
                    }
                    if (GetIfAbilityExists(AbilityType.R))
                    {
                        text += "\n[c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityNameColor)).Hex3() + ":Ability " + TerraLeague.ConvertKeyString(TerraLeague.RAbility) + ":] [c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(AbilityButtonColor)).Hex3() + ":" + GetAbility(AbilityType.R).GetAbilityName() + "]" +
                            "\n" + GetAbility(AbilityType.R).GetTooltip();
                    }
                    text += "\n[c/" + TerraLeague.PulseText(TerraLeague.ConvertHexToColor(QuoteColor)).Hex3() + ":'" + ChampQuote + "']";

                    string[] lines = text.Split('\n');
                    tooltips.RemoveAt(pos);


                    for (int i = 1; i < lines.Count(); i++)
                    {
                        tooltips.Insert(pos + i - 1, new TooltipLine(TerraLeague.instance, "Tooltip" + (i - 1), lines[i]));
                    }
                }
            }
            base.ModifyTooltips(item, tooltips);
        }

        public Ability GetAbility(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Q:
                    return AbilityQ;
                case AbilityType.W:
                    return AbilityW;
                case AbilityType.E:
                    return AbilityE;
                case AbilityType.R:
                    return AbilityR;
                default:
                    return null;
            }
        }

        public void SetAbility(AbilityType type, Ability ability)
        {
            switch (type)
            {
                case AbilityType.Q:
                    AbilityQ = ability;
                    break;
                case AbilityType.W:
                    AbilityW = ability;
                    break;
                case AbilityType.E:
                    AbilityE = ability;
                    break;
                case AbilityType.R:
                    AbilityR = ability;
                    break;
            }
        }

        public bool GetIfAbilityExists(AbilityType type)
        {
            return GetAbility(type) != null;
        }

        public string DefaultWeaponTooltip()
        {
            return "";
        }
    }
}
