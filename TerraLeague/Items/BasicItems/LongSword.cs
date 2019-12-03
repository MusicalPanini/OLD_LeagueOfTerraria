using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class LongSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Long Sword");
            Tooltip.SetDefault("2% increased melee and ranged damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 5000;
            item.rare = 1;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.02f;
            player.rangedDamage += 0.02f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:IronGroup", 5);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.anyWood = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
