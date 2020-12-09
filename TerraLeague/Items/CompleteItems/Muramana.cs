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
    public class Muramana : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Muramana");
            Tooltip.SetDefault("5% increased melee and ranged damage" +
                "\nIncreases maximum mana by 100" +
                "\nIncreases ability haste by 15" +
                "\nCan only have one AWE item equiped at a time");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            string itemName = player.armor[slot].Name;

            if (itemName == "Tear of the Goddess" || itemName == "Archangel's Staff" || itemName == "Seraph's Embrase" || itemName == "Manamune" || itemName == "Muramana")
                return true;
            if (modPlayer.awe)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Shock(5, 15),
                new Awe(6, 60, 0)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            modPlayer.abilityHaste += 15;
            player.statManaMax2 += 100;

            base.UpdateAccessory(player, hideVisual);
        }
    }
}
