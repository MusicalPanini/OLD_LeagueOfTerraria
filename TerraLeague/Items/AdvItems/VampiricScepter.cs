﻿using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class VampiricScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vampiric Scepter");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n4% melee and ranged life steal" +
                "\n10% decreased maximum life" +
                "\n10% increased damage taken");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 60000;
            item.rare = 3;
            item.accessory = true;
            item.material = true;
            item.defense = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.02f;
            player.rangedDamage += 0.02f;

            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 0.04;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 0.04;
            player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddIngredient(ItemID.Ruby, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
