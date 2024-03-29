﻿using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Rylais : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rylai's Crystal Scepter");
            Tooltip.SetDefault("9% increased magic damage" +
                "\nIncreases health by 30");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 150000;
            item.rare = 4;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.09f;
            player.statLifeMax2 += 30;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<AmpTome>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 10);
            recipe.AddIngredient(ItemID.FrostStaff, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Frosty(2);
        }
    }
}
