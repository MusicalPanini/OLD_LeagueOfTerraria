using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class CloakofAgility : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloak of Agility");
            Tooltip.SetDefault("5% increased ranged and melee critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 34;
            item.value = Item.buyPrice(0, 3, 75, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeCrit += 5;
            player.rangedCrit += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 25);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
