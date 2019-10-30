using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CompleteItems
{
    public class Seraphs : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seraph's Embrase");
            Tooltip.SetDefault("6% increased magic damage" +
                "\nIncreases maximum mana by 100" +
                "\nAbility cooldown reduced by 10%" +
                "\nCan only have one AWE item equiped at a time");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            string itemName = player.armor[slot].Name;

            if (itemName == "Tear of the Goddess" || itemName == "Archangel's Staff" || itemName == "Seraph's Embrase" || itemName == "Manamune" || itemName == "Muramana")
            {

            }
            if (modPlayer.awe)
                return false;

            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(60 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 90);
            return true;
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 300000;
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.magicDamage += 0.06f;
            modPlayer.Cdr -= 0.1;
            player.statManaMax2 += 100;

            base.UpdateAccessory(player, hideVisual);
        }

        public override Passive GetPrimaryPassive()
        {
            return new Awe(8, 0, 50);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Haste();
        }

        public override Active GetActive()
        {
            return new ManaShield(4, 15, 200, 50, 90);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ActivesAreActive[slot])
                    return ((int)GetStatOnPlayer(Main.LocalPlayer) / 60).ToString();
                else
                    return "";
            }
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0 || !Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ActivesAreActive[slot])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
