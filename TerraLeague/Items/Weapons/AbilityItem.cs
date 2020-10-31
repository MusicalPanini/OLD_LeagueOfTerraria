﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    /// <summary>
    /// <para>An AbilityItem is an item that can house up to 4 abilities (AbilityType: Q, W, E, and R).</para>
    /// <para>Each cycle, the game will check the players hand and inventory for one of each of the ability type that may exist within them using the DoesAbilityExist(AbilityType) Method.</para>
    /// <para>If this method returns true, it will mark the player with this item and grab the abilty when needed.</para>
    /// </summary>
    abstract public class AbilityItem : ModItem
    {
        internal AbilitiesPacketHandler PacketHandler = new AbilitiesPacketHandler(7);

        public Ability[] Abilities = new Ability[Enum.GetNames(typeof(AbilityType)).Length];

        /// <summary>
        /// <para>Modifies the Items tooltip</para>
        /// Recommended to not override.
        /// </summary>
        /// <param name="tooltips"></param>
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            SetDefaults();

            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.IndexOf(tt);

                string text = GetWeaponTooltip() != "" ? "\n" + GetWeaponTooltip() : "";
                if (GetIfAbilityExists(AbilityType.Q))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.QAbility) + ":] [c/5cd65c:" + Abilities[(int)AbilityType.Q].GetAbilityName() + "]" +
                        "\n" + Abilities[(int)AbilityType.Q].GetTooltip();
                }
                if (GetIfAbilityExists(AbilityType.W))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.WAbility) + ":] [c/5cd65c:" + Abilities[(int)AbilityType.W].GetAbilityName() + "]" +
                        "\n" + Abilities[(int)AbilityType.W].GetTooltip();
                }
                if (GetIfAbilityExists(AbilityType.E))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.EAbility) + ":] [c/5cd65c:" + Abilities[(int)AbilityType.E].GetAbilityName() + "]" +
                        "\n" + Abilities[(int)AbilityType.E].GetTooltip();
                }
                if (GetIfAbilityExists(AbilityType.R))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.RAbility) + ":] [c/5cd65c:" + Abilities[(int)AbilityType.R].GetAbilityName() + "]" +
                        "\n" + Abilities[(int)AbilityType.R].GetTooltip();
                }
                text += "\n[c/cc9900:'" + GetQuote() + "']";

                string[] lines = text.Split('\n');
                tooltips.RemoveAt(pos);


                for (int i = 1; i < lines.Count(); i++)
                {
                    tooltips.Insert(pos + i - 1, new TooltipLine(TerraLeague.instance, "Tooltip" + (i - 1), lines[i]));
                }
            }

            base.ModifyTooltips(tooltips);
        }

        /// <summary>
        /// <para>Returns the Weapons tooltip</para>
        /// Override to set
        /// </summary>
        /// <returns></returns>
        virtual public string GetWeaponTooltip()
        {
            return "";
        }

        /// <summary>
        /// Returns the Champions quote for the Weapon.
        /// </summary>
        /// <returns></returns>
        virtual public string GetQuote()
        {
            return "";
        }

        /// <summary>
        /// <para>Returns if the requested AbilityType actually exists on the item</para>
        /// Default returns false
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public bool GetIfAbilityExists(AbilityType type)
        {
            return Abilities[(int)type] != null;
        }
    }
}
