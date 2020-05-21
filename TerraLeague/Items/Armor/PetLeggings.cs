using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PetLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Petrified Greaves");
            Tooltip.SetDefault("Decreases maximum mana by 20");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 0;
            item.rare = ItemRarityID.Blue;
            item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 -= 20;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PetWood>(), 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
