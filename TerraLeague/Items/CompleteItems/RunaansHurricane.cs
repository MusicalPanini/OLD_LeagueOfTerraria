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
    public class RunaansHurricane : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runaan's Hurricane");
            Tooltip.SetDefault("15% increased ranged attack speed" +
                "\n8% increased ranged critical strike chance" +
                "\n7% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 36;
            item.value = 180000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedCrit += 8;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.15;
            player.moveSpeed += 0.07f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Dagger>(), 2);
            recipe.AddIngredient(ItemType<Zeal>(), 1);
            recipe.AddIngredient(ItemID.MoltenFury, 1);
            recipe.AddRecipeGroup("TerraLeague:Tier1Bar", 10);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new WindsFury(false);
        }
    }
}
