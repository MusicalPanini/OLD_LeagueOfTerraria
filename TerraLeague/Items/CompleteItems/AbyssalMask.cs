﻿using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class AbyssalMask : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Mask");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases maximum mana by 30" +
                "\nIncreases resist by 5" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = 200000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            player.statLifeMax2 += 30;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Catalyst>(), 1);
            recipe.AddIngredient(ItemType<NegatronCloak>(), 1);
            recipe.AddIngredient(ItemID.MimeMask, 1);
            recipe.AddIngredient(ItemType<VoidBar>(), 10);
            recipe.AddRecipeGroup("TerraLeague:EvilPartGroup", 10);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new AbyssalCurse();
        }

        public override Passive GetSecondaryPassive()
        {
            return new Eternity();
        }
    }
}
