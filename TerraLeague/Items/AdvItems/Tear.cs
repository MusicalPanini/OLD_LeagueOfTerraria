﻿using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Tear : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tear of the Goddess");
            Tooltip.SetDefault("Increases maximum mana by 10" +
                "\nCan only have one AWE item equiped at a time");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 60000;
            item.rare = 3;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ItemType<FaerieCharm>(), 1);
            recipe.AddIngredient(ItemID.NaturesGift, 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 4);
            recipe.AddIngredient(ItemType<ManaBar>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new ManaCharge();
        }

        public override Passive GetSecondaryPassive()
        {
            return new Awe(6, 0, 0);
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
