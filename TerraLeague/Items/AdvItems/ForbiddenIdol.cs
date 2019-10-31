using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class ForbiddenIdol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Idol");
            Tooltip.SetDefault("Ability cooldown reduced by 5%" +
                "\nIncreases mana regeneration by 15%"+
                "\n5% increased healing power");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 50000;
            item.rare = 3;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.15;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.05;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.05;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<FaerieCharm>(), 2);
            recipe.AddIngredient(ItemType<VoidFragment>(), 20);
            recipe.AddRecipeGroup("TerraLeague:DemonPartGroup", 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
