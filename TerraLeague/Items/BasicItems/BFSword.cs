using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class BFSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("B.F.Sword");
            Tooltip.SetDefault("4% increased melee and ranged damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBroadsword, 1);
            recipe.AddRecipeGroup("TerraLeague:IronGroup", 5);
            recipe.AddIngredient(ItemID.Leather, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.LeadBroadsword, 1);
            recipe2.AddRecipeGroup("TerraLeague:IronGroup", 5);
            recipe2.AddIngredient(ItemID.Leather, 2);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}
