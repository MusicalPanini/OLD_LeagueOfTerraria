using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class HextechEvolutionBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Hextech Evolution Breastplate");
            Tooltip.SetDefault("Increased maximum mana by 40" +
                "\n5% increased magic damage and critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 250000;
            item.rare = ItemRarityID.Pink;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 40;
            player.magicCrit += 5;
            player.magicDamage += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 20);
            recipe.AddIngredient(ItemType<PerfectHexCore>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
