﻿using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Shurelyas : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shurelya's Reverie");
            Tooltip.SetDefault("7% increased magic and minion damage" +
                "\n5% increased movement speed" +
                "\nIncreases maximum life by 20" +
                "\nIncreases mana regeneration by 60%" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 150000;
            item.rare = 4;
            item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(90 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.07;
            player.magicDamage += 0.07f;
            player.moveSpeed += 0.05f;
            player.statLifeMax2 += 20;
            //player.manaRegen = (int)(player.manaRegen * 1.6);
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.6;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Kindlegem>(), 1);
            recipe.AddIngredient(ItemType<AetherWisp>(), 1);
            recipe.AddIngredient(ItemType<FaerieCharm>(), 1);
            recipe.AddIngredient(3783, 1); //Forbidden Fragment
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new FleetFoot(500, 4, 90);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0)
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
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
