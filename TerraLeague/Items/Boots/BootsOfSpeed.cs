using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.Boots
{
    public class BootsOfSpeed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Speed");
            Tooltip.SetDefault("Lets you run faster");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 10000;
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T1Boots = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
