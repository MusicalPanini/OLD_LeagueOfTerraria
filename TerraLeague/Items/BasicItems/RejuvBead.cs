using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class RejuvBead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rejuvenation Bead");
            Tooltip.SetDefault("Increases life regeneration by 1");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.value = Item.buyPrice(0, 2, 50, 0);
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 5);
            recipe.AddIngredient(ItemID.Rope, 1);
            recipe.AddIngredient(ItemID.Daybloom, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
