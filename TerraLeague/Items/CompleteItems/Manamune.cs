using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Manamune : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Manamune");
            Tooltip.SetDefault("6% increased melee damage" +
                "\nIncreases maximum mana by 25" +
                "\nIf MANA CHARGE is fully stacked, this will upgrade into Muramana" +
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
            item.value = 180000;
            item.rare = 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.meleeDamage += 0.06f;
            player.statManaMax2 += 25;
            modPlayer.manaCharge = true;

            if (modPlayer.manaChargeStacks >= 750)
            {
                int where = TerraLeague.FindAccessorySlotOnPlayer(player, this);
                byte prefix = player.armor[where + 3].prefix;
                //player.QuickSpawnItem(ItemType<Seraphs>());

                player.armor[where + 3].SetDefaults(ItemType<Muramana>());
                player.armor[where + 3].Prefix(prefix);
                modPlayer.manaChargeStacks = 0;
                return;
            }

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Tear>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 10);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.ManaCrystal, 2);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Awe(6, 50, 0);
        }

        public override Passive GetSecondaryPassive()
        {
            return null;
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().manaChargeStacks.ToString();
            else
                return "";
        }
    }
}
