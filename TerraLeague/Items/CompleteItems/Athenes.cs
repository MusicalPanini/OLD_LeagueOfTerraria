﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Athenes : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Athene's Unholy Grail");
            Tooltip.SetDefault("4% increased magic and minion damage" +
                "\nIncreases resist by 4" +
                "\nIncreases mana regeneration by 30%" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;
            player.magicDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.04;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Codex>(), 1);
            recipe.AddIngredient(ItemType<Chalice>(), 1);
            recipe.AddIngredient(ItemID.SoulofFright, 6);
            recipe.AddIngredient(ItemID.SoulofMight, 6);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.Deathweed, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Dissonance(1, 40);
        }

        public override Passive GetSecondaryPassive()
        {
            return new BloodPool(250);
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
