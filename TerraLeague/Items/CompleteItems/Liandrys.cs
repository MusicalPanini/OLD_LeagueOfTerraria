﻿using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Liandrys : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Liandry's Torment");
            Tooltip.SetDefault("8% increased minion damage" +
                "\nIncreases health by 40" +
                "\nIncreases your max number of minions by 2");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 800000;
            item.rare = ItemRarityID.Red;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.08;
            player.statLifeMax2 += 40;
            player.maxMinions += 2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HauntingGuise>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemID.PapyrusScarab, 1);
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Madness(2);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Torment(3);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString();
            else
                return "";
        }
    }
}
