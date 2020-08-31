using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class HextechEvolutionLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hextech Evolution Boots");
            Tooltip.SetDefault("8% reduced mana cost");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 250000;
            item.rare = ItemRarityID.Pink;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.manaCost -= 0.08f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 16);
            recipe.AddIngredient(ItemType<PerfectHexCore>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
