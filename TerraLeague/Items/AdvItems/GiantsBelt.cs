using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class GiantsBelt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant's Belt");
            Tooltip.SetDefault("Increases maximum life by 40");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 7, 50, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 40;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemID.LifeCrystal, 2);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 8);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 4);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddIngredient(ItemID.Chain, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
