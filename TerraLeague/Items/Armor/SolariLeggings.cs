using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SolariLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Solari Greaves");
            Tooltip.SetDefault("Increases your max life by 20" +
                "\nEnemies are more likely to target you");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 145000 * 5;
            item.rare = ItemRarityID.Yellow;
            item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            player.aggro += 250;
            player.statLifeMax2 += 20;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 12);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
