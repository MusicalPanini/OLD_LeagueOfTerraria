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
    public class Zephyr : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zephyr");
            Tooltip.SetDefault("5% increased melee and ranged damage" +
                "\n10% increased melee and ranged attack speed" +
                "\n10% increased movement speed" +
                "\nAbility cooldown reduced by 10%" +
                "\nImmunity to Slow and Chilled");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 75000;
            item.rare = 3;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.moveSpeed += 0.1f;
            player.meleeSpeed += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;

            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Chilled] = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<Stinger>(), 1);
            recipe.AddIngredient(ItemType<Dagger>(), 1);
            recipe.AddIngredient(ItemID.Cloud, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new WindPower();
        }

        public override string GetStatText()
        {
                return "";
        }
    }
}
