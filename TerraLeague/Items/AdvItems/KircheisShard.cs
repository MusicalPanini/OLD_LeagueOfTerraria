﻿using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class KircheisShard : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kircheis Shard");
            Tooltip.SetDefault("12% increased melee and ranged attack speed");
        }

        public override void SetDefaults()
        {
            item.width = 52;
            item.height = 20;
            item.value = 60000;
            item.rare = 3;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.12f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Dagger>(), 1);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Energized(10, 15);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            if (slot != -1)
            {
                if (!Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
