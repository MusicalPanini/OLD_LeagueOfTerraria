using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class ChainVest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chain Vest");
            Tooltip.SetDefault("Armor increased by 4");
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
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ClothArmor>(), 1);
            recipe.AddIngredient(ItemID.Granite, 20);
            recipe.AddIngredient(ItemID.Chain, 10);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
