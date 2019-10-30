﻿using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class BlackCleaver : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Black Cleaver");
            Tooltip.SetDefault("15% increased melee damage" +
                "\nIncreases health by 40" +
                "\nAbility cooldown reduced by 20%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 800000;
            item.rare = 10;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.2;
            player.meleeDamage += 0.15f;
            player.statLifeMax2 += 40;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Phage>(), 1);
            recipe.AddIngredient(ItemType<Kindlegem>(), 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 18);
            recipe.AddIngredient(ItemID.ChlorophyteGreataxe, 1);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddTile(412);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Crush();
        }

        public override Passive GetSecondaryPassive()
        {
            return new Rage(5);
        }
    }
}
