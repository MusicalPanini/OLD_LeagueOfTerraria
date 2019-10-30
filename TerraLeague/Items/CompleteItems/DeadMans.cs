﻿using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class DeadMans : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dead Man's Plate");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases armor by 6" +
                "\nImmunity to Weakness and Broken Armor");
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
            player.statLifeMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;

            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ChainVest>(), 1);
            recipe.AddIngredient(ItemType<GiantsBelt>(), 1);
            recipe.AddIngredient(ItemID.ArmorBracing, 1);
            recipe.AddIngredient(ItemID.NecroBreastplate, 1);
            recipe.AddIngredient(ItemType<BrassBar>(), 10);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Dreadnought(0.05);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString() + "%";
            else
                return "";
        }
    }
}
